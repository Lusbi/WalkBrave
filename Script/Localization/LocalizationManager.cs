using UnityEngine;
using GameCore;
using System;
using System.Collections;
using UnityEngine.Localization.Settings;
using GameCore.Log;
using UnityEngine.Localization.Tables;

public class LocalizationManager : Singleton<LocalizationManager>, IInitlization
{
    private const string TABLE_NAME = "LocalizationCollection";
    private StringTable m_localizationStringTable;
    private Action m_initlizationCallBack;

    public void Initlization(Action callBack = null)
    {
        m_initlizationCallBack = callBack;
    }

    private IEnumerator Load()
    {
        m_localizationStringTable = null;
        var localizationOperation = LocalizationSettings.StringDatabase.GetTableAsync(TABLE_NAME);
        yield return localizationOperation;

        if (localizationOperation.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Failed)
        {
            eLog.Error($"{GetType().ToString()}���J�h��y�t����");
            m_initlizationCallBack?.Invoke();
            yield break;
        }

        m_localizationStringTable = localizationOperation.Result;
        m_initlizationCallBack?.Invoke();
    }

    /// <summary>
    /// ���h���
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public string GetLocalization(string key)
    {
        if (m_localizationStringTable == null)
        {
            return "�h��y�t���J���ѡA�L�kŪ���C";
        }

        return m_localizationStringTable.GetEntry(key).GetLocalizedString();
    }

}
