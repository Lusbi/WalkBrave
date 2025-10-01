using System;
using GameCore.UI;
using UnityEngine;

public class GameMainView : UIViewBase
{
    [SerializeField] private BattlePanel m_battlePanel;
    [SerializeField] private RoleScrollRect m_roleScrollRect;

    private PanelBase m_curPanel;

    public bool isBattlePanel => m_curPanel is BattlePanel;

#if UNITY_EDITOR
    protected override void CacheUIViews()
    {
        base.CacheUIViews();

        if (m_battlePanel == null)
            m_battlePanel = GetComponentInChildren<BattlePanel>();
    }
#endif


    public override void Initlization(Action callBack = null)
    {
        base.Initlization(callBack);

        m_battlePanel.Initlization();
        m_battlePanel.Apply(this);

    }
    public override void Active(bool isActive)
    {
        base.Active(isActive);

        if (isActive)
        {
            ActionCommand(CommandType.Battle);

        }
    }

    internal void ActionCommand(CommandType commandType)
    {
        switch (commandType)
        {
            case CommandType.Battle:
                Debug.Log("Battle Command Executed");
                m_curPanel?.Active(false);
                m_battlePanel.Active(true);
                m_curPanel = m_battlePanel;
                break;
            default:
                Debug.Log("Unknown Command Executed");
                break;
        }
    }

    private void Update()
    {
        if (m_battlePanel)
            m_battlePanel.Tick(Time.deltaTime);
    }

    public void ApplyRoleRect()
    {
        m_roleScrollRect.ListUpdate();
    }
}
