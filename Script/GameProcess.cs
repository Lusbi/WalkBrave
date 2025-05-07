using GameCore.UI;
using UnityEngine;

/// <summary>
/// 遊戲流程入口
/// </summary>
public class GameProcess : MonoBehaviour
{
    public static GameProcess Instance { get; private set; }
    public bool ForceNewGame = true;

    private void OnApplicationQuit()
    {
        StorageManager.instance.SaveStorageData();
    }

    private void Awake()
    {
        DontDestroyOnLoad(this);
        Instance = this;
    }

    private void Start()
    {
        GameCore.Database.DatabaseManager.instance.Initialize(()=>Setup());
    }

    /// <summary>
    /// 設定一些 Manager 初始化
    /// </summary>
    private void Setup()
    {
        UIManager.instance.Initlization(ChangeToTitle);

        if (ForceNewGame == false)
            StorageManager.instance.LoadStorageData();
        else
            StorageManager.instance.SetStorageData(new StorageData());
    }

    private void ChangeToTitle()
    {
        UIManager.instance.ChangeView(UIEnum.Title);
    }
}
