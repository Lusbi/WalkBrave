using GameCore.CSV;
using GameCore.Database;
using UnityEngine;

[System.Serializable]
public class DropInfo : SerializedCSV
{
    [SerializeField] private ItemReference m_itemReference;
    [SerializeField] private float m_dropRate;
    [SerializeField] private float m_dropBonus;

    public ItemReference itemReference => m_itemReference;
    public float dropRate => m_dropRate;
    public float dropBonus => m_dropBonus;

    public override void FromCSV(string text)
    {
        // 假設格式為 itemReference_rate_bonus
        var parts = text.Split('_');
        if (parts.Length == 3)
        {
            m_itemReference = new ItemReference();
            m_itemReference.SetKey(parts[0]);
            if (float.TryParse(parts[1], out var rate))
            {
                m_dropRate = rate;
            }
            if (float.TryParse(parts[2], out var bonus))
            {
                m_dropBonus = bonus;
            }
        }
    }

    public override string ToCSV()
    {
        return $"{m_itemReference.GetKey()}_{m_dropRate:F0}_{m_dropBonus:F0}";
    }
}
