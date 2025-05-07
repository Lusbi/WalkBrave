using UnityEngine;

public class AppManager : MonoBehaviour
{
    public static AppManager Instance { get; private set; }

    [Header("退出設定")]
    [SerializeField] private KeyCode exitKey = KeyCode.Escape;
    [SerializeField] private GameObject exitConfirmPanel;

    [Header("系統托盤設定")]
    [SerializeField] private bool minimizeToTray = true;

    private void Awake()
    {
        // 單例模式
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // 確保退出面板最初是隱藏的
        if (exitConfirmPanel != null)
            exitConfirmPanel.SetActive(false);

        // 設置應用不會自動休眠
        Application.runInBackground = true;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    private void Update()
    {
        // 處理退出按鍵
        if (Input.GetKeyDown(exitKey))
        {
            //ToggleExitPanel();
            ExitApplication();
        }
    }

    public void ToggleExitPanel()
    {
        if (exitConfirmPanel != null)
        {
            exitConfirmPanel.SetActive(!exitConfirmPanel.activeSelf);
        }
    }

    public void ExitApplication()
    {
        Debug.Log("應用程式退出");
        Application.Quit();
    }

    public void MinimizeApplication()
    {
        // 在Unity播放器中無法實現，但在實際發布的應用程序中可以通過Windows API實現
        #if !UNITY_EDITOR
        // 實現最小化功能
        #endif
    }
}
