using GameCore.Database;
using GameCore.Log;
using UnityEngine;

public class FlagProcess : IProcessEvent
{
    public ProcessType processType { get; set; }
    private string m_flagKey;
    
    public FlagProcess(string flagKey)
    {
        m_flagKey = flagKey;
    }

    public void Inilization()
    {
        m_flagKey = string.Empty;
        processType = ProcessType.None;
    }

    public bool Execute()
    {
        if (Database<FlagData>.Exists(m_flagKey) == false)
        {
            eLog.Error("�X�и�Ʀ����A�Э��s�ˬd");
            return false;
        }

        StorageManager.instance.StorageData.AddFlagStorageValue(m_flagKey);
        processType = ProcessType.Succeeded;
        return true;
    }
}
