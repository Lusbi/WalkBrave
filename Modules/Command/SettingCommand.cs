using UnityEngine;

public class SettingCommand : CommandBase, ICommand
{
    public override CommandType CommandType => CommandType.Setting;

    public override void Execute()
    {
        Debug.Log("Executing Setting Command");
        // 在這裡添加 Setting 指令的具體邏輯
    }
}
