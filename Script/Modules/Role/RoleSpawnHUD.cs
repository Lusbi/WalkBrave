using System;
using System.Numerics;
using BigMath;
using GameCore;
using GameCore.Database;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoleSpawnHUD : MonoBehaviour, IInitlization
{
    [Header("UI Components")]
    [SerializeField] private Image m_enemyImage; // 敵人圖片
    [SerializeField] private TextMeshProUGUI m_enemyHealthText; // 敵人血量數值
    [SerializeField] private Image m_enemyHealthBar; // 敵人血條表現
    [SerializeField] private RectTransform m_enemyRect;
    [SerializeField] private TextMeshProUGUI m_talkText;
    private Action<bool> m_dieActionCallBack;
    private BigNumber m_maxHitCount; // 最大血量
    private BigNumber m_curHitCount; // 當前血量

    public void Initlization(Action callBack = null)
    {
        m_curHitCount = 0;
        m_enemyImage.sprite = null;
        m_enemyHealthText.text = "0";
        //m_enemyNameText.text = "Enemy";
        m_enemyHealthBar.fillAmount = 1f; // 預設血條為滿
        m_dieActionCallBack = null;
        m_enemyRect = m_enemyImage.GetComponent<RectTransform>();
    }

    public void ApplyDieAction(Action<bool> dieActionCallBack)
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
        m_enemyImage.enabled = m_enemyImage.sprite != null;
        // 取得敵人名稱
        // m_enemyNameText.text = roleData.RoleName;

        m_maxHitCount = roleData.HitCount;
        m_curHitCount = roleData.HitCount;
        // 更新血條表現
        UpdateHealthBar(roleData.HitCount, roleData.HitCount);

        float rotationY = roleData.DirectionType == DirectionType.Right ? 180f : 0f;
        m_enemyRect.localEulerAngles = new UnityEngine.Vector3(0, rotationY, 0); // 面向右邊

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
    private void UpdateHealthBar(BigNumber currentHealth, BigNumber maxHealth)
    {
        m_enemyHealthText.text = $"{m_curHitCount}/{m_maxHitCount}";
        // 計算血量百分比
        m_enemyHealthBar.fillAmount = (float)((double)currentHealth.Value / (double)maxHealth.Value);
    }

    public void Hit(BigNumber damageValue , bool isTomatoModel = false)
    {
        m_curHitCount = m_curHitCount - damageValue ;
        if (m_curHitCount < 0)
            m_curHitCount = 0;
        UpdateHealthBar(m_curHitCount, m_maxHitCount);

        if (m_curHitCount <= 0)
        {
            m_dieActionCallBack?.Invoke(isTomatoModel);
        }
        Shake();
    }

    public void EnemyDieAnimation(Action callBack)
    {
        TweenSettings tweenSettings = new()
        {
            duration = 0.3f,
            cycleMode = CycleMode.Rewind,
            ease = Ease.Default,
        };
        PrimeTween.Tween.Custom(1f, 0f, tweenSettings, (x) =>
        {
            m_enemyImage.fillAmount = x;
            Shake();
        }).OnComplete(() => { callBack?.Invoke(); });
    }

    private void Shake()
    {
        // 圖片震動(0.1秒)，使用 Tween
        ShakeSettings shakeSettings = new()
        {
            // 建立簡易的震動效果
            duration = 0.1f,
            strength = UnityEngine.Random.insideUnitSphere * 5, // 震動強度
            cycles = 1, // 震動次數
            easeBetweenShakes = Ease.OutSine, // 震動過渡效果
            frequency = 3,
        };
        PrimeTween.Tween.PunchLocalPosition(m_enemyRect, shakeSettings);
    }
}
