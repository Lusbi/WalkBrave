using UnityEngine;

[System.Flags]
public enum BattleCommandFlag
{
    /// <summary>
    /// 單體
    /// </summary>
    Single,
    /// <summary>
    /// 複數
    /// </summary>
    Multi,

    /// <summary>
    /// 同一隊
    /// </summary>
    OurSide,
    /// <summary>
    /// 對方
    /// </summary>
    EnemySide,
    /// <summary>
    /// 雙方
    /// </summary>
    AllSide,
}
