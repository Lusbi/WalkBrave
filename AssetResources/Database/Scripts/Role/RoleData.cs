using System;
using GameCore.Log;
using Sirenix.OdinInspector;
using UnityEditor;
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
        [LabelText("敵人小圖示")]
        [SerializeField]
        private Sprite m_enemyIcon;
        [LabelText("敵人面向")]
        [SerializeField]
        private DirectionType m_directionType = DirectionType.Left;
        [LabelText("生成條件")]
        [SerializeField]
        private FlagReference[] m_flagReferenceConditions;
        [LabelText("擊殺後增加旗標")]
        [SerializeField]
        private FlagReference m_killToAddFlagReference;

        [LabelText("死亡後觸發事件類型")]
        [SerializeField]
        private DieActionType m_dieActionType;

        [SerializeField , LabelText("死亡後變更敵人") , ShowIf(nameof(m_dieActionType) , DieActionType.ChangeStyle)]
        private string m_changeStyleKey;
        [SerializeField, LabelText("死亡後觸發劇情事件"), ShowIf(nameof(m_dieActionType), DieActionType.Drama)]
        private string m_dramaKey;

        [SerializeField]
        private string m_roleIconkey;

        public string RoleName => m_roleName;
        public string RoleDescription => m_roleDescription;
        public string EnemyType => m_enemyType;
        public ScenemapReference SceneReference => m_sceneReference;
        public DropInfo DropInfo => m_dropInfo;
        public int HitCount => m_hitCount;
        public DirectionType DirectionType => m_directionType;

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
                if (flag.TryLoad(out var flagData))
                {
                    result &= StorageManager.instance.StorageData.GetFlagStorageValue(flagData.key) > 0;
                }
                else
                {
                    eLog.Error($"無旗標資料：{flag.GetKey()}");
                    result = false;
                }
            }
            return result;
        }

#if UNITY_EDITOR
        public void SetSprite()
        {
            string[] guids = AssetDatabase.FindAssets($"t:Sprite {m_roleIconkey}");
            if (guids.Length > 0)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                m_enemyIcon = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            }
            else
            {
                Debug.LogWarning($"No sprite found for key: {m_roleIconkey}");
                m_enemyIcon = null;
            }
        }

        public override string GetLog()
        {
            return $"{base.GetLog()}, roleName = {m_roleName}, roleDescription = {m_roleDescription}, enemyType = {m_enemyType}, sceneReference = {m_sceneReference}, dropInfo = {m_dropInfo}, hitCount = {m_hitCount}";
        }
#endif
    }
}