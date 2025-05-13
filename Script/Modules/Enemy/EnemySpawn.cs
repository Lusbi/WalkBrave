using System;
using GameCore;
using GameCore.Database;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawn : MonoBehaviour , IInitlization
{
    [Header("UI Components")]
    [SerializeField] private Image m_enemyImage; // �ĤH�Ϥ�
    [SerializeField] private TextMeshProUGUI m_enemyHealthText; // �ĤH��q�ƭ�
    [SerializeField] private TextMeshProUGUI m_enemyNameText; // �ĤH�W��
    [SerializeField] private Image m_enemyHealthBar; // �ĤH�����{

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
        // ���o�ĤH�W��
        m_enemyNameText.text = roleData.RoleName;

        m_maxHitCount = roleData.HitCount;
        m_curHitCount = roleData.HitCount;
        // ��s�����{
        UpdateHealthBar(roleData.HitCount, roleData.HitCount);
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
        m_curHitCount -= GameProcess.Instance.hitRate * 1;
        UpdateHealthBar(m_curHitCount , m_maxHitCount);

        if (m_curHitCount <= 0)
        {
            m_dieActionCallBack?.Invoke();
        }
    }
}
