using System;
using UnityEngine;

namespace GameCore.Database
{
    [Serializable]
    public class EquipData : Data , IDataDetail , IDataShopDetail
    {
        [SerializeField] private DatabaseNormalDetail m_databaseNormalDetail;
        [SerializeField] private EquipmentType m_equipmentType;
        [SerializeField] private RareTypes m_rareType;
        [SerializeField] private int m_slotCount;
        [SerializeField] private DatabaseShopDetail m_databaseShopDetail;

        public EquipmentType equipmentType => m_equipmentType;

        public string DetailName => m_databaseNormalDetail.DetailName;

        public string DetailDescription => m_databaseNormalDetail.DetailDescription;

        public int purchasePrice => m_databaseShopDetail.purchasePrice;

        public int sellPrice => m_databaseShopDetail.sellPrice;

        public Sprite GetSprite() => m_databaseNormalDetail.GetSprite();

        public void SetSprite(out Sprite sprite) => sprite = m_databaseNormalDetail.GetSprite();
    }
}