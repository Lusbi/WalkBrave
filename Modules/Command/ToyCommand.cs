using UnityEngine;

public class ToyCommand : CommandBase, ICommand
{
    public override CommandType CommandType => CommandType.Toy;

    public override void Execute()
    {
        Debug.Log("Executing Toy Command");
        // �b�o�̲K�[ Toy ���O�������޿�
    }
}
