using System;
using System.Collections.Generic;
using GameCore;
using UnityEngine;

public class CommandManager : Singleton<CommandManager>
{
    // 用於緩存輸入的字元
    private string currentInput = string.Empty;

    // 預定義的合法指令
    private readonly Dictionary<string, Action> commands = new Dictionary<string, Action>();


    private void Start()
    {
        // 初始化合法指令
        InitializeCommands();
    }

    private void InitializeCommands()
    {
        // 預設初始化指令可在此添加
    }

    /// <summary>
    /// 註冊新的指令
    /// </summary>
    /// <param name="command">指令名稱</param>
    /// <param name="action">指令對應的行為</param>
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
    /// 取消註冊指令
    /// </summary>
    /// <param name="command">指令名稱</param>
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
        // 將按鍵轉換為字元並添加到當前輸入
        if (inputChar != '\0') // 確保是有效字元
        {
            currentInput += inputChar;
            Debug.Log($"Current Input: {currentInput}");

            // 檢查是否有合法指令
            CheckCommand();
        }
    }

    private void CheckCommand()
    {
        // 檢查當前輸入是否匹配任何合法指令
        foreach (var command in commands)
        {
            if (command.Key.StartsWith(currentInput))
            {
                // 如果完全匹配，執行指令
                if (command.Key == currentInput)
                {
                    Debug.Log($"Command '{command.Key}' executed.");
                    command.Value.Invoke();
                    currentInput = string.Empty; // 清空輸入
                }
                return; // 如果有匹配的指令前綴，繼續等待輸入
            }
        }

        // 如果沒有匹配的指令，清空輸入
        Debug.LogWarning($"No matching command for input: {currentInput}");
        currentInput = string.Empty;
    }
}
