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

    private RoleData m_curBattleRole;
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
        string curSceneMapKey = StorageManager.instance.StorageData.CurrentSceneMap;
        if (Database<ScenemapData>.TryLoad(curSceneMapKey, out ScenemapData scenemapData))
        {
            // 取出合法的項目
            IReadOnlyList<RoleData> validRoleDatas = scenemapData.enemies
                .Where(data => data.ValidateFlagReferenceConditions())
                .ToList();

            // 進一步處理 validRoleDatas
            int RandomValue = UnityEngine.Random.Range(0, validRoleDatas.Count);
            m_curBattleRole = validRoleDatas[RandomValue];
            m_enemySpawn.LoadRoleData(m_curBattleRole);
        }
    }

    private void DieAction()
    {
        // 清除角色

        // 更新角色死亡次數
        // 機率獲得道具

        CreateBattleEnemy();
    }

    public void Hit(string hit)
    {
        if (active == false)
            return;

        if (m_enemySpawn)
            m_enemySpawn.Hit();
    }
}
