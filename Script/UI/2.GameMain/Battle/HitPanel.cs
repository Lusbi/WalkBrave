using System;
using GameCore;
using GameCore.Log;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class HitPanel : MonoBehaviour , IInitlization
{
    [SerializeField] private RectTransform m_defaultRect;
    [SerializeField] private RectTransform m_parentRect;
    [SerializeField] private TextMeshProUGUI m_hitTextTemplate;

    private ObjectPool<TextMeshProUGUI> m_textPool ;

    public void Initlization(Action callBack = null)
    {
        m_textPool = new ObjectPool<TextMeshProUGUI>(OnCreateText, OnGetText, OnReleaseText, null, false, 10);
    }

    private TextMeshProUGUI OnCreateText()
    {
        TextMeshProUGUI textMeshProUGUI = GameObject.Instantiate<TextMeshProUGUI>(m_hitTextTemplate);
        textMeshProUGUI.rectTransform.SetParent(m_defaultRect);
        textMeshProUGUI.rectTransform.anchoredPosition = Vector3.zero;
        textMeshProUGUI.rectTransform.localScale = Vector3.one;
        textMeshProUGUI.gameObject.SetActive(false);
        return textMeshProUGUI;
    }

    private void OnGetText(TextMeshProUGUI uGUI)
    {
        uGUI.rectTransform.SetParent(m_parentRect);
        uGUI.rectTransform.anchoredPosition = UnityEngine.Random.insideUnitSphere * 50f ;
        uGUI.rectTransform.localScale = Vector3.one;
        uGUI.gameObject.SetActive(true);
    }

    private void OnReleaseText(TextMeshProUGUI uGUI)
    {
        uGUI.rectTransform.SetParent(m_defaultRect);
        uGUI.rectTransform.anchoredPosition = Vector3.zero;
        uGUI.rectTransform.localScale = Vector3.one;
        uGUI.gameObject.SetActive(false);
    }


    public void PlayHit(string hitValue)
    {
        var ui = m_textPool.Get();
        ui.text = hitValue;

        ui.GetComponent<UIDropCurvePrimeTween>().PlayOnce(() => 
        {
            m_textPool.Release(ui);
        }
        );
    }
}
