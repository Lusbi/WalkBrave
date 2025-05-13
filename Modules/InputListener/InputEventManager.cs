using System;
using System.Collections.Generic;
using GameCore;
using UnityEngine;

public class InputEventManager : MonoSingleton<InputEventManager>
{

    // 註冊的成員
    private readonly List<IInputEventListener> listeners = new List<IInputEventListener>();

    private void Update()
    {
        // 監聽鍵盤輸入
        foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(keyCode))
            {
                NotifyKeyDown(keyCode);
            }

            //if (Input.GetKeyUp(keyCode))
            //{
            //    NotifyKeyUp(keyCode);
            //}
        }

        // 監聽滑鼠點擊
        //if (Input.GetMouseButtonDown(0)) // 左鍵
        //{
        //    NotifyMouseClick(0, Input.mousePosition);
        //}
        //if (Input.GetMouseButtonDown(1)) // 右鍵
        //{
        //    NotifyMouseClick(1, Input.mousePosition);
        //}
        //if (Input.GetMouseButtonDown(2)) // 中鍵
        //{
        //    NotifyMouseClick(2, Input.mousePosition);
        //}
    }

    // 註冊成員
    public void RegisterListener(IInputEventListener listener)
    {
        if (!listeners.Contains(listener))
        {
            listeners.Add(listener);
        }
    }

    // 取消註冊成員
    public void UnregisterListener(IInputEventListener listener)
    {
        if (listeners.Contains(listener))
        {
            listeners.Remove(listener);
        }
    }

    // 通知鍵盤按下事件
    private void NotifyKeyDown(KeyCode keyCode)
    {
        foreach (var listener in listeners)
        {
            listener.OnKeyDown(keyCode);
        }
    }

    // 通知鍵盤釋放事件
    private void NotifyKeyUp(KeyCode keyCode)
    {
        foreach (var listener in listeners)
        {
            listener.OnKeyUp(keyCode);
        }
    }

    // 通知滑鼠點擊事件
    private void NotifyMouseClick(int button, Vector3 position)
    {
        foreach (var listener in listeners)
        {
            listener.OnMouseClick(button, position);
        }
    }
}
