using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameCore.Database
{
    [Serializable]
    public class ScenemapData : Data
    {
        [SerializeField]
        private int m_scenemapNo;
        [SerializeField]
        private string m_scenemapName = string.Empty;
        [SerializeField]
        private string m_scenemapDescription = string.Empty;
        [SerializeField]
        [LabelText("背景圖片")]
        private Sprite m_imgBgReference;

        [SerializeField]
        [LabelText("場景載入物件")]
        private GameObject m_spawnObjectReference;

        [SerializeField]
        [LabelText("關卡選擇小圖")]
        private Sprite m_levelSelectionThumbnail;

        public int SceneMapNo => m_scenemapNo;
        public string ScenemapName => m_scenemapName;
        public string ScenemapDescription => m_scenemapDescription;
        public Sprite ImgBgReference => m_imgBgReference;
        public GameObject SpawnObjectReference => m_spawnObjectReference;
        public Sprite LevelSelectionThumbnail => m_levelSelectionThumbnail;


        private List<RoleData> m_cachedEnemies;
        public IReadOnlyList<RoleData> enemies
        {
            get
            {
                if (m_cachedEnemies == null || m_cachedEnemies.Count == 0)
                    LoadRoleDatas();
                return m_cachedEnemies.AsReadOnly();
            }
        }

        void LoadRoleDatas()
        {
            m_cachedEnemies = new List<RoleData>();

            var roles = Database<RoleData>.GetAll();
            foreach (RoleData roleData in roles)
            {
                if (roleData.SceneReference.GetKey() == key)
                {
                    m_cachedEnemies.Add(roleData);
                }
            }
        }

        public override string GetTitle([CallerMemberName] string memberName = null)
        {
            return base.GetTitle();
        }
    }
}