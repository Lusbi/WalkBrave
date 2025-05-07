using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    #region Windows API 導入
    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

    [DllImport("user32.dll")]
    private static extern uint GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    [DllImport("user32.dll")]
    private static extern bool SetLayeredWindowAttributes(IntPtr hWnd, uint crKey, byte bAlpha, uint dwFlags);

    [DllImport("user32.dll")]
    private static extern bool ReleaseCapture();

    [DllImport("user32.dll")]
    private static extern bool SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

    [DllImport("user32.dll")]
    private static extern bool UpdateWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern bool RedrawWindow(IntPtr hWnd, IntPtr lprcUpdate, IntPtr hrgnUpdate, uint flags);

    [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
    private static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);
    #endregion

    #region Windows API 常數
    private const int GWL_STYLE = -16;
    private const int GWL_EXSTYLE = -20;
    private const uint WS_POPUP = 0x80000000;
    private const uint WS_EX_LAYERED = 0x00080000;
    private const uint WS_EX_TRANSPARENT = 0x00000020;
    private const uint WS_EX_COMPOSITED = 0x02000000; // 防止窗口閃爍
    private const uint WS_EX_TOOLWINDOW = 0x00000080; // 工具窗口，不在任務欄顯示
    private const uint LWA_COLORKEY = 0x00000001;
    private const uint LWA_ALPHA = 0x00000002;
    private const int WM_NCLBUTTONDOWN = 0xA1;
    private const int HTCAPTION = 0x2;

    // RedrawWindow flags
    private const uint RDW_INVALIDATE = 0x0001;
    private const uint RDW_INTERNALPAINT = 0x0002;
    private const uint RDW_ERASE = 0x0004;
    private const uint RDW_ALLCHILDREN = 0x0080;
    private const uint RDW_UPDATENOW = 0x0100;
    private const uint RDW_FRAME = 0x0400;

    // 用於SetWindowPos的特殊句柄和標誌
    private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
    private const uint SWP_NOSIZE = 0x0001;
    private const uint SWP_NOMOVE = 0x0002;
    private const uint SWP_SHOWWINDOW = 0x0040;
    private const uint SWP_FRAMECHANGED = 0x0020; // 通知窗口框架改變
    #endregion

    // 視窗句柄
    private IntPtr windowHandle;

    // 拖動相關
    [SerializeField] private bool enableDragging = true;

    // 視窗設置
    [SerializeField] private bool alwaysOnTop = true;
    [SerializeField] private Color transparentColor = Color.black;
    [SerializeField] private byte windowAlpha = 255;

    // 視窗位置
    [SerializeField] private Vector2 initialPosition = new Vector2(0, 0);
    [SerializeField] private bool centerScreenHorizontally = true;
    [SerializeField] private bool positionAtBottom = true;

    // 拖動控制區域（如果不是整個窗口可拖動）
    [SerializeField] private GameObject dragHandle;

    // 滑鼠軌跡消除設定
    [SerializeField] private float redrawInterval = 0.1f; // 重繪間隔時間
    private float redrawTimer;

    private Camera mainCamera;
    private bool initialized = false;

    private void Awake()
    {
        mainCamera = Camera.main;

        // 確保主相機的背景是透明的
        if (mainCamera != null)
        {
            mainCamera.clearFlags = CameraClearFlags.SolidColor;
            mainCamera.backgroundColor = new Color(0, 0, 0, 0);
        }

        // 禁用光標軌跡
        Cursor.visible = true;
    }

    private void Start()
    {
        // 等待一幀確保窗口創建完成
        StartCoroutine(InitializeAfterDelay());
    }

    private System.Collections.IEnumerator InitializeAfterDelay()
    {
        yield return null; // 等待一幀
        yield return new WaitForSeconds(0.1f); // 再多等一點時間確保窗口完全創建
        InitializeWindow();
    }

    private void InitializeWindow()
    {
        if (initialized)
            return;

        windowHandle = GetActiveWindow();

        // 設置窗口樣式（無邊框）
        uint style = GetWindowLong(windowHandle, GWL_STYLE);
        style &= ~(uint)(0x00C00000); // 移除WS_CAPTION
        style &= ~(uint)(0x00800000); // 移除WS_BORDER
        style = WS_POPUP; // 使用彈出窗口樣式
        SetWindowLong(windowHandle, GWL_STYLE, style);

        // 設置擴展樣式（透明、分層、合成）
        uint exStyle = GetWindowLong(windowHandle, GWL_EXSTYLE);
        exStyle |= WS_EX_LAYERED | WS_EX_COMPOSITED | WS_EX_TOOLWINDOW;
        if (!enableDragging)
            exStyle |= WS_EX_TRANSPARENT; // 如果不允許拖動，則設置為透明（點擊穿透）
        SetWindowLong(windowHandle, GWL_EXSTYLE, exStyle);

        // 設置透明色和透明度
        uint transparentColorKey = (uint)(((int)(transparentColor.b * 255)) |
                                 (((int)(transparentColor.g * 255)) << 8) |
                                 (((int)(transparentColor.r * 255)) << 16));
        SetLayeredWindowAttributes(windowHandle, transparentColorKey, windowAlpha, LWA_COLORKEY | LWA_ALPHA);

        // 設置窗口位置
        SetWindowPosition();

        // 如果需要置頂
        if (alwaysOnTop)
        {
            SetWindowPos(windowHandle, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_SHOWWINDOW | SWP_FRAMECHANGED);
        }

        // 強制窗口重繪
        RedrawWindow(windowHandle, IntPtr.Zero, IntPtr.Zero, RDW_INVALIDATE | RDW_UPDATENOW | RDW_FRAME | RDW_ALLCHILDREN | RDW_ERASE);
        UpdateWindow(windowHandle);

        initialized = true;
    }

    private void SetWindowPosition()
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

        SetWindowPos(windowHandle, IntPtr.Zero, x, y, windowWidth, windowHeight, SWP_FRAMECHANGED);
    }

    private void Update()
    {
        // 處理拖動
        if (enableDragging)
        {
            bool shouldDrag = false;

            // 如果指定了拖動句柄，檢查是否點擊了該句柄
            if (dragHandle != null)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Vector2 mousePos = Input.mousePosition;
                    Ray ray = mainCamera.ScreenPointToRay(mousePos);
                    RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

                    if (hit.collider != null && hit.collider.gameObject == dragHandle)
                    {
                        shouldDrag = true;
                    }
                }
            }
            // 否則整個窗口可拖動
            else if (Input.GetMouseButtonDown(0))
            {
                shouldDrag = true;
            }

            if (shouldDrag)
            {
                ReleaseCapture();
                SendMessage(windowHandle, WM_NCLBUTTONDOWN, HTCAPTION, 0);

                // 拖動時立即重繪
                ForceRedraw();
            }
        }

        // 定期重繪窗口以消除滑鼠軌跡
        redrawTimer -= Time.deltaTime;
        if (redrawTimer <= 0)
        {
            redrawTimer = redrawInterval;
            ForceRedraw();
        }
    }

    private void ForceRedraw()
    {
        if (windowHandle != IntPtr.Zero)
        {
            RedrawWindow(windowHandle, IntPtr.Zero, IntPtr.Zero, RDW_INVALIDATE | RDW_UPDATENOW | RDW_FRAME);
            UpdateWindow(windowHandle);
        }
    }

    private void OnApplicationQuit()
    {
        // 應用退出時確保窗口正確釋放
        if (windowHandle != IntPtr.Zero)
        {
            SetLayeredWindowAttributes(windowHandle, 0, 255, LWA_ALPHA);
        }
    }
}
