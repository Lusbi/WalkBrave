using UnityEngine;

public class CommandExample : MonoBehaviour
{
    private void Start()
    {
        // ���U�R�O
    }

    private void OnDestroy()
    {
        // �������U�R�O
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
        // �b�o�̲K�[�ͦ��ĤH���޿�
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
        // �b�o�̲K�[�����ĤH���޿�
    }

    private void HelpCommand(string[] args)
    {
        Debug.Log("Available commands: spawn, kill, help");
    }
}
