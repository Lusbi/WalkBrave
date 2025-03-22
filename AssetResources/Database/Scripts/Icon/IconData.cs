using System;
using UnityEngine;

namespace GameCore.Database
{
    [Serializable]
    public class IconData : Data
    {
        [SerializeField] private Sprite m_sprite;

        public Sprite sprite => m_sprite;
    }
}