using UnityEngine;

public class BookCommand : CommandBase, ICommand
{
    public override CommandType CommandType => CommandType.Book;

    public override void Execute()
    {
        Debug.Log("Executing Book Command");
        // 在這裡添加 Book 指令的具體邏輯
    }
}
