using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace GameCore.Database
{
    [Serializable]
    public class RoleData : Data, IDataDetail
    {
        [SerializeField , LabelText("基本資料")] private DatabaseNormalDetail m_detailBaseNormalDetal;
        [SerializeField , LabelText("職業")] private JobReference m_job;
        [SerializeField , LabelText("喜愛的食物")] private ItemReference[] m_favoriteItems;
        [SerializeField , LabelText("厭惡的食物")] private ItemReference[] m_hateItems;
        [SerializeField , LabelText("角色的類型")] private RoleType m_roleType;

        public string DetailName => m_detailBaseNormalDetal.DetailName;
        public string DetailDescription => m_detailBaseNormalDetal.DetailDescription;
        public Sprite GetSprite() => m_detailBaseNormalDetal.GetSprite();
    }
}