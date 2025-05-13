using System;
using System.Collections.Generic;
using GameCore;
using UnityEngine;

public class InputEventManager : MonoSingleton<InputEventManager>
{

    // ���U������
    private readonly List<IInputEventListener> listeners = new List<IInputEventListener>();

    private void Update()
    {
        // ��ť��L��J
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

        // ��ť�ƹ��I��
        //if (Input.GetMouseButtonDown(0)) // ����
        //{
        //    NotifyMouseClick(0, Input.mousePosition);
        //}
        //if (Input.GetMouseButtonDown(1)) // �k��
        //{
        //    NotifyMouseClick(1, Input.mousePosition);
        //}
        //if (Input.GetMouseButtonDown(2)) // ����
        //{
        //    NotifyMouseClick(2, Input.mousePosition);
        //}
    }

    // ���U����
    public void RegisterListener(IInputEventListener listener)
    {
        if (!listeners.Contains(listener))
        {
            listeners.Add(listener);
        }
    }

    // �������U����
    public void UnregisterListener(IInputEventListener listener)
    {
        if (listeners.Contains(listener))
        {
            listeners.Remove(listener);
        }
    }

    // �q����L���U�ƥ�
    private void NotifyKeyDown(KeyCode keyCode)
    {
        foreach (var listener in listeners)
        {
            listener.OnKeyDown(keyCode);
        }
    }

    // �q����L����ƥ�
    private void NotifyKeyUp(KeyCode keyCode)
    {
        foreach (var listener in listeners)
        {
            listener.OnKeyUp(keyCode);
        }
    }

    // �q���ƹ��I���ƥ�
    private void NotifyMouseClick(int button, Vector3 position)
    {
        foreach (var listener in listeners)
        {
            listener.OnMouseClick(button, position);
        }
    }
}
