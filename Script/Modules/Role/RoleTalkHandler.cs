using System;
using System.Collections.Generic;
using GameCore.Database;
using UnityEngine;

public class RoleTalkHandler
{
    private RoleData m_roleData;
    private float m_durationTime = 0;

    /// <summary>
    /// 更新當前戰鬥角色
    /// </summary>
    /// <param name="roleData"></param>
    public void Apply(RoleData roleData)
    {
        m_roleData = roleData;
        m_durationTime = 0;
    }
    /// <summary>
    /// 逐幀更新
    /// </summary>
    /// <param name="deltaTime"></param>
    public void Tick(float deltaTime)
    {
        if (m_roleData == null)
            return;
        m_durationTime -= deltaTime;
        if (m_durationTime < 0)
            ConditionTalk();
    }
    /// <summary>
    /// 條件判斷
    /// </summary>
    private void ConditionTalk()
    {
        List<TalkScriptableObjectName> validTalks = new List<TalkScriptableObjectName>();
        // 取得合法可說的內容
        foreach (var talkScriptableObjectName in m_roleData.TalkScriptableObject.Entries)
        {
            bool conditionValid = ConditionValid(talkScriptableObjectName.Conditions);
            if (conditionValid)
            {
                validTalks.Add(talkScriptableObjectName);
            }
        }

        if (validTalks.Count > 0)
        {
            TalkScriptableObjectName talk = validTalks[UnityEngine.Random.Range(0, validTalks.Count)];

            // 發送對話內容
            m_durationTime = talk.Duration;
        }
        
    }

    private bool ConditionValid(IReadOnlyList<TalkCondition> talkConditions)
    {
        bool result = true;
        foreach (var condition in talkConditions)
        {
            if (condition.FlagReference.Exists())
            {
                result &= StorageManager.instance.StorageData.GetFlagStorageValue(condition.FlagReference.GetKey()) > 0;
            }
            result &= StorageManager.instance.StorageData.GetEnemyStorageData(m_roleData.key).KillValue > condition.RemainingCount;
        }
        return result;
    }
}
