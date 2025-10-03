using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameCore.Database
{
    // 定義對話觸發條件的分類
    public enum TalkConditionType
    {
        None = 0, // 無條件，任何情況都能觸發
        Flag = 1, // 需要檢查旗標是否成立
        RemainingHealthTimes = 2 // 依據角色剩餘血量次數判斷是否觸發
    }

    // 定義單一對話觸發條件的資料結構
    [Serializable]
    public class TalkCondition
    {
        [SerializeField]
        [LabelText("條件類型")]
        private TalkConditionType m_conditionType = TalkConditionType.None; // 條件分類，對應旗標或血量次數

        [SerializeField]
        [LabelText("旗標條件")]
        [ShowIf(nameof(m_conditionType), TalkConditionType.Flag)]
        private FlagReference m_flagReference; // 條件類型為旗標時所需的旗標參考

        [SerializeField]
        [LabelText("剩餘血量次數")]
        [ShowIf(nameof(m_conditionType), TalkConditionType.RemainingHealthTimes)]
        private int m_remainingHealthTimes; // 條件類型為血量次數時所需的閾值設定

        public TalkConditionType ConditionType => m_conditionType; // 讓外部讀取條件類型

        public FlagReference FlagReference => m_flagReference; // 讓外部讀取旗標條件

        public int RemainingHealthTimes => m_remainingHealthTimes; // 讓外部讀取血量次數條件
    }

    // 定義符合條件後要播放的對話條目
    [Serializable]
    public class TalkEntry
    {
        [SerializeField]
        [LabelText("觸發條件清單")]
        private List<TalkCondition> m_conditions = new List<TalkCondition>(); // 單筆對話條目所有的觸發條件

        [SerializeField]
        [LabelText("對話字串列表")]
        private List<string> m_dialogLocalizationKeys = new List<string>(); // 觸發時要播放的在地化字串鍵值

        public IReadOnlyList<TalkCondition> Conditions => m_conditions; // 提供唯讀的條件列表給外部使用

        public IReadOnlyList<string> DialogLocalizationKeys => m_dialogLocalizationKeys; // 提供唯讀的對話字串鍵值列表
    }

    // 提供編輯器建立對話資料用的 ScriptableObject
    [CreateAssetMenu(menuName = "GameCore/Role/Talk Scriptable Object", fileName = "TalkScriptableObject")]
    public class TalkScriptableObject : ScriptableObject
    {
        [SerializeField]
        [LabelText("對話條目列表")]
        private List<TalkEntry> m_talkEntries = new List<TalkEntry>(); // 儲存所有可用的對話條目

        public IReadOnlyList<TalkEntry> TalkEntries => m_talkEntries; // 對話條目的唯讀介面，方便外部查詢

        public TalkEntry GetRandomEntry()
        {
            // 若無資料則回傳 null，避免隨機取值時出現例外
            if (m_talkEntries == null || m_talkEntries.Count == 0)
            {
                return null;
            }

            // 從所有條目中隨機挑選一筆回傳
            int index = UnityEngine.Random.Range(0, m_talkEntries.Count);
            return m_talkEntries[index];
        }

        public IReadOnlyList<string> GetRandomDialogLocalizationKeys()
        {
            // 透過隨機條目直接取得對話字串列表，若沒有資料則回傳空陣列避免 Null 參考
            TalkEntry entry = GetRandomEntry();
            return entry != null ? entry.DialogLocalizationKeys : Array.Empty<string>();
        }
    }
}
