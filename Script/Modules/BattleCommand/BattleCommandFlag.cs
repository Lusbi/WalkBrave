using UnityEngine;

[System.Flags]
public enum BattleCommandFlag
{
    /// <summary>
    /// ����
    /// </summary>
    Single,
    /// <summary>
    /// �Ƽ�
    /// </summary>
    Multi,

    /// <summary>
    /// �P�@��
    /// </summary>
    OurSide,
    /// <summary>
    /// ���
    /// </summary>
    EnemySide,
    /// <summary>
    /// ����
    /// </summary>
    AllSide,
}
