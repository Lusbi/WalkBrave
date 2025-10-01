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
    /// 傷害公式： 基本傷害+(累計擊殺加成)^(累計蕃茄鐘加成) 
    /// </summary>
    /// <returns></returns>
    public static BigNumber GetDefaultDamageValue()
    {
        Initlization();

        BigNumber bigNumber = new BigNumber();
        bigNumber = gameDefaultSetting.defaultClickDamage;
        BigNumber killBonus = 0;
        int tomatoBonus = 0;
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

        eLog.Log($"當前傷害：{bigNumber} + ({killBonus} * (1 +{ tomatoBonus})) = {(bigNumber + (killBonus * (1 + tomatoBonus)))}");
        return bigNumber + (killBonus * (1+tomatoBonus));
    }
}
