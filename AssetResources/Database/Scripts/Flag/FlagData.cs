using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.Database
{
    [System.Serializable]
    public struct FlagStageInfo
    {
        public int index;
        public string comment;
    }


    [Serializable]
    public class FlagData : Data
    {
        [SerializeField] private List<FlagStageInfo> m_flagStageInfos;

        public IReadOnlyList<FlagStageInfo> flagStageInfos => m_flagStageInfos;
    }
}