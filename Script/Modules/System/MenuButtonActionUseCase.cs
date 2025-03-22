using UnityEngine;

public class MenuButtonActionUseCase
{
    public void Execute(MenuActionType menuActionType)
    {
        switch (menuActionType)
        {
            case MenuActionType.Main: 
                Action_Main();
                break;
            case MenuActionType.Status: 
                Action_Status();
                break;
            case MenuActionType.Equipment: 
                Action_Equipment();
                break;
            case MenuActionType.Skill:
                Action_Skill();
                break;
            case MenuActionType.Shop: 
                Action_Shop();
                break;
            case MenuActionType.Mission: 
                Action_Mission();
                break;
            case MenuActionType.Story: 
                Action_Story();
                break;
            case MenuActionType.Options: 
                Action_Options();
                break;
        }
    }

    /// <summary>
    /// ��^�D����
    /// </summary>
    private void Action_Main()
    {

    }

    /// <summary>
    /// �������A��
    /// </summary>
    private void Action_Status()
    {

    }

    /// <summary>
    /// �����˳ƭ�
    /// </summary>
    private void Action_Equipment()
    {

    }

    /// <summary>
    /// �����ޯ୶
    /// </summary>
    private void Action_Skill() 
    {
    }

    /// <summary>
    /// �����ө���
    /// </summary>
    private void Action_Shop() 
    {
    }

    /// <summary>
    /// �������ȭ�
    /// </summary>
    private void Action_Mission()
    {

    }

    /// <summary>
    /// �����@����
    /// </summary>
    private void Action_Story() 
    { 
    }

    /// <summary>
    /// �����ﶵ��
    /// </summary>
    private void Action_Options()
    {

    }
}
