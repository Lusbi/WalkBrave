using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameCore.Database
{
    [Serializable]
    public class ItemData : Data
    {
        
        [SerializeField]
        private string m_itemName = string.Empty;
        
        [SerializeField]
        private string m_itemDescription = string.Empty;
        
        [SerializeField]
        private RoleReference m_roleReference;
        
        [SerializeField]
        private ToyReference[] m_toyReferences;
        
        [SerializeField]
        private Sprite m_itemIcon; // 新增道具圖示欄位


        public string itemName => m_itemName;
        public string itemDescription => m_itemDescription;
        public RoleReference roleReference => m_roleReference;
        public IReadOnlyList<ToyReference> toyReferences => m_toyReferences;
        public Sprite itemIcon => m_itemIcon; // 新增道具圖示屬性

#if UNITY_EDITOR
        public override string GetLog()
        {
            var toyReferencesLog = string.Join(", ", m_toyReferences.Select(tr => tr.GetKey()));
            return $"{base.GetLog()}, itemName = {m_itemName}, itemDescription = {m_itemDescription}, roleReference = {m_roleReference.GetKey()}, toyReferences = [{toyReferencesLog}], itemIcon = {m_itemIcon?.name ?? "None"}";
        }
#endif
    }
}