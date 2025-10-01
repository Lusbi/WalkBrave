using GameCore.Event;

/// <summary>
/// �]�w�԰�����
/// </summary>
public struct SetBattleRoleEvent : IEventBase
{
    public string roleKey;
    public SetBattleRoleEvent(string roleKey) => this.roleKey = roleKey;
}

/// <summary>
/// ���X���}�l
/// </summary>
public struct TomatoTriggerEvent : IEventBase 
{
    public bool enable;
    public TomatoTriggerEvent(bool enable) => this.enable = enable;
}