using System;
using GameCore;
using UnityEngine;
using UnityEngine.Pool;

public class HitPanel : MonoBehaviour , IInitlization
{
    [SerializeField] private RectTransform m_defaultRect;
    [SerializeField] private RectTransform m_parentRect;
    [SerializeField] private HitHUD m_hitHUDTemplate;

    private ObjectPool<HitHUD> m_textPool ;

    public void Initlization(Action callBack = null)
    {
        m_textPool = new ObjectPool<HitHUD>(OnCreateText, OnGetText, OnReleaseText, null, false, 10);
    }

    private HitHUD OnCreateText()
    {
        HitHUD hitHud = GameObject.Instantiate<HitHUD>(m_hitHUDTemplate);
        hitHud.rectTransform.SetParent(m_defaultRect);
        hitHud.rectTransform.anchoredPosition = Vector3.zero;
        hitHud.rectTransform.localScale = Vector3.one;
        hitHud.gameObject.SetActive(false);
        return hitHud;
    }

    private void OnGetText(HitHUD uGUI)
    {
        uGUI.rectTransform.SetParent(m_parentRect);
        uGUI.rectTransform.anchoredPosition = UnityEngine.Random.insideUnitSphere * 50f ;
        uGUI.rectTransform.localScale = Vector3.one;
        uGUI.gameObject.SetActive(true);
    }

    private void OnReleaseText(HitHUD uGUI)
    {
        uGUI.rectTransform.SetParent(m_defaultRect);
        uGUI.rectTransform.anchoredPosition = Vector3.zero;
        uGUI.rectTransform.localScale = Vector3.one;
        uGUI.Recycle();
        uGUI.gameObject.SetActive(false);
    }


    public void PlayHit(string hitValue , bool isTomato = false)
    {
        var ui = m_textPool.Get();
        ui.SetText(hitValue);
        ui.PlayHit(isTomato);
    }
}
