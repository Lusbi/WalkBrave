using UnityEngine;

public class SettingCommand : CommandBase, ICommand
{
    public override CommandType CommandType => CommandType.Setting;

    public override void Execute()
    {
        Debug.Log("Executing Setting Command");
        // �b�o�̲K�[ Setting ���O�������޿�
    }
}
