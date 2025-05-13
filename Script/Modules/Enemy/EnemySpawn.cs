using System;
using GameCore;
using GameCore.Database;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawn : MonoBehaviour , IInitlization
{
    [Header("UI Components")]
    [SerializeField] private Image m_enemyImage; // 敵人圖片
    [SerializeField] private TextMeshProUGUI m_enemyHealthText; // 敵人血量數值
    [SerializeField] private TextMeshProUGUI m_enemyNameText; // 敵人名稱
    [SerializeField] private Image m_enemyHealthBar; // 敵人血條表現

    private Action m_dieActionCallBack;
    private int m_maxHitCount; // 最大血量
    private int m_curHitCount; // 當前血量

    public void Initlization(Action callBack = null)
    {
        m_curHitCount = 0;
        m_enemyImage.sprite = null;
        m_enemyHealthText.text = "0";
        m_enemyNameText.text = "Enemy";
        m_enemyHealthBar.fillAmount = 1f; // 預設血條為滿
        m_dieActionCallBack = null;
    }

    public void ApplyDieAction(Action dieActionCallBack)
    {
        m_dieActionCallBack = dieActionCallBack;
    }

    /// <summary>
    /// 載入敵人資料
    /// </summary>
    /// <param name="roleData"></param>
    public void LoadRoleData(RoleData roleData)
    {
        // 取得敵人圖片
        m_enemyImage.sprite = roleData.EnemyIcon;
        // 取得敵人名稱
        m_enemyNameText.text = roleData.RoleName;

        m_maxHitCount = roleData.HitCount;
        m_curHitCount = roleData.HitCount;
        // 更新血條表現
        UpdateHealthBar(roleData.HitCount, roleData.HitCount);
    }

    /// <summary>
    /// 載入最後戰鬥中的敵人資料
    /// </summary>
    /// <param name="battleStorageData"></param>
    public void ApplyBattleStorageData(BattleStorageData battleStorageData)
    {
        if (Database<RoleData>.TryLoad(battleStorageData.EnemyKey, out var roleData))
        {
            LoadRoleData(roleData);

            m_curHitCount = battleStorageData.RemainHit;
            UpdateHealthBar(m_curHitCount, m_maxHitCount);
        }

    }

    /// <summary>
    /// 更新敵人血條表現
    /// </summary>
    /// <param name="currentHealth">當前血量</param>
    /// <param name="maxHealth">最大血量</param>
    private void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        m_enemyHealthText.text = $"{m_curHitCount}/{m_maxHitCount}";
        // 計算血量百分比
        m_enemyHealthBar.fillAmount = (float)currentHealth / maxHealth;
    }

    public void Hit()
    {
        m_curHitCount -= GameProcess.Instance.hitRate * 1;
        UpdateHealthBar(m_curHitCount , m_maxHitCount);

        if (m_curHitCount <= 0)
        {
            m_dieActionCallBack?.Invoke();
        }
    }
}
