using UnityEngine;

public class CommandExample : MonoBehaviour
{
    private void Start()
    {
        // 註冊命令
    }

    private void OnDestroy()
    {
        // 取消註冊命令
    }

    private void SpawnCommand(string args)
    {
        if (args.Length == 0)
        {
            Debug.Log("Usage: spawn <enemyType>");
            return;
        }

        string enemyType = args;
        Debug.Log($"Spawning enemy of type: {enemyType}");
        // 在這裡添加生成敵人的邏輯
    }

    private void KillCommand(string args)
    {
        if (args.Length == 0)
        {
            Debug.Log("Usage: kill <enemyId>");
            return;
        }

        string enemyId = args;
        Debug.Log($"Killing enemy with ID: {enemyId}");
        // 在這裡添加殺死敵人的邏輯
    }

    private void HelpCommand(string[] args)
    {
        Debug.Log("Available commands: spawn, kill, help");
    }
}
