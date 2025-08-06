using GameCore.Database;
using GameCore.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BookItemHud : UIViewBase
{
    [SerializeField] private Image m_imgItemIcon;
    [SerializeField] private Image m_imgEnemyIcon;
    [SerializeField] private TextMeshProUGUI m_textHoldAmount;

    private void HUDReset()
    {
        m_imgItemIcon.gameObject.SetActive(false);
        m_textHoldAmount.text = string.Empty;
    }

    public void ApplyFromRole(RoleData roleData)
    {
        if (roleData == null)
        {
            HUDReset();
            return;
        }
        if (roleData.DropInfo.itemReference.Exists() == false)
        {
            HUDReset();
            return;
        }
        ApplyFromItem(roleData.DropInfo.itemReference.GetKey());
    }

    public void ApplyFromItem(string itemKey)
    {
        HUDReset();
        if (string.IsNullOrEmpty(itemKey))
        {
            return;
        }

        Database<ItemData>.TryLoad(itemKey, out var m_itemData);
        if (m_itemData == null)
            return;

        Sprite itemIcon = m_itemData.itemIcon;
        int holdAmount = 0;
        var m_itemStorageData = StorageManager.instance.StorageData.GetItemStorageData(itemKey);
        if (m_itemStorageData != null)
        {
            holdAmount = m_itemStorageData.ItemValue;
        }

        m_imgItemIcon.sprite = itemIcon;
        m_textHoldAmount.text = holdAmount > 0 ? holdAmount.ToString() : string.Empty;

        m_imgEnemyIcon.sprite = m_itemData.roleReference.Load().EnemyIcon;
        m_imgEnemyIcon.enabled = true;
        m_imgEnemyIcon.color = Color.white;

        m_imgItemIcon.gameObject.SetActive(true);
        m_imgEnemyIcon.gameObject.SetActive(true);
    }
}
