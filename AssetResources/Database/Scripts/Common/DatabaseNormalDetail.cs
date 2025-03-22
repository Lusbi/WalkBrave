using GameCore.CSV;
using UnityEngine;

namespace GameCore.Database
{
    [System.Serializable]
    public class DatabaseNormalDetail : SerializedCSV , IDataDetail
    {
        [SerializeField] private string m_textRefName;
        [SerializeField] private string m_textRefDetail;
        [SerializeField] private IconReference m_iconRefSmallIcon;

        public string DetailName => LocalizationManager.instance.GetLocalization(m_textRefName);

        public string DetailDescription => LocalizationManager.instance.GetLocalization(m_textRefDetail);

        public Sprite GetSprite()
        {
            if (m_iconRefSmallIcon.TryLoad(out IconData iconData))
            {
                return iconData.sprite;
            }
            return null;
        }

        public override void FromCSV(string text)
        {
            string[] values = text.Split(new char[] { ',' });
            if (values.Length > 0)
                m_textRefName = values[0];
            if (values.Length > 1)
                m_textRefDetail = values[1];
            if (values.Length > 2)
            {
                m_iconRefSmallIcon = new IconReference();
                m_iconRefSmallIcon.SetKey(values[2]);
            }
        }

        public override string ToCSV()
        {
            return $"{m_textRefName},{m_textRefDetail},{m_iconRefSmallIcon.GetKey()}";
        }
    }
}