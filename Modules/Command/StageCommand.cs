using UnityEngine;

public class StageCommand : CommandBase, ICommand
{
    public override CommandType CommandType => CommandType.Stage;

    public override void Execute()
    {
        Debug.Log("Executing Stage Command");
        // �b�o�̲K�[ Stage ���O�������޿�
    }
}
