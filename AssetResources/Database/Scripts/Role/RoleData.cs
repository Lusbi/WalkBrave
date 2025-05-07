using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace GameCore.Database
{
    [Serializable]
    public class RoleData : Data
    {
        [HideInInspector]
        [SerializeField]
        private string m_roleName = string.Empty;
        [HideInInspector]
        [SerializeField]
        private string m_roleDescription = string.Empty;
        [LabelText("敵人類型")]
        [SerializeField]
        private string m_enemyType = string.Empty;
        [LabelText("場景參考")]
        [SerializeField]
        private ScenemapReference m_sceneReference;
        [LabelText("掉落資訊")]
        [SerializeField]
        private DropInfo m_dropInfo;
        [LabelText("命中次數")]
        [SerializeField]
        private int m_hitCount = 0;
        [LabelText("敵人圖示")]
        [SerializeField]
        private Sprite m_enemyIcon;
        [LabelText("敵人圖示")]
        [SerializeField]
        private Sprite m_enemyBttleSprite;
        [LabelText("生成條件")]
        [SerializeField]
        private FlagReference[] m_flagReferenceConditions;
        [LabelText("擊殺後增加旗標")]
        [SerializeField]
        private FlagReference m_killToAddFlagReference;


        public string RoleName => m_roleName;
        public string RoleDescription => m_roleDescription;
        public string EnemyType => m_enemyType;
        public ScenemapReference SceneReference => m_sceneReference;
        public DropInfo DropInfo => m_dropInfo;
        public int HitCount => m_hitCount;

        public FlagReference[] FlagReferenceConditions => m_flagReferenceConditions;
        public FlagReference KillToAddFlagReference => m_killToAddFlagReference;
        public Sprite EnemyIcon => m_enemyIcon;

        public bool ValidateFlagReferenceConditions()
        {
            if (m_flagReferenceConditions == null || m_flagReferenceConditions.Length == 0)
            {
                return true;
            }
            bool result = true;
            foreach (var flag in m_flagReferenceConditions)
            {
                if (!flag.TryLoad(out var flagData))
                {
                    result &= StorageManager.instance.StorageData.GetFlagStorageValue(flagData.key) > 0;
                    return false;
                }
            }
            return result;
        }

#if UNITY_EDITOR
        public override string GetLog()
        {
            return $"{base.GetLog()}, roleName = {m_roleName}, roleDescription = {m_roleDescription}, enemyType = {m_enemyType}, sceneReference = {m_sceneReference}, dropInfo = {m_dropInfo}, hitCount = {m_hitCount}";
        }
#endif
    }
}