using System;
using GameCore.UI;
using UnityEngine;

public class GameMainView : UIViewBase
{
    [SerializeField] private DesktopPetController m_petController;
    [SerializeField] private CommandBase[] m_commands;

    [SerializeField] private HomePanel m_homePanel;
    [SerializeField] private BattlePanel m_battlePanel;

    [SerializeField] private StagePanel m_stagePanel;

    private PanelBase m_curPanel;

    public bool isBattlePanel => m_curPanel is BattlePanel;

#if UNITY_EDITOR
    protected override void CacheUIViews()
    {
        base.CacheUIViews();

        if (m_battlePanel == null)
            m_battlePanel = GetComponentInChildren<BattlePanel>();
        if (m_commands == null || m_commands.Length == 0)
            m_commands = GetComponentsInChildren<CommandBase>(true);
        if (m_homePanel == null)
            m_homePanel = GetComponentInChildren<HomePanel>();
        if (m_stagePanel == null)
            m_stagePanel = GetComponentInChildren<StagePanel>();
    }
#endif

    public DesktopPetController desktopPetController => m_petController;

    public override void Initlization(Action callBack = null)
    {
        base.Initlization(callBack);

        m_homePanel.Initlization();
        m_homePanel.Apply(this);
        m_battlePanel.Initlization();
        m_battlePanel.Apply(this);
        m_stagePanel.Initlization();
        m_stagePanel.Apply(this);
        foreach (var cmd in m_commands)
            cmd.Apply(this);

    }
    public override void Active(bool isActive)
    {
        base.Active(isActive);

        if (isActive)
            ActionCommand(CommandType.Home);
    }

    internal void ActionCommand(CommandType commandType)
    {
        switch (commandType)
        {
            case CommandType.Home:
                Debug.Log("Home Command Executed");
                m_curPanel?.Active(false);
                m_homePanel.Active(true);
                m_curPanel = m_homePanel;
                break;
            case CommandType.Battle:
                Debug.Log("Battle Command Executed");
                m_curPanel?.Active(false);
                m_battlePanel.Active(true);
                m_curPanel = m_battlePanel;
                break;
            case CommandType.Stage:
                Debug.Log("Stage Command Executed");
                m_stagePanel.Active(true);
                break;
            case CommandType.Toy:
                Debug.Log("Toy Command Executed");
                break;
            case CommandType.Book:
                Debug.Log("Book Command Executed");
                break;
            case CommandType.Setting:
                Debug.Log("Setting Command Executed");
                break;
            default:
                Debug.Log("Unknown Command Executed");
                break;
        }
    }

    private void Update()
    {
        if (m_curPanel is HomePanel panel)
            panel.Tick(Time.deltaTime);
    }
}
