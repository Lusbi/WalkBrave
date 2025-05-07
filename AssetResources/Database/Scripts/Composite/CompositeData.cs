using System;
using UnityEngine;

namespace GameCore.Database
{
    [Serializable]
    public class CompositeData : Data
    {
        [SerializeField] private CompositeInfo m_compositeInfo;

        public CompositeInfo compositeInfo => m_compositeInfo;
    }
}