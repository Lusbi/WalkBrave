#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Globalization;
using GameCore.Database;
using GameCore.Database.Editor;
using UnityEditor;
using UnityEngine;

internal static class TalkScriptableObjectSheetImporter
{
    private static readonly string[] s_headerKeywords =
    {
        "檔名",
        "TalkScriptableObjectName",
        "對話內容",
        "執行秒數"
    };

    internal static void Import(string rawContent)
    {
        if (string.IsNullOrWhiteSpace(rawContent))
        {
            Debug.LogWarning("Talk sheet content is empty.");
            return;
        }

        TalkScriptableObjectEditorUtility.EnsureFolder();

        Dictionary<int, RoleData> roleLookup = null;
        TalkScriptableObject currentAsset = null;
        string currentAssetName = string.Empty;
        var entries = new List<TalkScriptableObjectName>();

        string[] lines = rawContent.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);
        foreach (string rawLine in lines)
        {
            if (string.IsNullOrWhiteSpace(rawLine))
            {
                continue;
            }

            string[] columns = SplitColumns(rawLine);
            if (columns.Length == 0)
            {
                continue;
            }

            string firstColumn = columns[0].Trim();
            if (string.IsNullOrEmpty(firstColumn) || IsHeader(firstColumn))
            {
                continue;
            }

            bool startsNewAsset = ShouldStartNewAsset(columns);
            if (startsNewAsset)
            {
                SaveCurrentAsset();

                currentAssetName = firstColumn;
                currentAsset = TalkScriptableObjectEditorUtility.GetOrCreateTalkAssetByName(currentAssetName);
                roleLookup ??= BuildRoleLookup();
                TalkScriptableObjectEditorUtility.TryAssignToRole(currentAsset, currentAssetName, roleLookup);
                entries.Clear();
                continue;
            }

            if (currentAsset == null)
            {
                Debug.LogWarning($"Encountered talk content without an assigned TalkScriptableObject name: {rawLine}");
                continue;
            }

            string content = columns[0];
            float duration = ParseDuration(columns, 1);
            entries.Add(new TalkScriptableObjectName(content, duration));
        }

        SaveCurrentAsset();

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Talk Scriptable Objects imported successfully.");

        void SaveCurrentAsset()
        {
            if (currentAsset == null)
            {
                return;
            }

            TalkScriptableObjectEditorUtility.ApplyEntries(currentAsset, entries);
        }
    }

    private static Dictionary<int, RoleData> BuildRoleLookup()
    {
        var roles = DatabaseEditorUtils.GetDatas<RoleData>();
        var lookup = new Dictionary<int, RoleData>();
        foreach (RoleData role in roles)
        {
            int id = TalkScriptableObjectEditorUtility.GetRoleId(role);
            if (id >= 0 && lookup.ContainsKey(id) == false)
            {
                lookup.Add(id, role);
            }
        }

        return lookup;
    }

    private static string[] SplitColumns(string rawLine)
    {
        string[] columns = rawLine.Split('\t');
        if (columns.Length <= 1)
        {
            columns = rawLine.Split(',');
        }

        return columns;
    }

    private static bool ShouldStartNewAsset(string[] columns)
    {
        if (columns.Length <= 1)
        {
            return true;
        }

        string secondColumn = columns[1].Trim();
        if (string.IsNullOrEmpty(secondColumn) || IsHeader(secondColumn))
        {
            return true;
        }

        return float.TryParse(secondColumn, NumberStyles.Float, CultureInfo.InvariantCulture, out _) == false;
    }

    private static bool IsHeader(string value)
    {
        foreach (string keyword in s_headerKeywords)
        {
            if (string.Equals(value, keyword, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    private static float ParseDuration(string[] columns, int index)
    {
        if (columns.Length <= index)
        {
            return 0f;
        }

        string value = columns[index].Trim();
        if (float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out float result))
        {
            return result;
        }

        if (float.TryParse(value, NumberStyles.Float, CultureInfo.CurrentCulture, out result))
        {
            return result;
        }

        return 0f;
    }
}
#endif
