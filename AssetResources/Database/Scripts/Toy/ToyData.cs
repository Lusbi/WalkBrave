using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameCore.Database
{
    [Serializable]
    public class ToyData : Data
    {
        [HideInInspector]
        [SerializeField]
        private string m_toyName = string.Empty;
        [HideInInspector]
        [SerializeField]
        private string m_toyDescription = string.Empty;

        [LabelText("獲得場景")]
        [SerializeField]
        private ScenemapReference m_scenemapReference;

        [LabelText("合成關聯")]
        [SerializeField]
        private CompositeReference m_compositeReference;

        public string toyName => m_toyName;
        public string toyDescription => m_toyDescription;
        public ScenemapReference scenemapReference => m_scenemapReference;
        public CompositeReference compositeReference => m_compositeReference;
    }
}