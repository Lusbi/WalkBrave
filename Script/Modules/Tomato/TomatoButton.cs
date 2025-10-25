using System;
using GameCore.Database;
using GameCore.Event;
using GameCore.UI;
using TMPro;
using UnityEngine;

public class TomatoButton : MonoBehaviour
{
    private const string k_useageLocalization = "Tomato.UseageTip";
    private const string k_lockedLocalization = "Tomato.DontClose";
    [SerializeField] private CanvasGroup m_canvasGroup;
    [SerializeField] private TextMeshProUGUI m_minuteText;
    [SerializeField] private UIButton m_clickBtn;
    [SerializeField] private GameObject m_enableObj;
    [SerializeField] private GameObject m_disableObj;
    [SerializeField] private TextMeshProUGUI m_tipText;
    [SerializeField] private FlagReference m_flagReference;
    private TomatoManager m_tomatoManager;
    private EventListener m_eventListener;
    public void Initlization()
    {
        m_tomatoManager ??= TomatoManager.instance;
        if (m_clickBtn)
            m_clickBtn.onClicked = TriggerTomatoTime;

        if (m_minuteText)
            m_minuteText.text = string.Empty;

        if (m_eventListener == null)
        {
            m_eventListener = new EventListener();
            m_eventListener.AddListener<FlagUpdateEvent>(OnFlagUpdateEvent, true);
        }
        // 遊戲開始時檢查一次旗標決定是否開放
        OnFlagUpdateEvent(new FlagUpdateEvent(m_flagReference.GetKey()));
        SetTrigger(false);
    }

    private void OnFlagUpdateEvent(FlagUpdateEvent eventData)
    {
        if (StorageManager.instance.StorageData.GetFlagStorageValue(eventData.flagKey) > 0)
        {
            TomatoState(true);
        }
        else
        {
            TomatoState(false);
        }
    }

    public void SetTrigger(bool trigger)
    {
        m_enableObj.SetActive(trigger);
        m_disableObj.SetActive(!trigger);

        UpdateTipText(trigger);
    }

    public void ApplyTomatoTime()
    {
        m_minuteText.text = m_tomatoManager.curTomatoTimeString;
    }

    private void TriggerTomatoTime(UIButton uIButton)
    {
        if (m_tomatoManager.isTomatoTime)
        {
            return;
        }

        m_tomatoManager.TomatoTrigger();
    }

    private void UpdateTipText(bool state)
    {
        if (m_tipText == null)
            return;
        if (state)
            m_tipText.text = LocalizationManager.instance.GetLocalization(k_lockedLocalization);
        else
            m_tipText.text = LocalizationManager.instance.GetLocalization(k_useageLocalization);

    }

    private void TomatoState(bool state)
    {
        m_canvasGroup.alpha = state ? 1 : 0;
        m_canvasGroup.blocksRaycasts = state;
    }
}
