using System;
using JetBrains.Annotations;
using UnityEngine;

public class BattleManager : GameCore.Singleton<BattleManager>, IInputEventListener
{

    private Action<string> m_onHitEvent;

    public BattleManager()
    {
        InputEventManager.instance.RegisterListener(this);
    }
    ~BattleManager()
    {
        InputEventManager.instance.UnregisterListener(this);
    }

    public void Register(Action<string> onHit)
    {
        m_onHitEvent = onHit;
    }

    public void OnKeyDown(KeyCode keyCode) => Hint(keyCode.ToString());

    public void OnKeyUp(KeyCode keyCode) => Hint(keyCode.ToString());

    public void OnMouseClick(int button, Vector3 position) => Hint();

    private void Hint(string key = null)
    {
        m_onHitEvent?.Invoke(key);
    }
}
