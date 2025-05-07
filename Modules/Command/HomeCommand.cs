using UnityEngine;

public class HomeCommand : CommandBase, ICommand
{
    public override CommandType CommandType => CommandType.Home;

    public override void Execute()
    {
        Debug.Log("Executing Home Command");
        // 在這裡添加 Home 指令的具體邏輯
    }
}
