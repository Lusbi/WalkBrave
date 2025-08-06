using GameCore.Database;
using UnityEngine;
using UnityEngine.UI;

public class DropItem : MonoBehaviour
{
    [SerializeField] private CanvasGroup m_canvasGroup;
    [SerializeField] private Image m_image;

    public void SetItemKey(string itemKey)
    {
        if (string.IsNullOrEmpty(itemKey))
        {
            m_canvasGroup.alpha = 0f;
            m_image.sprite = null;
            return;
        }
        if (Database<ItemData>.TryLoad(itemKey, out var itemData))
        {
            m_image.sprite = itemData.itemIcon;
            m_canvasGroup.alpha = 1f;
        }
        else
        {
            m_canvasGroup.alpha = 0f;
            m_image.sprite = null;
        }
    }
}
