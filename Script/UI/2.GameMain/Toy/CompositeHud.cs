using GameCore.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CompositeHud : MonoBehaviour
{
    [SerializeField] private Image m_imgIcon;
    [SerializeField] private TextMeshProUGUI m_txtTip;

    public void Apply(CompositeData compositeData)
    {
        if (m_imgIcon == null)
        {
            return;
        }
        if (m_txtTip == null)
        {
            return;
        }

        if (compositeData == null)
        {
            m_imgIcon.gameObject.SetActive(false);
            m_txtTip.text = string.Empty;
            return;
        }
        int curAmount = 0;
        ItemStorageData itemStorageData = StorageManager.instance.StorageData.GetItemStorageData(compositeData.itemReference.GetKey());
        if (itemStorageData != null)
        {
            curAmount = itemStorageData.ItemValue;
        }

        m_imgIcon.sprite = compositeData.itemReference.Load().itemIcon;
        m_txtTip.text = $"{curAmount}/{compositeData.amount}";
        m_txtTip.color = curAmount >= compositeData.amount ? Color.green : Color.red;

    }
}
