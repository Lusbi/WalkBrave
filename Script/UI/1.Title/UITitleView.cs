using GameCore.UI;
using UnityEngine;

public class UITitleView : UIViewBase
{
    [SerializeField] private UIButton m_btn;

    public override void ViewEnable()
    {
        base.ViewEnable();

        if (m_btn.onClicked is null)
            m_btn.SetOnClick(EnterGame);
    }

    public override void ViewDisable()
    {
        base.ViewDisable();
    }

    private void EnterGame(UIButton obj)
    {
        UIManager.instance.ChangeView(UIEnum.GameMain);
    }
}
