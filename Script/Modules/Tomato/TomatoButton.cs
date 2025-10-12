using GameCore.Log;
using GameCore.UI;
using TMPro;
using UnityEngine;

public class TomatoButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_minuteText;
    [SerializeField] private UIButton m_clickBtn;
    [SerializeField] private GameObject m_enableObj;
    [SerializeField] private GameObject m_disableObj;
    [SerializeField] private TextMeshProUGUI m_tipText;

    private TomatoManager m_tomatoManager;
    public void Initlization()
    {
        m_tomatoManager ??= TomatoManager.instance;
        if (m_clickBtn)
            m_clickBtn.onClicked = TriggerTomatoTime;

        if (m_minuteText)
            m_minuteText.text = string.Empty;

        SetTrigger(false);
    }

    public void SetTrigger(bool trigger)
    {
        m_enableObj.SetActive(trigger);
        m_disableObj.SetActive(!trigger);
    }

    public void ApplyTomatoTime()
    {
        m_minuteText.text = m_tomatoManager.curTomatoTimeString;
    }

    private void TriggerTomatoTime(UIButton uIButton)
    {
        if (m_tomatoManager.isTomatoTime)
        {
            eLog.Log($"���X���|������");
            return;
        }

        m_tomatoManager.TomatoTrigger();
    }
}
