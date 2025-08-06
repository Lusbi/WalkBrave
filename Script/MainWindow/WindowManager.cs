using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    #region Native Window API
    [DllImport("user32.dll")] private static extern IntPtr GetActiveWindow();
    [DllImport("user32.dll")] private static extern bool IsWindow(IntPtr hWnd);
    [DllImport("user32.dll")] private static extern bool IsWindowVisible(IntPtr hWnd);
    [DllImport("user32.dll")] private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
    [DllImport("user32.dll")] private static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);
    [DllImport("user32.dll")] private static extern uint GetWindowLong(IntPtr hWnd, int nIndex);
    [DllImport("user32.dll")] private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
    [DllImport("user32.dll")] private static extern bool SetLayeredWindowAttributes(IntPtr hWnd, uint crKey, byte bAlpha, uint dwFlags);
    [DllImport("user32.dll")] private static extern bool ReleaseCapture();
    [DllImport("user32.dll")] private static extern bool SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
    [DllImport("user32.dll")] private static extern bool UpdateWindow(IntPtr hWnd);
    [DllImport("user32.dll")] private static extern bool RedrawWindow(IntPtr hWnd, IntPtr lprcUpdate, IntPtr hrgnUpdate, uint flags);
    #endregion

    #region Window Constants
    private const int GWL_STYLE = -16;
    private const int GWL_EXSTYLE = -20;
    private const int SW_SHOW = 5;
    private const int WM_NCLBUTTONDOWN = 0xA1;
    private const int HTCAPTION = 0x2;

    private const uint WS_POPUP = 0x80000000;
    private const uint WS_EX_LAYERED = 0x00080000;
    private const uint WS_EX_COMPOSITED = 0x02000000;
    private const uint LWA_COLORKEY = 0x00000001;
    private const uint LWA_ALPHA = 0x00000002;
    private const uint RDW_INVALIDATE = 0x0001;
    private const uint RDW_UPDATENOW = 0x0100;
    private const uint SWP_FRAMECHANGED = 0x0020;
    private const uint SWP_SHOWWINDOW = 0x0040;
    private const uint SWP_NOMOVE = 0x0002;
    private const uint SWP_NOSIZE = 0x0001;

    private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
    #endregion

    #region Serialized Fields
    [Header("Window Settings")]
    [SerializeField] private bool enableDragging = true;
    [SerializeField] private bool alwaysOnTop = true;
    [SerializeField] private Color transparentColor = Color.black;
    [SerializeField] private byte windowAlpha = 255;
    [SerializeField] private Vector2 initialPosition = Vector2.zero;
    [SerializeField] private bool centerScreenHorizontally = true;
    [SerializeField] private bool positionAtBottom = true;
    [SerializeField] private GameObject dragHandle;

    [Header("Performance Settings")]
    [SerializeField] private int targetFrameRate = 60;
    [SerializeField] private float redrawInterval = 0.033f;

    [Header("Mouse Settings")]
    [SerializeField, Range(0.1f, 3.0f)] private float mouseSensitivity = 1.0f;
    [SerializeField] private bool useMouseSmoothing = true;
    [SerializeField, Range(0.0f, 1.0f)] private float mouseSmoothing = 0.5f;
    #endregion

    #region Private Fields
    private IntPtr windowHandle;
    private Camera mainCamera;
    private bool initialized;
    private float redrawTimer;
    private Vector2 currentMousePosition;
    private Vector2 smoothedMousePosition;
    private readonly RaycastHit2D[] raycastResults = new RaycastHit2D[1];
    #endregion

    #region Unity Lifecycle
    private void Awake()
    {
        InitializeBasicSettings();
    }

    private void Start()
    {
        StartCoroutine(InitializeWindowCoroutine());
    }

    private void Update()
    {
        if (!initialized) return;
        HandleDragging();
        UpdateWindowVisibility();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus && initialized && windowHandle != IntPtr.Zero)
        {
            ShowWindow(windowHandle, SW_SHOW);
        }
    }

    private void OnApplicationQuit()
    {
        if (windowHandle != IntPtr.Zero)
        {
            SetLayeredWindowAttributes(windowHandle, 0, 255, LWA_ALPHA);
        }
    }
    #endregion

    #region Initialization
    private void InitializeBasicSettings()
    {
        mainCamera = Camera.main;
        if (mainCamera != null)
        {
            mainCamera.clearFlags = CameraClearFlags.SolidColor;
            mainCamera.backgroundColor = Color.clear;
        }

        currentMousePosition = Input.mousePosition;
        smoothedMousePosition = currentMousePosition;
        Application.targetFrameRate = targetFrameRate;
        QualitySettings.vSyncCount = 0;
        Cursor.visible = true;
    }

    private System.Collections.IEnumerator InitializeWindowCoroutine()
    {
        yield return null;
        yield return new WaitForSeconds(0.1f);

        windowHandle = GetActiveWindow();
        if (windowHandle == IntPtr.Zero || !IsWindow(windowHandle))
        {
            Debug.LogError("Failed to get valid window handle!");
            yield break;
        }

        SetupWindowStyle();
        SetupWindowPosition();
        ShowWindow(windowHandle, SW_SHOW);

        initialized = true;
    }

    private void SetupWindowStyle()
    {
        uint style = GetWindowLong(windowHandle, GWL_STYLE);
        style &= ~(uint)(0x00C00000 | 0x00800000);
        style |= WS_POPUP;
        SetWindowLong(windowHandle, GWL_STYLE, style);

        uint exStyle = GetWindowLong(windowHandle, GWL_EXSTYLE);
        exStyle |= WS_EX_LAYERED | WS_EX_COMPOSITED;
        SetWindowLong(windowHandle, GWL_EXSTYLE, exStyle);

        SetLayeredWindowAttributes(windowHandle, ColorToUInt(transparentColor), windowAlpha, LWA_COLORKEY | LWA_ALPHA);

        if (alwaysOnTop)
        {
            SetWindowPos(windowHandle, HWND_TOPMOST, 0, 0, 0, 0,
                SWP_NOMOVE | SWP_NOSIZE | SWP_SHOWWINDOW | SWP_FRAMECHANGED);
        }
    }

    private void SetupWindowPosition()
    {
        int screenWidth = Screen.currentResolution.width;
        int screenHeight = Screen.currentResolution.height;
        int windowWidth = Screen.width;
        int windowHeight = Screen.height;

        int x = (int)initialPosition.x;
        int y = (int)initialPosition.y;

        if (centerScreenHorizontally)
            x = (screenWidth - windowWidth) / 2;
        if (positionAtBottom)
            y = screenHeight - windowHeight;

        SetWindowPos(windowHandle, IntPtr.Zero, x, y, windowWidth, windowHeight,
            SWP_FRAMECHANGED | SWP_SHOWWINDOW);
    }
    #endregion

    #region Window Operations
    private void HandleDragging()
    {
        if (!enableDragging || !Input.GetMouseButtonDown(0)) return;

        Vector2 mousePos = GetSmoothedMousePosition();
        if (CheckDragHandle(mousePos))
        {
            ReleaseCapture();
            SendMessage(windowHandle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            ForceRedraw();
        }
    }

    private void UpdateWindowVisibility()
    {
        if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0))
        {
            redrawTimer -= Time.deltaTime;
            if (redrawTimer <= 0)
            {
                redrawTimer = redrawInterval;
                ForceRedraw();
            }
        }

        if (!IsWindowVisible(windowHandle))
        {
            ShowWindow(windowHandle, SW_SHOW);
        }
    }

    private void ForceRedraw()
    {
        if (windowHandle != IntPtr.Zero)
        {
            RedrawWindow(windowHandle, IntPtr.Zero, IntPtr.Zero, RDW_INVALIDATE | RDW_UPDATENOW);
            UpdateWindow(windowHandle);
        }
    }
    #endregion

    #region Helper Methods
    private Vector2 GetSmoothedMousePosition()
    {
        if (!useMouseSmoothing)
            return Input.mousePosition;

        currentMousePosition = Input.mousePosition;
        smoothedMousePosition = Vector2.Lerp(
            smoothedMousePosition,
            currentMousePosition,
            (1f - mouseSmoothing) * Time.deltaTime * 60f
        );

        return smoothedMousePosition * mouseSensitivity;
    }

    private bool CheckDragHandle(Vector2 mousePos)
    {
        if (dragHandle == null)
            return true;

        var ray = mainCamera.ScreenPointToRay(mousePos);
        int hitCount = Physics2D.RaycastNonAlloc(ray.origin, ray.direction, raycastResults);

        return hitCount > 0 && raycastResults[0].collider != null &&
               raycastResults[0].collider.gameObject == dragHandle;
    }

    private uint ColorToUInt(Color color)
    {
        return (uint)(
            ((int)(color.b * 255)) |
            ((int)(color.g * 255) << 8) |
            ((int)(color.r * 255) << 16)
        );
    }
    #endregion

#if UNITY_EDITOR
    private void OnGUI()
    {
        GUILayout.Label($"FPS: {1.0f / Time.deltaTime:F1}");
        GUILayout.Label($"Mouse Position: {Input.mousePosition}");
        GUILayout.Label($"Smoothed Position: {smoothedMousePosition}");
    }
#endif
}
