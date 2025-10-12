using System;
using System.Collections.Generic;
using GameCore.Database;
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

[Serializable, HideInInspector]
public struct TalkScriptableObjectName
{
    [SerializeField, TextArea]
    private string m_content;

    [SerializeField, Min(0f)]
    private float m_duration;

    [SerializeField]
    private TalkCondition[] m_conditions;

    public string Content => m_content;

    public float Duration => m_duration;

    public IReadOnlyList<TalkCondition> Conditions => m_conditions ?? Array.Empty<TalkCondition>();

    public TalkScriptableObjectName(string content, float duration, IReadOnlyList<TalkCondition> conditions = null)
    {
        m_content = content;
        m_duration = duration;
        if (conditions == null || conditions.Count == 0)
        {
            m_conditions = Array.Empty<TalkCondition>();
            return;
        }

        m_conditions = new TalkCondition[conditions.Count];
        for (int i = 0; i < conditions.Count; i++)
        {
            m_conditions[i] = conditions[i];
        }
    }
}

[Serializable]
public struct TalkCondition
{
    [SerializeField, Min(0)]
    private int m_remainingCount;

    [SerializeField, Min(0)]
    private int m_killCount;

    [SerializeField]
    private FlagReference m_flagReference;

    public int RemainingCount => m_remainingCount;

    public int KillCount => m_killCount;

    public FlagReference FlagReference => m_flagReference;

    public TalkCondition(int remainingCount, int KillCount, FlagReference flagReference)
    {
        m_remainingCount = remainingCount;
        m_killCount = KillCount;
        m_flagReference = flagReference;
    }
}
