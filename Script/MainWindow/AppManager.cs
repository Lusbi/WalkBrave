using UnityEngine;

public class AppManager : MonoBehaviour
{
    public static AppManager Instance { get; private set; }

    [Header("�h�X�]�w")]
    [SerializeField] private KeyCode exitKey = KeyCode.Escape;
    [SerializeField] private GameObject exitConfirmPanel;

    [Header("�t�Φ��L�]�w")]
    [SerializeField] private bool minimizeToTray = true;

    private void Awake()
    {
        // ��ҼҦ�
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

        // �T�O�h�X���O�̪�O���ê�
        if (exitConfirmPanel != null)
            exitConfirmPanel.SetActive(false);

        // �]�m���Τ��|�۰ʥ�v
        Application.runInBackground = true;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    private void Update()
    {
        // �B�z�h�X����
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
        Debug.Log("���ε{���h�X");
        Application.Quit();
    }

    public void MinimizeApplication()
    {
        // �bUnity���񾹤��L�k��{�A���b��ڵo�������ε{�Ǥ��i�H�q�LWindows API��{
        #if !UNITY_EDITOR
        // ��{�̤p�ƥ\��
        #endif
    }
}
