using UnityEngine;

public class BattleCommand : CommandBase, ICommand
{
    public override CommandType CommandType => CommandType.Battle;

    public override void Execute()
    {
        Debug.Log("Executing Battle Command");
        // �b�o�̲K�[ Battle ���O�������޿�
    }
}
