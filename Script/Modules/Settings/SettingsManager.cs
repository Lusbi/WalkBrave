using System;
using GameCore;
using GameCore.Resources;

public class SettingsManager : Singleton<SettingsManager>, IInitlization
{
    private const string SETTING_ADDRESSABLE_KEY = "GameDefaultSetting";

    public GameDefaultSetting gameDefaultSetting { get; private set; }

    public void Initlization(Action callBack = null)
    {
        CoreResoucesService.LoadAssetAsync<GameDefaultSetting>(SETTING_ADDRESSABLE_KEY,
            (setting) => gameDefaultSetting = setting);
    }
}
