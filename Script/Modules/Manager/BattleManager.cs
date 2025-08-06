using System;
using GameCore.Log;
using UnityEngine;

public class BattleManager : GameCore.Singleton<BattleManager>
{

    private Action<string> m_onHitEvent;

    public void Register(Action<string> onHitEvent)
    {
        if (onHitEvent == m_onHitEvent)
        {
            return;
        }

        if (onHitEvent != null)
        {
            WindowsInputHookManager.OnGlobalKeyDown += OnGlobalKeyDown;
            WindowsInputHookManager.OnGlobalMouseDown += OnGlobalMouseDown;
        }
        else
        {
            WindowsInputHookManager.OnGlobalKeyDown -= OnGlobalKeyDown;
            WindowsInputHookManager.OnGlobalMouseDown -= OnGlobalMouseDown;
        }
        m_onHitEvent = onHitEvent;
    }
    private void OnGlobalMouseDown(int obj)
    {
        Hit();
    }

    private void OnGlobalKeyDown(KeyCode code)
    {
        Hit(code.ToString());
    }

    private void Hit(string key = null)
    {
        m_onHitEvent?.Invoke(key);
    }
}
