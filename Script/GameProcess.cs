using GameCore.UI;
using UnityEngine;

/// <summary>
/// 遊戲流程入口
/// </summary>
public class GameProcess : MonoBehaviour
{
    public static GameProcess Instance { get; private set; }

    private void Awake()
    {
        DontDestroyOnLoad(this);
        Instance = this;
    }

    private void Start()
    {
        Setup();
    }

    /// <summary>
    /// 設定一些 Manager 初始化
    /// </summary>
    private void Setup()
    {
        UIManager.instance.Initlization(ChangeToTitle);
    }

    private void ChangeToTitle()
    {
        UIManager.instance.ChangeView(UIEnum.Title);
    }
}
