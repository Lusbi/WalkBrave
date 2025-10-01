using System;
using GameCore;
using GameCore.Database;
using GameCore.Event;
using GameCore.Log;
using GameCore.Resources;
using UnityEngine;

public class TomatoManager : MonoSingleton<TomatoManager>, IInitlization
{
    private const string SETTING_ADDRESSABLE_KEY = "TomatoSetting";

    // PlayerPrefs key：讓你切換介面/重載物件時仍能還原
    private const string PP_START_UTC_TICKS = "Tomato.StartUtcTicks";
    private const string PP_DURATION_SEC = "Tomato.DurationSec";

    // 狀態
    public bool isTomatoTime { get; private set; }
    public float curTomatoTime { get; private set; } // 剩餘秒數（給 UI 端讀）
    public TomatoSetting setting { get; private set; }

    // 內部時間參數
    private DateTime _startUtc;   // 觸發當下時間（UTC）
    private float _durationSec;   // 總秒數
    private bool _notified;       // 是否已拋出「時間到」事件

    #region Public: 格式化（你原本就有）
    public string curTomatoTimeString
    {
        get
        {
            if (curTomatoTime <= 0) return string.Empty;
            int minutes = Mathf.FloorToInt(curTomatoTime / 60);
            int sec = Mathf.FloorToInt(curTomatoTime % 60);
            return string.Format("{0:00}:{1:00}", minutes, sec);
        }
    }
    #endregion

    public void Initlization(Action callBack = null)
    {
        CoreResoucesService.LoadAssetAsync<TomatoSetting>(SETTING_ADDRESSABLE_KEY, s =>
        {
            this.setting = s;

            // 如果先前有正在跑的番茄，還原它（切場景/重啟物件時）
            TryRestoreRunningClock();

            callBack?.Invoke();
        });
    }

    /// <summary>
    /// 由外部傳入角色資料決定番茄時長（分鐘→秒）
    /// </summary>
    public void Apply(RoleData roleData)
    {
        if (roleData)
        {
            float minute = setting.GetMinute(roleData.key);
            _durationSec = minute * 60f;
            curTomatoTime = _durationSec; // UI 初值
        }
        else
        {
            _durationSec = 0f;
            curTomatoTime = 0f;
        }
    }

    /// <summary>
    /// 由外部呼叫啟動（例如 12:00 Moment）
    /// </summary>
    public void TomatoTrigger()
    {
        if (isTomatoTime)
        {
            return;
        }

        _startUtc = DateTime.UtcNow;                   // 1) 觸發當下時間（UTC）
        isTomatoTime = true;
        _notified = false;

        SaveRunningClock();                            // 持久化（介面切換/重載可還原）
        CancelInvoke(nameof(Tick));
        InvokeRepeating(nameof(Tick), 0f, 0.25f);      // 2) 背景持續檢查（每 0.25s）

        // 你原本的 Trigger 事件
        EventManager.instance.Notify(new TomatoTriggerEvent(true));
    }

    /// <summary>
    /// 主要檢查邏輯：以牆鐘差計算剩餘秒數，到時丟事件（由你播放音效）
    /// </summary>
    private void Tick()
    {
        // 以 UTC 較不受時區/夏令時影響
        var endUtc = _startUtc.AddSeconds(_durationSec);
        var remain = (float)(endUtc - DateTime.UtcNow).TotalSeconds;

        if (remain < 0f) remain = 0f;

        curTomatoTime = remain; // 提供 UI 端讀取（你說會在別處處理）

        if (remain <= 0f && !_notified)
        {
            _notified = true;
            isTomatoTime = false;
            CancelInvoke(nameof(Tick));
            ClearRunningClock();

            // 到時事件（你在別處接這個事件去播音效/處理 UI）
            // EventManager.instance.Notify(new TomatoTimeoutEvent());
            eLog.Error("蕃茄鐘結束。");
            EventManager.instance.Notify(new TomatoTriggerEvent(false));
        }
    }

    #region 還原/持久化（切回介面時可立即得到正確剩餘）
    private void TryRestoreRunningClock()
    {
        if (!PlayerPrefs.HasKey(PP_START_UTC_TICKS) || !PlayerPrefs.HasKey(PP_DURATION_SEC))
            return;

        var ticksStr = PlayerPrefs.GetString(PP_START_UTC_TICKS);
        if (!long.TryParse(ticksStr, out var ticks))
        {
            ClearRunningClock();
            return;
        }

        _startUtc = new DateTime(ticks, DateTimeKind.Utc);
        _durationSec = PlayerPrefs.GetFloat(PP_DURATION_SEC, 0f);

        // 立刻算一次剩餘並決定是否繼續 Tick
        var endUtc = _startUtc.AddSeconds(_durationSec);
        var remain = (float)(endUtc - DateTime.UtcNow).TotalSeconds;
        if (remain <= 0f)
        {
            curTomatoTime = 0f;
            isTomatoTime = false;
            _notified = true;
            ClearRunningClock();
        }
        else
        {
            curTomatoTime = remain;
            isTomatoTime = true;
            _notified = false;
            CancelInvoke(nameof(Tick));
            InvokeRepeating(nameof(Tick), 0f, 0.25f);
        }
    }

    private void SaveRunningClock()
    {
        PlayerPrefs.SetString(PP_START_UTC_TICKS, _startUtc.Ticks.ToString());
        PlayerPrefs.SetFloat(PP_DURATION_SEC, _durationSec);
        PlayerPrefs.Save();
    }

    private void ClearRunningClock()
    {
        PlayerPrefs.DeleteKey(PP_START_UTC_TICKS);
        PlayerPrefs.DeleteKey(PP_DURATION_SEC);
        PlayerPrefs.Save();
    }
    #endregion

    #region（可選）切回前景時立即刷新一次剩餘值
    private void OnApplicationFocus(bool focus)
    {
        if (focus && isTomatoTime) Tick();
    }
    private void OnApplicationPause(bool paused)
    {
        if (!paused && isTomatoTime) Tick();
    }
    #endregion
}
