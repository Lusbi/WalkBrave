using GameCore.UI;
using UnityEngine;

/// <summary>
/// �C���y�{�J�f
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
    /// �]�w�@�� Manager ��l��
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
