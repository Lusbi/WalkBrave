using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using BigMath;
using GameCore.Database;
using GameCore.Event;

[System.Serializable]
public class StorageData
{
    private string m_currentSceneMap;
    private BattleStorageData m_battleStorageData = new BattleStorageData(string.Empty, 0);
    private List<FlagStorageData> m_flagStorageDatas = new List<FlagStorageData>();
    private List<EnemyStorageData> m_enemyStorageDatas = new List<EnemyStorageData>();

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

        EventManager.instance.Notify(new FlagUpdateEvent(flagKey));
    }

    // API for EnemyStorageData
    public List<EnemyStorageData> GetEnemyStorageDatas()
    {
        return new List<EnemyStorageData>(m_enemyStorageDatas);
    }

    public EnemyStorageData GetEnemyStorageData(string enemyKey)
    {
        foreach (var data in m_enemyStorageDatas)
        {
            if (data.EnemyKey == enemyKey)
            {
                return data;
            }
        }
        return null; // Default value if not found
    }

    public void AddEnemyKillCount(string enemyKey , bool tomatoModel = false)
    {
        foreach (var data in m_enemyStorageDatas)
        {
            if (data.EnemyKey == enemyKey)
            {
                data.SetKillValue(data.KillValue + 1 , tomatoModel);
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

    public void ApplyLastBattleStorageData(string enemyKey , BigInteger remainHit)
    {
        m_battleStorageData = new BattleStorageData(enemyKey, remainHit);
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
    private bool m_silverClear;
    private bool m_goldenClear;

    public string EnemyKey => m_enemyKey;
    public int KillValue => m_killValue;
    public bool silverClear => m_silverClear;
    public bool goldenClear => m_goldenClear;
    public EnemyStorageData(string enemyKey, int killValue)
    {
        m_enemyKey = enemyKey;
        SetKillValue(killValue, false);
    }
    public void SetKillValue(int killValue , bool tomatoModel)
    {
        m_killValue = killValue;

        if (m_killValue > 0)
        {
            if (tomatoModel)
            {
                m_goldenClear = true;
            }
            m_silverClear = true;
        }
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
    private BigNumber m_remainHit;

    public string EnemyKey => m_enemyKey;
    public BigNumber RemainHit => m_remainHit;
    public BattleStorageData(string enemyKey, BigNumber remainHit)
    {
        m_enemyKey = enemyKey;
        m_remainHit = remainHit;
    }
    public void SetRemainHit(int remainHit)
    {
        m_remainHit = remainHit;
    }
}