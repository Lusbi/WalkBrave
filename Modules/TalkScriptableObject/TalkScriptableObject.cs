using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TalkScriptableObject", menuName = "Scriptable Objects/Talk Scriptable Object")]
public class TalkScriptableObject : ScriptableObject
{
    [SerializeField]
    private TalkScriptableObjectName[] m_entries = Array.Empty<TalkScriptableObjectName>();

    public IReadOnlyList<TalkScriptableObjectName> Entries => m_entries;

#if UNITY_EDITOR
    public void Editor_SetEntries(IReadOnlyList<TalkScriptableObjectName> entries)
    {
        if (entries == null || entries.Count == 0)
        {
            m_entries = Array.Empty<TalkScriptableObjectName>();
            return;
        }

        var array = new TalkScriptableObjectName[entries.Count];
        for (int i = 0; i < entries.Count; i++)
        {
            array[i] = entries[i];
        }

        m_entries = array;
    }
#endif
}

[Serializable]
public struct TalkScriptableObjectName
{
    [SerializeField, TextArea]
    private string m_content;

    [SerializeField, Min(0f)]
    private float m_duration;

    public string Content => m_content;

    public float Duration => m_duration;

    public TalkScriptableObjectName(string content, float duration)
    {
        m_content = content;
        m_duration = duration;
    }
}
