using GameCore.CSV;
using GameCore.Utils;
using UnityEngine;

[System.Serializable]
public class DatabaseShopDetail : SerializedCSV , IDataShopDetail
{
    [SerializeField] private int m_purchasePrice;
    [SerializeField] private int m_sellPrice;

    public int purchasePrice => m_purchasePrice;
    public int sellPrice => m_sellPrice;

    public override void FromCSV(string text)
    {
        string[] values = text.SplitToField(StringArrayUtils.FormatType.Csv ,System.StringSplitOptions.RemoveEmptyEntries);
        if (values.Length > 0)
            m_purchasePrice = values[0].ToInt();
        if (values.Length > 1)
            m_sellPrice = values[1].ToInt();
    }

    public override string ToCSV()
    {
        return $"{m_purchasePrice},{m_sellPrice}";
    }
}
