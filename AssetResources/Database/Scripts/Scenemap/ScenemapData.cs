using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.Database
{
    [Serializable]
    public class ScenemapData : Data
    {
        [HideInInspector]
        [SerializeField]
        private string m_scenemapName = string.Empty;
        [HideInInspector]
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
                if (m_cachedEnemies == null)
                    LoadRoleDatas();
                return m_cachedEnemies.AsReadOnly();
            }
        }

        void LoadRoleDatas()
        {
            m_cachedEnemies = new List<RoleData>();

            var roles = Database<RoleData>.LoadAll();
            foreach (RoleData roleData in roles)
            {
                if (roleData.SceneReference.GetKey() == key)
                {
                    m_cachedEnemies.Add(roleData);
                }
            }
        }
    }
}