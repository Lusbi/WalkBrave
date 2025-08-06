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
        // 確認是否有最後戰鬥資料
        bool hasBattleData = string.IsNullOrEmpty(StorageManager.instance.StorageData.BattleStorageData.EnemyKey) == false;
        // 讀取最後記錄中的場景
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

    // 依最後場景載入敵人資料
    // 確認可出場敵人
    // 生成對應敵人
    private void CreateBattleEnemy()
    {
        string curSceneMapKey = StorageManager.instance.StorageData?.CurrentSceneMap;
        if (string.IsNullOrEmpty(curSceneMapKey) || Database<ScenemapData>.Exists(curSceneMapKey) == false)
        {
            // 載入預設場景資料…
            curSceneMapKey = SettingsManager.instance.gameDefaultSetting.defaultScenemapReference.GetKey();
        }

        if (Database<ScenemapData>.TryLoad(curSceneMapKey, out ScenemapData scenemapData))
        {
            m_bgImage.sprite = scenemapData.ImgBgReference;
            m_bgImage.enabled = m_bgImage.sprite != null;

            // 取出合法的項目
            IReadOnlyList <RoleData> validRoleDatas = scenemapData.enemies
                .Where(data => data.ValidateFlagReferenceConditions())
                .ToList();

            // 進一步處理 validRoleDatas
            int RandomValue = UnityEngine.Random.Range(0, validRoleDatas.Count);
            m_curBattleRole = validRoleDatas[RandomValue];
            m_enemySpawn.LoadRoleData(m_curBattleRole);
        }

        PlayAnimation(ANI_ENEMYSPAWN, () => m_isAlive = true);
    }

    private void DieAction()
    {
        m_isAlive = false;
        // 更新角色死亡次數
        StorageManager.instance.StorageData.AddEnemyKillCount(m_curBattleRole.key);
        // 更新旗標
        if (m_curBattleRole.KillToAddFlagReference != null)
        {
            StorageManager.instance.StorageData.AddFlagStorageValue(m_curBattleRole.KillToAddFlagReference?.GetKey());
        }
        // 機率獲得道具
        float dropRate = m_curBattleRole.DropInfo.dropRate;
        float dropBonus = m_curBattleRole.DropInfo.dropBonus;
        StorageManager.instance.StorageData.GetEnemyKillCount(m_curBattleRole.key, out var enemyKillValue);
        float finalDropRate = Mathf.Clamp(dropRate + (dropBonus * enemyKillValue), 0, 100);
        if (UnityEngine.Random.Range(0, 100) <= finalDropRate)
        {
            string itemKey = m_curBattleRole.DropInfo.itemReference.GetKey();
            m_dropItem.SetItemKey(itemKey);
            // 獲得道具
            StorageManager.instance.StorageData.ModifyItemCount(itemKey, 1);
            eLog.Log($"獲得道具: {m_curBattleRole.DropInfo.itemReference.GetKey()}");
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
        // 生成演出中不處理
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
