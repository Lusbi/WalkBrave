using System.Collections.Generic;
using GameCore.Database;

[System.Serializable]
public class StorageData
{
    private string m_currentSceneMap;
    private BattleStorageData m_battleStorageData = new BattleStorageData(string.Empty, 0);
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

    public void AddFlagStorageValue(string flagKey)
    {
        foreach (var data in m_flagStorageDatas)
        {
            if (data.FlagKey == flagKey)
            {
                data.SetValue(data.Value + 1);
                return;
            }
        }
        if (Database<FlagData>.Exists(flagKey) == false)
            return;
        // If not found, add a new entry
        var newData = new FlagStorageData(flagKey, 1);
        m_flagStorageDatas.Add(newData);
    }

    // API for EnemyStorageData
    public List<EnemyStorageData> GetEnemyStorageDatas()
    {
        return new List<EnemyStorageData>(m_enemyStorageDatas);
    }

    public void AddEnemyKillCount(string enemyKey)
    {
        foreach (var data in m_enemyStorageDatas)
        {
            if (data.EnemyKey == enemyKey)
            {
                data.SetKillValue(data.KillValue + 1);
                return;
            }
        }
        if (Database<RoleData>.Exists(enemyKey) == false)
            return;
        // If not found, add a new entry
        m_enemyStorageDatas.Add(new EnemyStorageData(enemyKey, 1));
    }

    public void GetEnemyKillCount(string enemyKey, out int killCount)
    {
        foreach (var data in m_enemyStorageDatas)
        {
            if (data.EnemyKey == enemyKey)
            {
                killCount = data.KillValue;
                return;
            }
        }
        killCount = 0; // Default value if not found
    }

    // API for ToyStorageData
    public List<ToyStorageData> GetToyStorageDatas()
    {
        return new List<ToyStorageData>(m_toyStorageDatas);
    }
    public ToyStorageData GetToyStorageData(string toyKey)
    {
        foreach (var data in m_toyStorageDatas)
        {
            if (data.ToyKey == toyKey)
            {
                return data;
            }
        }
        return null; // Return null if not found
    }
    public void AddToyCount(string toyKey)
    {
        foreach (var data in m_toyStorageDatas)
        {
            if (data.ToyKey == toyKey)
            {
                data.SetUseableValue(data.UseableValue + 1);
                return;
            }
        }
        if (Database<ToyData>.Exists(toyKey) == false)
            return;
        // If not found, add a new entry
        m_toyStorageDatas.Add(new ToyStorageData(toyKey, 1));
    }
    // API for ItemStorageData
    public List<ItemStorageData> GetItemStorageDatas()
    {
        return new List<ItemStorageData>(m_itemStorageDatas);
    }

    public ItemStorageData GetItemStorageData(string itemKey)
    {
        foreach (var data in m_itemStorageDatas)
        {
            if (data.ItemKey == itemKey)
            {
                return data;
            }
        }
        return null; // Return null if not found
    }

    public void ModifyItemCount(string itemKey, int count)
    {
        foreach (var data in m_itemStorageDatas)
        {
            if (data.ItemKey == itemKey)
            {
                data.SetItemValue(data.ItemValue + count);
                return;
            }
        }
        if (Database<ItemData>.Exists(itemKey) == false)
            return;
        // If not found, add a new entry
        m_itemStorageDatas.Add(new ItemStorageData(itemKey, count));
    }
}


[System.Serializable]
public class FlagStorageData
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