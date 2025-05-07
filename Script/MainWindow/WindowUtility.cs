using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class WindowUtility : MonoBehaviour
{
    [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
    private static extern int SetWindowLong(IntPtr hwnd, int index, int value);

    [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
    private static extern int GetWindowLong(IntPtr hwnd, int index);

    [DllImport("user32.dll", EntryPoint = "SetLayeredWindowAttributes")]
    private static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);

    [DllImport("user32.dll", EntryPoint = "GetActiveWindow")]
    private static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll", EntryPoint = "MoveWindow")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, [MarshalAs(UnmanagedType.Bool)] bool bRepaint);

    [DllImport("user32.dll", EntryPoint = "ReleaseCapture")]
    private static extern bool ReleaseCapture();

    [DllImport("user32.dll", EntryPoint = "SendMessage")]
    private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

    private const int GWL_STYLE = -16;
    private const int WS_BORDER = 0x00800000;
    private const int WS_CAPTION = 0x00C00000;
    private const int WS_SYSMENU = 0x00080000;
    private const int WS_THICKFRAME = 0x00040000;
    private const int WS_MINIMIZEBOX = 0x00020000;
    private const int WS_MAXIMIZEBOX = 0x00010000;

    private const int GWL_EXSTYLE = -20;
    private const int WS_EX_LAYERED = 0x80000;
    private const int WS_EX_TRANSPARENT = 0x20;
    private const uint LWA_COLORKEY = 0x1;
    private const uint LWA_ALPHA = 0x2;

    private const int WM_NCLBUTTONDOWN = 0xA1;
    private const int HTCAPTION = 0x2;

    private IntPtr hwnd;

    void Start()
    {
        hwnd = GetActiveWindow();
        int style = GetWindowLong(hwnd, GWL_STYLE);
        style &= ~(WS_BORDER | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX);
        SetWindowLong(hwnd, GWL_STYLE, style);

        int extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
        SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_LAYERED);
        SetLayeredWindowAttributes(hwnd, 0, 255, LWA_ALPHA); // 設置透明度

        // 設置窗口初始位置
        SetWindowPosition();
    }

    void Update()
    {
        // 檢查滑鼠左鍵是否按下並拖動窗口
        if (Input.GetMouseButtonDown(0))
        {
            ReleaseCapture();
            SendMessage(hwnd, WM_NCLBUTTONDOWN, HTCAPTION, 0);
        }
    }

    private void SetWindowPosition()
    {
        // 獲取屏幕寬度和高度
        int screenWidth = Screen.currentResolution.width;
        int screenHeight = Screen.currentResolution.height;

        // 設置窗口位置在屏幕底部中央
        int windowWidth = Screen.width;
        int windowHeight = Screen.height;
        int x = (screenWidth - windowWidth) / 2;
        int y = screenHeight - windowHeight;

        MoveWindow(hwnd, x, y, windowWidth, windowHeight, true);
    }
}

