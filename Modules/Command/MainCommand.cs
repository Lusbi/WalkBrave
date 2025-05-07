using UnityEngine;

public class MainCommand : CommandBase, ICommand
{
    public override CommandType CommandType => CommandType.Main;

    public override void Execute()
    {
        Debug.Log("Executing Main Command");
        // 在這裡添加 Main 指令的具體邏輯
        // 例如，切換到主界面或執行其他相關操作
        
    }
}
