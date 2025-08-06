using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Database;
using GameCore.Log;
using UnityEngine;
using UnityEngine.UI;

public class BattlePanel : PanelBase
{
    private const string ANI_ENEMYSPAWN = "SpawnEnemyAnim";
    private const string ANI_DROPITEM = "DropItemAnim";

    [SerializeField] private RectTransform m_petParent;

    [SerializeField] private RectTransform m_enemyParent;

    [SerializeField] private Image m_bgImage;

    [SerializeField] private EnemySpawn m_enemySpawn;

    [SerializeField] private DropItem m_dropItem;

    [SerializeField] private Animation m_ani;
    private RoleData m_curBattleRole;
    private bool m_isAlive = false;
    public override void Initlization(Action callBack = null)
    {
        base.Initlization(callBack);

        if (m_enemySpawn)
        {
            m_enemySpawn.Initlization();
            m_enemySpawn.ApplyDieAction(DieAction);
        }
    }

    public override void ActiveOn()
    {
        // �T�{�O�_���̫�԰����
        bool hasBattleData = string.IsNullOrEmpty(StorageManager.instance.StorageData.BattleStorageData.EnemyKey) == false;
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
        uiGameMainView.desktopPetController.ChangeState(PetState.Battle);

        BattleManager.instance.Register(Hit);
    }

    public override void ActiveOff()
    {
        BattleManager.instance.Register(null);
    }

    // �̳̫�������J�ĤH���
    // �T�{�i�X���ĤH
    // �ͦ������ĤH
    private void CreateBattleEnemy()
    {
        string curSceneMapKey = StorageManager.instance.StorageData?.CurrentSceneMap;
        if (string.IsNullOrEmpty(curSceneMapKey) || Database<ScenemapData>.Exists(curSceneMapKey) == false)
        {
            // ���J�w�]������ơK
            curSceneMapKey = SettingsManager.instance.gameDefaultSetting.defaultScenemapReference.GetKey();
        }

        if (Database<ScenemapData>.TryLoad(curSceneMapKey, out ScenemapData scenemapData))
        {
            m_bgImage.sprite = scenemapData.ImgBgReference;
            m_bgImage.enabled = m_bgImage.sprite != null;

            // ���X�X�k������
            IReadOnlyList <RoleData> validRoleDatas = scenemapData.enemies
                .Where(data => data.ValidateFlagReferenceConditions())
                .ToList();

            // �i�@�B�B�z validRoleDatas
            int RandomValue = UnityEngine.Random.Range(0, validRoleDatas.Count);
            m_curBattleRole = validRoleDatas[RandomValue];
            m_enemySpawn.LoadRoleData(m_curBattleRole);
        }

        PlayAnimation(ANI_ENEMYSPAWN, () => m_isAlive = true);
    }

    private void DieAction()
    {
        m_isAlive = false;
        // ��s���⦺�`����
        StorageManager.instance.StorageData.AddEnemyKillCount(m_curBattleRole.key);
        // ��s�X��
        if (m_curBattleRole.KillToAddFlagReference != null)
        {
            StorageManager.instance.StorageData.AddFlagStorageValue(m_curBattleRole.KillToAddFlagReference?.GetKey());
        }
        // ���v��o�D��
        float dropRate = m_curBattleRole.DropInfo.dropRate;
        float dropBonus = m_curBattleRole.DropInfo.dropBonus;
        StorageManager.instance.StorageData.GetEnemyKillCount(m_curBattleRole.key, out var enemyKillValue);
        float finalDropRate = Mathf.Clamp(dropRate + (dropBonus * enemyKillValue), 0, 100);
        if (UnityEngine.Random.Range(0, 100) <= finalDropRate)
        {
            string itemKey = m_curBattleRole.DropInfo.itemReference.GetKey();
            m_dropItem.SetItemKey(itemKey);
            // ��o�D��
            StorageManager.instance.StorageData.ModifyItemCount(itemKey, 1);
            eLog.Log($"��o�D��: {m_curBattleRole.DropInfo.itemReference.GetKey()}");
            EnemyDieAnimation(
                () =>
                {
                    PlayAnimation(ANI_DROPITEM, CreateBattleEnemy);
                }
            );
        }
        else
        {
            EnemyDieAnimation(CreateBattleEnemy);
        }
    }

    public void Hit(string hit)
    {
        if (active == false)
            return;
        // �ͦ��t�X�����B�z
        if (m_isAlive == false)
            return;

        if (m_enemySpawn)
            m_enemySpawn.Hit();
    }

    private void EnemyDieAnimation(Action callBack = null)
    {
        m_enemySpawn.EnemyDieAnimation(callBack);
    }

    private void PlayAnimation(string ClipName, Action callBack = null)
    {
        AnimationClip clip = m_ani.GetClip(ClipName);
        m_ani.Play(ClipName);
        PrimeTween.Tween.Delay(clip.length, () =>
        {
            callBack?.Invoke();
        });
    }
}
