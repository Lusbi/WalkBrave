using System;
using System.Collections.Generic;
using GameCore;
using UnityEngine;

public class CommandManager : Singleton<CommandManager>
{
    // �Ω�w�s��J���r��
    private string currentInput = string.Empty;

    // �w�w�q���X�k���O
    private readonly Dictionary<string, Action> commands = new Dictionary<string, Action>();


    private void Start()
    {
        // ��l�ƦX�k���O
        InitializeCommands();
    }

    private void InitializeCommands()
    {
        // �w�]��l�ƫ��O�i�b���K�[
    }

    /// <summary>
    /// ���U�s�����O
    /// </summary>
    /// <param name="command">���O�W��</param>
    /// <param name="action">���O�������欰</param>
    public void RegisterCommand(string command, Action action)
    {
        if (!commands.ContainsKey(command))
        {
            commands.Add(command, action);
            Debug.Log($"Command '{command}' registered successfully.");
        }
        else
        {
            Debug.LogWarning($"Command '{command}' is already registered.");
        }
    }

    /// <summary>
    /// �������U���O
    /// </summary>
    /// <param name="command">���O�W��</param>
    public void UnregisterCommand(string command)
    {
        if (commands.ContainsKey(command))
        {
            commands.Remove(command);
            Debug.Log($"Command '{command}' unregistered successfully.");
        }
        else
        {
            Debug.LogWarning($"Command '{command}' is not registered.");
        }
    }

    public void InputCommand(char inputChar)
    {
        // �N�����ഫ���r���òK�[���e��J
        if (inputChar != '\0') // �T�O�O���Ħr��
        {
            currentInput += inputChar;
            Debug.Log($"Current Input: {currentInput}");

            // �ˬd�O�_���X�k���O
            CheckCommand();
        }
    }

    private void CheckCommand()
    {
        // �ˬd��e��J�O�_�ǰt����X�k���O
        foreach (var command in commands)
        {
            if (command.Key.StartsWith(currentInput))
            {
                // �p�G�����ǰt�A������O
                if (command.Key == currentInput)
                {
                    Debug.Log($"Command '{command.Key}' executed.");
                    command.Value.Invoke();
                    currentInput = string.Empty; // �M�ſ�J
                }
                return; // �p�G���ǰt�����O�e��A�~�򵥫ݿ�J
            }
        }

        // �p�G�S���ǰt�����O�A�M�ſ�J
        Debug.LogWarning($"No matching command for input: {currentInput}");
        currentInput = string.Empty;
    }
}
