using System;
using UnityEngine;

namespace GameCore.Database
{
    [Serializable]
    public class ItemData : Data, IDataDetail, IDataShopDetail
    {
        [SerializeField] private DatabaseNormalDetail m_databaseNormalDetail;
        [SerializeField] private ItemTypes m_itemTypes;
        [SerializeField] private DatabaseShopDetail m_databaseShopDetail;

        public string DetailName => m_databaseNormalDetail.DetailName;

        public string DetailDescription => m_databaseNormalDetail.DetailDescription;

        public Sprite GetSprite() => m_databaseNormalDetail.GetSprite();

        public int purchasePrice => m_databaseShopDetail.purchasePrice;

        public int sellPrice => m_databaseShopDetail.sellPrice;

        // 能力

        public bool HasItemTypes(ItemTypes itemTypes) => (m_itemTypes & itemTypes) == itemTypes;
    }
}