using UnityEngine;

public class BookCommand : CommandBase, ICommand
{
    public override CommandType CommandType => CommandType.Book;

    public override void Execute()
    {
        Debug.Log("Executing Book Command");
        // �b�o�̲K�[ Book ���O�������޿�
    }
}
