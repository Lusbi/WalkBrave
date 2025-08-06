using GameCore.Database;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BookToyHUD : UITitleView
{
    [SerializeField] private Image m_imageIcon;
    [SerializeField] private TextMeshProUGUI m_textHasHold;

    private void HUDReset()
    {
        m_imageIcon.gameObject.SetActive(false);
        m_textHasHold.text = string.Empty;
    }
    public void ApplyToy(string toyKey)
    {
        HUDReset();
        if (string.IsNullOrEmpty(toyKey))
        {
            return;
        }

        Database<ToyData>.TryLoad(toyKey, out var toyData);
        if (toyData == null)
            return;

        Sprite itemIcon = toyData.toySprite;
        int holdAmount = 0;
        var m_itemStorageData = StorageManager.instance.StorageData.GetToyStorageData(toyKey);
        if (m_itemStorageData != null)
        {
            holdAmount = m_itemStorageData.UseableValue;
        }

        m_imageIcon.sprite = itemIcon;
        m_textHasHold.text = holdAmount > 0 ? $"<color=#33ff33>{holdAmount}</color>" : "<color=#ff3333>-</color>";
        m_imageIcon.gameObject.SetActive(m_imageIcon.sprite != null);
    }
}
