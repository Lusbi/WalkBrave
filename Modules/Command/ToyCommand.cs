using UnityEngine;

public class ToyCommand : CommandBase, ICommand
{
    public override CommandType CommandType => CommandType.Toy;

    public override void Execute()
    {
        Debug.Log("Executing Toy Command");
        // 在這裡添加 Toy 指令的具體邏輯
    }
}
