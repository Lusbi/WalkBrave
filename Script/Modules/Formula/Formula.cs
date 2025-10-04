using System;
using System.Collections.Generic;
using System.Numerics;
using BigMath;
using GameCore.Database;
using GameCore.Log;
using UnityEngine;

public static class Formula
{
    private static GameDefaultSetting gameDefaultSetting;

    private static void Initlization()
    {
        if (gameDefaultSetting)
            return;

        gameDefaultSetting = SettingsManager.instance.setting;
    }

    /// <summary>
    /// 基本傷害 + 累計加成 * (1 + 蕃茄鐘加成)
    /// </summary>
    /// <returns></returns>
    public static BigNumber GetDefaultDamageValue()
    {
        Initlization();

        BigNumber bigNumber = new BigNumber();
        bigNumber = gameDefaultSetting.defaultClickDamage;
        BigNumber killBonus = 0;
        BigNumber tomatoBonus = 0;
        IReadOnlyList<EnemyStorageData> enemyStorageDatas = StorageManager.instance.StorageData.GetEnemyStorageDatas();
        foreach (var data in enemyStorageDatas)
        {
            if (Database<RoleData>.TryLoad(data.EnemyKey, out var roleData) == false)
            {
                continue;
            }
            if (data.silverClear)
            {
                killBonus += roleData.KillBonus;
            }
            if (data.goldenClear)
            {
                tomatoBonus += roleData.TomatoBonus;
            }
        }

        return bigNumber + (killBonus * (1+tomatoBonus));
    }
}
