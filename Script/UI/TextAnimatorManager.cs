using Febucci.UI;
using UnityEngine;

public class TextAnimatorManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("預設會執行打字機效果的 TextAnimatorPlayer 物件")]
    private TextAnimatorPlayer m_defaultPlayer; // 預設的 TextAnimatorPlayer 參考，方便在 Prefab 內直接指定

    private TextAnimatorPlayer m_runtimePlayer; // 於執行時使用的 TextAnimatorPlayer 參考，若未指定則退回預設值

    private void Awake()
    {
        // 初始化時預設使用 Inspector 指定的 TextAnimatorPlayer
        m_runtimePlayer = m_defaultPlayer;
    }

    public void SetPlayer(TextAnimatorPlayer player)
    {
        // 允許外部指定要使用的 TextAnimatorPlayer，若傳入為 null 則仍會使用預設值
        m_runtimePlayer = player;
    }

    public void PlayText(string text, bool skipTypewriter = false)
    {
        // 取得實際要使用的 TextAnimatorPlayer，若兩者皆為 null 則直接警示並離開
        TextAnimatorPlayer targetPlayer = m_runtimePlayer != null ? m_runtimePlayer : m_defaultPlayer;
        if (targetPlayer == null)
        {
            Debug.LogWarning("TextAnimatorManager: 未設定 TextAnimatorPlayer，無法播放文字動畫");
            return;
        }

        // 設定要顯示的文字並啟動打字機效果
        targetPlayer.ShowText(text);
        targetPlayer.StartShowingText(true);

        // 若需要直接跳過打字效果則呼叫 SkipTypewriter 立即顯示全部文字
        if (skipTypewriter)
        {
            targetPlayer.SkipTypewriter();
        }
    }
}
