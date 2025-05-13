using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StorageData
{
    private string m_currentSceneMap;
    private BattleStorageData m_battleStorageData = new BattleStorageData(string.Empty , 0);
    private List<FlagStorageData> m_flagStorageDatas = new List<FlagStorageData>();
    private List<EnemyStorageData> m_enemyStorageDatas = new List<EnemyStorageData>();
    private List<ToyStorageData> m_toyStorageDatas = new List<ToyStorageData>();
    private List<ItemStorageData> m_itemStorageDatas = new List<ItemStorageData>();

    // Property to get the current scene map
    public string CurrentSceneMap => m_currentSceneMap;
    public BattleStorageData BattleStorageData => m_battleStorageData;

    // API to set the current scene map
    public void SetCurrentSceneMap(string sceneMapKey)
    {
        m_currentSceneMap = sceneMapKey;
    }

    // API for FlagStorageData
    public void AddFlagStorageData(FlagStorageData data)
    {
        m_flagStorageDatas.Add(data);
    }

    public void RemoveFlagStorageData(FlagStorageData data)
    {
        m_flagStorageDatas.Remove(data);
    }

    public List<FlagStorageData> GetFlagStorageDatas()
    {
        return new List<FlagStorageData>(m_flagStorageDatas);
    }

    public int GetFlagStorageValue(string key)
    {
        foreach (var data in m_flagStorageDatas)
        {
            if (data.FlagKey == key)
            {
                return data.Value;
            }
        }
        return 0; // Default value if not found
    }

    // API for EnemyStorageData
    public void AddEnemyStorageData(EnemyStorageData data)
    {
        m_enemyStorageDatas.Add(data);
    }

    public void RemoveEnemyStorageData(EnemyStorageData data)
    {
        m_enemyStorageDatas.Remove(data);
    }

    public List<EnemyStorageData> GetEnemyStorageDatas()
    {
        return new List<EnemyStorageData>(m_enemyStorageDatas);
    }

    // API for ToyStorageData
    public void AddToyStorageData(ToyStorageData data)
    {
        m_toyStorageDatas.Add(data);
    }

    public void RemoveToyStorageData(ToyStorageData data)
    {
        m_toyStorageDatas.Remove(data);
    }

    public List<ToyStorageData> GetToyStorageDatas()
    {
        return new List<ToyStorageData>(m_toyStorageDatas);
    }

    // API for ItemStorageData
    public void AddItemStorageData(ItemStorageData data)
    {
        m_itemStorageDatas.Add(data);
    }

    public void RemoveItemStorageData(ItemStorageData data)
    {
        m_itemStorageDatas.Remove(data);
    }

    public List<ItemStorageData> GetItemStorageDatas()
    {
        return new List<ItemStorageData>(m_itemStorageDatas);
    }
}


[System.Serializable]
public class  FlagStorageData
{
    private string m_flagKey;
    private int m_value;

    public string FlagKey => m_flagKey;
    public int Value => m_value;
    public FlagStorageData(string flagKey, int value)
    {
        m_flagKey = flagKey;
        m_value = value;
    }
    public void SetValue(int value)
    {
        m_value = value;
    }
}

[System.Serializable]
public class EnemyStorageData
{
    private string m_enemyKey;
    private int m_killValue;
    
    public string EnemyKey => m_enemyKey;
    public int KillValue => m_killValue;
    public EnemyStorageData(string enemyKey, int killValue)
    {
        m_enemyKey = enemyKey;
        m_killValue = killValue;
    }
    public void SetKillValue(int killValue)
    {
        m_killValue = killValue;
    }
}

[System.Serializable]
public class ToyStorageData
{
    private string m_toyKey;
    private int m_useableValue;

    public string ToyKey => m_toyKey;
    public int UseableValue => m_useableValue;
    public ToyStorageData(string toyKey, int useableValue)
    {
        m_toyKey = toyKey;
        m_useableValue = useableValue;
    }
    public void SetUseableValue(int useableValue)
    {
        m_useableValue = useableValue;
    }
}

[System.Serializable]
public class ItemStorageData
{
    private string m_itemKey;
    private int m_itemValue;

    public string ItemKey => m_itemKey;
    public int ItemValue => m_itemValue;
    public ItemStorageData(string itemKey, int itemValue)
    {
        m_itemKey = itemKey;
        m_itemValue = itemValue;
    }
    public void SetItemValue(int itemValue)
    {
        m_itemValue = itemValue;
    }
}

[System.Serializable]
public class BattleStorageData
{
    private string m_enemyKey;
    private int m_remainHit;

    public string EnemyKey => m_enemyKey;
    public int RemainHit => m_remainHit;
    public BattleStorageData(string enemyKey, int remainHit)
    {
        m_enemyKey = enemyKey;
        m_remainHit = remainHit;
    }
    public void SetRemainHit(int remainHit)
    {
        m_remainHit = remainHit;
    }
}