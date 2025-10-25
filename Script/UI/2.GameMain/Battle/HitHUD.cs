using TMPro;
using UnityEngine;

public class HitHUD : MonoBehaviour
{
    private const string k_aniNormal = "Hit_Normal";
    private const string k_aniTomato = "Hit_Tomato";
    private Animation m_ani;
    private TextMeshProUGUI m_text;
    private RectTransform m_rectTransform;
    public RectTransform rectTransform => m_rectTransform;
    void Awake()
    {
        m_ani = GetComponent<Animation>();
        m_text = GetComponent<TextMeshProUGUI>();
        m_rectTransform = GetComponent<RectTransform>();
    }
    public void Recycle()
    {
        m_text.text = string.Empty;
    }
    public void SetText(string hitValue)
    {
        m_text.text = hitValue;
    }
    public void PlayHit(bool isTomato = false)
    {
        if (isTomato)
        {
            m_ani.clip = m_ani.GetClip(k_aniTomato);
            m_ani.Play(k_aniTomato);
        }
        else
        {
            m_ani.clip = m_ani.GetClip(k_aniNormal);
            m_ani.Play(k_aniNormal);
        }
    }
}
