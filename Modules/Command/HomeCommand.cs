using UnityEngine;

public class HomeCommand : CommandBase, ICommand
{
    public override CommandType CommandType => CommandType.Home;

    public override void Execute()
    {
        Debug.Log("Executing Home Command");
        // �b�o�̲K�[ Home ���O�������޿�
    }
}
