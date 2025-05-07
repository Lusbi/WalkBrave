using System;
using System.Collections.Generic;
using GameCore.CSV;
using GameCore.Database;
using UnityEngine;


[System.Serializable]
public class CompositeData : SerializedCSV
{
    [SerializeField] private ItemReference m_itemReference;
    [SerializeField] private int m_amount;

    public ItemReference itemReference => m_itemReference;

    public int amount => m_amount;

    public override void FromCSV(string text)
    {
        if (string.IsNullOrEmpty(text))
            throw new ArgumentException("Input CSV text cannot be null or empty.");

        var parts = text.Split('_');
        if (parts.Length != 2)
            throw new FormatException("CSV format is invalid. Expected format: ItemReference_Amount");

        m_itemReference = new ItemReference();
        m_itemReference.SetKey(parts[0]);
        if (!int.TryParse(parts[1], out m_amount))
            throw new FormatException("Amount must be a valid integer.");
    }

    public override string ToCSV()
    {
        if (m_itemReference == null || string.IsNullOrEmpty(m_itemReference.GetKey()))
            throw new InvalidOperationException("ItemReference is not properly set.");

        return $"{m_itemReference.GetKey()}_{m_amount}";
    }
}

[System.Serializable]
public class CompositeInfo : SerializedCSV
{
    [SerializeField] private CompositeData[] m_compositeDatas;

    public IReadOnlyList<CompositeData> CompositeDatas => Array.AsReadOnly(m_compositeDatas);

    public override void FromCSV(string text)
    {
        if (string.IsNullOrEmpty(text))
            throw new ArgumentException("Input CSV text cannot be null or empty.");

        var parts = text.Split('&');
        m_compositeDatas = new CompositeData[parts.Length];

        for (int i = 0; i < parts.Length; i++)
        {
            var compositeData = new CompositeData();
            compositeData.FromCSV(parts[i]);
            m_compositeDatas[i] = compositeData;
        }
    }

    public override string ToCSV()
    {
        if (m_compositeDatas == null || m_compositeDatas.Length == 0)
            throw new InvalidOperationException("CompositeDatas is not properly set.");

        return string.Join("&", Array.ConvertAll(m_compositeDatas, data => data.ToCSV()));
    }
}
