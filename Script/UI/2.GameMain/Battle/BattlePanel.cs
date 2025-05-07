using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Database;
using UnityEngine;

public class BattlePanel : PanelBase
{
    [SerializeField] private RectTransform m_petParent;

    [SerializeField] private RectTransform m_enemyParent;

    [SerializeField] private EnemySpawn m_enemySpawn;

    private GameMainView m_gameMainView;

    public override void Initlization(Action callBack = null)
    {
        base.Initlization(callBack);

        if (m_enemySpawn)
        {
            m_enemySpawn.ApplyDieAction(DieAction);
            m_enemySpawn.Initlization();
        }
    }

    public override void ActiveOn()
    {
        // �T�{�O�_���̫�԰����
        bool hasBattleData = string.IsNullOrEmpty(StorageManager.instance.StorageData.BattleStorageData.EnemyKey);
        // Ū���̫�O����������
        if (hasBattleData)
        {
            m_enemySpawn.ApplyBattleStorageData(StorageManager.instance.StorageData.BattleStorageData);
        }
        else
        {
            CreateBattleEnemy();
        }

        uiGameMainView.desktopPetController.SetParent(m_petParent);
    }

    // �̳̫�������J�ĤH���
    // �T�{�i�X���ĤH
    // �ͦ������ĤH
    private void CreateBattleEnemy()
    {
        string curSceneMapKey = StorageManager.instance.StorageData.CurrentSceneMap;
        if (Database<ScenemapData>.TryLoad(curSceneMapKey, out ScenemapData scenemapData))
        {
            // ���X�X�k������
            IReadOnlyList<RoleData> validRoleDatas = scenemapData.enemies
                .Where(data => data.ValidateFlagReferenceConditions())
                .ToList();

            // �i�@�B�B�z validRoleDatas
            int RandomValue = UnityEngine.Random.Range(0, validRoleDatas.Count);
            RoleData roleData = validRoleDatas[RandomValue];
            m_enemySpawn.LoadRoleData(roleData);
        }
    }

    private void DieAction()
    {
        // �M������
        // ��s���⦺�`����
        // ���v��o�D��
    }

    public void Hit()
    {
        if (active == false)
            return;

        if (m_enemySpawn)
            m_enemySpawn.Hit();
    }
}
