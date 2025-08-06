using System;
using GameCore.Database;
using GameCore.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToyHud : MonoBehaviour
{
    [SerializeField] private UIButton m_btnSelect;
    [SerializeField] private Image m_imageIcon;
    [SerializeField] private TextMeshProUGUI m_textToyName;
    public string toyKey { get; private set; }
    public int index { get; private set; }

    private void Awake()
    {
        m_btnSelect.data = this;
    }

    public void Apply(string toyKey , int index)
    {
        if (m_imageIcon == null)
        {
            return;
        }

        if (m_textToyName == null)
        {
            return;
        }

        this.toyKey = toyKey;
        this.index = index;
        Database<ToyData>.TryLoad(toyKey, out var toyData);
        if (toyData == null)
        {
            return;
        }

        m_imageIcon.sprite = toyData.toySprite;
        m_textToyName.text = toyData.toyName;
    }
    public void ApplyOnClick(Action<UIButton> onClick )
    {
        m_btnSelect.onClicked = onClick;
    }
}
