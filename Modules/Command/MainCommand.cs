using UnityEngine;

public class MainCommand : CommandBase, ICommand
{
    public override CommandType CommandType => CommandType.Main;

    public override void Execute()
    {
        Debug.Log("Executing Main Command");
        // �b�o�̲K�[ Main ���O�������޿�
        // �Ҧp�A������D�ɭ��ΰ����L�����ާ@
        
    }
}
