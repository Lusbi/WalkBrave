using System;
using GameCore;
using GameCore.Event;
using GameCore.Log;
using GameCore.UI;
using UnityEngine;
using UnityEngine.UI;

public class RoleItemHUD : MonoBehaviour , IInitlization
{
    [SerializeField] private Image m_iconImg;
    [SerializeField] private Image m_signImg;
    [SerializeField] private UIButton m_selectBtn;
    [SerializeField] private Image m_startImg;
    [SerializeField] private Sprite m_silverSprite;
    [SerializeField] private Sprite m_goldenSprite;

    private bool m_isNew;
    private RoleInfo m_roleInfo;


    public void Apply(RoleInfo roleInfo)
    {
        m_selectBtn.ResetButtonState();
        m_roleInfo = roleInfo;
        bool locked = roleInfo.roleData.ValidateFlagReferenceConditions();
        int killValue = 0;
        bool isGolden = false;
        bool isSilver = false;
        var storageData = StorageManager.instance.StorageData.GetEnemyStorageData(roleInfo.roleData.key);
        killValue = storageData != null ? storageData.KillValue : 0;
        isGolden = storageData != null ? storageData.goldenClear : false;
        isSilver = storageData != null ? storageData.silverClear : false;
        if (m_iconImg)
        {
            m_iconImg.sprite = m_roleInfo.roleData.EnemyIcon;
            m_iconImg.enabled = m_iconImg.sprite != null;
        }
        if (m_signImg)
        {
            m_isNew = killValue == 0;
            m_signImg.enabled = m_isNew && locked;
        }

        m_selectBtn.locked = !locked;
        if (locked == false)
        {
            // 檢查是不是當前選擇的物件
            bool isCurSelectRole = StorageManager.instance.StorageData.BattleStorageData.EnemyKey == roleInfo.roleData.key;
            m_selectBtn.selected = isCurSelectRole;
        }

        if (m_signImg.enabled)
        {
            eLog.Log($"{roleInfo.roleData.key} => Lock:{m_selectBtn.locked} Select:{m_selectBtn.selected}");
        }

        if (m_startImg)
        {
            m_startImg.sprite = isGolden ? m_goldenSprite : (isSilver ? m_silverSprite : m_startImg.sprite);
            m_startImg.enabled = killValue > 0;
        }
    }

    public void Initlization(Action callBack = null)
    {
        if (m_iconImg)
        {
            m_iconImg.enabled = false;
        }
        if (m_signImg)
        {
            m_signImg.enabled = false;
        }
        if (m_selectBtn && m_selectBtn.onClicked == null)
        {
            m_selectBtn.onClicked = OnClick;
        }
    }

    private void OnClick(UIButton button)
    {
        eLog.Log($"選擇到的角色為：{m_roleInfo.roleData.roleSortId}:{m_roleInfo.roleData.RoleName}");
        EventManager.instance.Notify(new SetBattleRoleEvent(m_roleInfo.roleData.key));
    }
}
