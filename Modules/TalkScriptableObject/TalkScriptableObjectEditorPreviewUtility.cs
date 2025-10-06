#if UNITY_EDITOR
using UnityEditor.Localization;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

/// <summary>
/// 提供 Talk Scriptable Object 在編輯器中預覽繁體中文文本的工具。
/// </summary>
internal static class TalkScriptableObjectEditorPreviewUtility
{
    private const string TableName = "LocalizationCollection";
    private const string DefaultLocaleIdentifier = "zh-TW";

    private static StringTable s_cachedTable;
    private static LocaleIdentifier s_cachedLocaleIdentifier;

    /// <summary>
    /// 根據傳入的多國語系 Key 讀取繁體中文顯示內容。
    /// </summary>
    internal static string GetTraditionalChineseContent(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            return string.Empty;
        }

        EnsureTable();

        if (s_cachedTable == null)
        {
            return key;
        }

        var entry = s_cachedTable.GetEntry(key);
        return entry != null ? entry.LocalizedValue : key;
    }

    /// <summary>
    /// 快取目標字串表，避免每次都重新搜尋資源。
    /// </summary>
    private static void EnsureTable()
    {
        if (s_cachedTable != null && s_cachedLocaleIdentifier.Code == DefaultLocaleIdentifier)
        {
            return;
        }

        var collection = LocalizationEditorSettings.GetStringTableCollection(TableName);
        if (collection == null)
        {
            s_cachedTable = null;
            return;
        }

        var locale = LocalizationEditorSettings.GetLocale(new LocaleIdentifier(DefaultLocaleIdentifier));
        if (locale == null)
        {
            s_cachedTable = null;
            return;
        }

        s_cachedLocaleIdentifier = locale.Identifier;
        s_cachedTable = collection.GetTable(locale.Identifier);
    }
}
#endif
