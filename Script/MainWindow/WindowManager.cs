using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    #region Windows API �ɤJ
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

    #region Windows API �`��
    private const int GWL_STYLE = -16;
    private const int GWL_EXSTYLE = -20;
    private const uint WS_POPUP = 0x80000000;
    private const uint WS_EX_LAYERED = 0x00080000;
    private const uint WS_EX_TRANSPARENT = 0x00000020;
    private const uint WS_EX_COMPOSITED = 0x02000000; // ����f�{�{
    private const uint WS_EX_TOOLWINDOW = 0x00000080; // �u�㵡�f�A���b���������
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

    // �Ω�SetWindowPos���S��y�`�M�лx
    private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
    private const uint SWP_NOSIZE = 0x0001;
    private const uint SWP_NOMOVE = 0x0002;
    private const uint SWP_SHOWWINDOW = 0x0040;
    private const uint SWP_FRAMECHANGED = 0x0020; // �q�����f�ج[����
    #endregion

    // �����y�`
    private IntPtr windowHandle;

    // ��ʬ���
    [SerializeField] private bool enableDragging = true;

    // �����]�m
    [SerializeField] private bool alwaysOnTop = true;
    [SerializeField] private Color transparentColor = Color.black;
    [SerializeField] private byte windowAlpha = 255;

    // ������m
    [SerializeField] private Vector2 initialPosition = new Vector2(0, 0);
    [SerializeField] private bool centerScreenHorizontally = true;
    [SerializeField] private bool positionAtBottom = true;

    // ��ʱ���ϰ�]�p�G���O��ӵ��f�i��ʡ^
    [SerializeField] private GameObject dragHandle;

    // �ƹ��y������]�w
    [SerializeField] private float redrawInterval = 0.1f; // ��ø���j�ɶ�
    private float redrawTimer;

    private Camera mainCamera;
    private bool initialized = false;

    private void Awake()
    {
        mainCamera = Camera.main;

        // �T�O�D�۾����I���O�z����
        if (mainCamera != null)
        {
            mainCamera.clearFlags = CameraClearFlags.SolidColor;
            mainCamera.backgroundColor = new Color(0, 0, 0, 0);
        }

        // �T�Υ��Эy��
        Cursor.visible = true;
    }

    private void Start()
    {
        // ���ݤ@�V�T�O���f�Ыا���
        StartCoroutine(InitializeAfterDelay());
    }

    private System.Collections.IEnumerator InitializeAfterDelay()
    {
        yield return null; // ���ݤ@�V
        yield return new WaitForSeconds(0.1f); // �A�h���@�I�ɶ��T�O���f�����Ы�
        InitializeWindow();
    }

    private void InitializeWindow()
    {
        if (initialized)
            return;

        windowHandle = GetActiveWindow();

        // �]�m���f�˦��]�L��ء^
        uint style = GetWindowLong(windowHandle, GWL_STYLE);
        style &= ~(uint)(0x00C00000); // ����WS_CAPTION
        style &= ~(uint)(0x00800000); // ����WS_BORDER
        style = WS_POPUP; // �ϥμu�X���f�˦�
        SetWindowLong(windowHandle, GWL_STYLE, style);

        // �]�m�X�i�˦��]�z���B���h�B�X���^
        uint exStyle = GetWindowLong(windowHandle, GWL_EXSTYLE);
        exStyle |= WS_EX_LAYERED | WS_EX_COMPOSITED | WS_EX_TOOLWINDOW;
        if (!enableDragging)
            exStyle |= WS_EX_TRANSPARENT; // �p�G�����\��ʡA�h�]�m���z���]�I����z�^
        SetWindowLong(windowHandle, GWL_EXSTYLE, exStyle);

        // �]�m�z����M�z����
        uint transparentColorKey = (uint)(((int)(transparentColor.b * 255)) |
                                 (((int)(transparentColor.g * 255)) << 8) |
                                 (((int)(transparentColor.r * 255)) << 16));
        SetLayeredWindowAttributes(windowHandle, transparentColorKey, windowAlpha, LWA_COLORKEY | LWA_ALPHA);

        // �]�m���f��m
        SetWindowPosition();

        // �p�G�ݭn�m��
        if (alwaysOnTop)
        {
            SetWindowPos(windowHandle, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_SHOWWINDOW | SWP_FRAMECHANGED);
        }

        // �j��f��ø
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
        // �B�z���
        if (enableDragging)
        {
            bool shouldDrag = false;

            // �p�G���w�F��ʥy�`�A�ˬd�O�_�I���F�ӥy�`
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
            // �_�h��ӵ��f�i���
            else if (Input.GetMouseButtonDown(0))
            {
                shouldDrag = true;
            }

            if (shouldDrag)
            {
                ReleaseCapture();
                SendMessage(windowHandle, WM_NCLBUTTONDOWN, HTCAPTION, 0);

                // ��ʮɥߧY��ø
                ForceRedraw();
            }
        }

        // �w����ø���f�H�����ƹ��y��
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
        // ���ΰh�X�ɽT�O���f���T����
        if (windowHandle != IntPtr.Zero)
        {
            SetLayeredWindowAttributes(windowHandle, 0, 255, LWA_ALPHA);
        }
    }
}
