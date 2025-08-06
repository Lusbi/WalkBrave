using System;
using Febucci.UI.Core;
using GameCore;
using GameCore.Database;
using PrimeTween;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawn : MonoBehaviour , IInitlization
{
    [Header("UI Components")]
    [SerializeField] private Image m_enemyImage; // �ĤH�Ϥ�
    [SerializeField] private TextMeshProUGUI m_enemyHealthText; // �ĤH��q�ƭ�
    [SerializeField] private TextMeshProUGUI m_enemyNameText; // �ĤH�W��
    [SerializeField] private Image m_enemyHealthBar; // �ĤH�����{
    [SerializeField] private RectTransform m_enemyRect;
    private Action m_dieActionCallBack;
    private int m_maxHitCount; // �̤j��q
    private int m_curHitCount; // ��e��q

    public void Initlization(Action callBack = null)
    {
        m_curHitCount = 0;
        m_enemyImage.sprite = null;
        m_enemyHealthText.text = "0";
        m_enemyNameText.text = "Enemy";
        m_enemyHealthBar.fillAmount = 1f; // �w�]�������
        m_dieActionCallBack = null;
        m_enemyRect = m_enemyImage.GetComponent<RectTransform>();
    }

    public void ApplyDieAction(Action dieActionCallBack)
    {
        m_dieActionCallBack = dieActionCallBack;
    }

    /// <summary>
    /// ���J�ĤH���
    /// </summary>
    /// <param name="roleData"></param>
    public void LoadRoleData(RoleData roleData)
    {
        // ���o�ĤH�Ϥ�
        m_enemyImage.sprite = roleData.EnemyIcon;
        m_enemyImage.enabled = m_enemyImage.sprite != null;
        // ���o�ĤH�W��
        m_enemyNameText.text = roleData.RoleName;

        m_maxHitCount = roleData.HitCount;
        m_curHitCount = roleData.HitCount;
        // ��s�����{
        UpdateHealthBar(roleData.HitCount, roleData.HitCount);

        float rotationY = roleData.DirectionType == DirectionType.Right ? 180f : 0f;
        m_enemyRect.localEulerAngles = new Vector3(0, rotationY, 0); // ���V�k��
        
    }

    /// <summary>
    /// ���J�̫�԰������ĤH���
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
    /// ��s�ĤH�����{
    /// </summary>
    /// <param name="currentHealth">��e��q</param>
    /// <param name="maxHealth">�̤j��q</param>
    private void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        m_enemyHealthText.text = $"{m_curHitCount}/{m_maxHitCount}";
        // �p���q�ʤ���
        m_enemyHealthBar.fillAmount = (float)currentHealth / maxHealth;
    }

    public void Hit()
    {
        m_curHitCount = Mathf.Clamp( m_curHitCount - SettingsManager.instance.gameDefaultSetting.defaultClickDamage , 0 , int .MaxValue);
        UpdateHealthBar(m_curHitCount , m_maxHitCount);

        if (m_curHitCount <= 0)
        {
            m_dieActionCallBack?.Invoke();
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
        // �Ϥ��_��(0.1��)�A�ϥ� Tween
        ShakeSettings shakeSettings = new()
        {
            // �إ�²�����_�ʮĪG
            duration = 0.1f,
            strength = UnityEngine.Random.insideUnitSphere * 5, // �_�ʱj��
            cycles = 1, // �_�ʦ���
            easeBetweenShakes = Ease.OutSine, // �_�ʹL��ĪG
            frequency = 3,
        };
        PrimeTween.Tween.PunchLocalPosition(m_enemyRect, shakeSettings);
    }
}
