using Febucci.UI;
using GameCore;
using TMPro;
using UnityEngine;

public class TalkManager : MonoSingleton<TalkManager>
{
    [SerializeField] private TextMeshProUGUI m_talkText;
    [SerializeField] private TextAnimatorPlayer m_textAnimatorPlayer;

    public void Play(string contentKey)
    {
        m_textAnimatorPlayer.ShowText(contentKey);
    }
}
