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

    // PlayerPrefs key�G���A��������/��������ɤ����٭�
    private const string PP_START_UTC_TICKS = "Tomato.StartUtcTicks";
    private const string PP_DURATION_SEC = "Tomato.DurationSec";

    // ���A
    public bool isTomatoTime { get; private set; }
    public float curTomatoTime { get; private set; } // �Ѿl��ơ]�� UI ��Ū�^
    public TomatoSetting setting { get; private set; }

    // �����ɶ��Ѽ�
    private DateTime _startUtc;   // Ĳ�o��U�ɶ��]UTC�^
    private float _durationSec;   // �`���
    private bool _notified;       // �O�_�w�ߥX�u�ɶ���v�ƥ�

    #region Public: �榡�ơ]�A�쥻�N���^
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

            // �p�G���e�����b�]���f�X�A�٭쥦�]������/���Ҫ���ɡ^
            TryRestoreRunningClock();

            callBack?.Invoke();
        });
    }

    /// <summary>
    /// �ѥ~���ǤJ�����ƨM�w�f�X�ɪ��]��������^
    /// </summary>
    public void Apply(RoleData roleData)
    {
        if (roleData)
        {
            float minute = setting.GetMinute(roleData.key);
            _durationSec = minute * 60f;
            curTomatoTime = _durationSec; // UI ���
        }
        else
        {
            _durationSec = 0f;
            curTomatoTime = 0f;
        }
    }

    /// <summary>
    /// �ѥ~���I�s�Ұʡ]�Ҧp 12:00 Moment�^
    /// </summary>
    public void TomatoTrigger()
    {
        if (isTomatoTime)
        {
            return;
        }

        _startUtc = DateTime.UtcNow;                   // 1) Ĳ�o��U�ɶ��]UTC�^
        isTomatoTime = true;
        _notified = false;

        SaveRunningClock();                            // ���[�ơ]��������/�����i�٭�^
        CancelInvoke(nameof(Tick));
        InvokeRepeating(nameof(Tick), 0f, 0.25f);      // 2) �I�������ˬd�]�C 0.25s�^

        // �A�쥻�� Trigger �ƥ�
        EventManager.instance.Notify(new TomatoTriggerEvent(true));
    }

    /// <summary>
    /// �D�n�ˬd�޿�G�H�����t�p��Ѿl��ơA��ɥ�ƥ�]�ѧA���񭵮ġ^
    /// </summary>
    private void Tick()
    {
        // �H UTC �������ɰ�/�L�O�ɼv�T
        var endUtc = _startUtc.AddSeconds(_durationSec);
        var remain = (float)(endUtc - DateTime.UtcNow).TotalSeconds;

        if (remain < 0f) remain = 0f;

        curTomatoTime = remain; // ���� UI ��Ū���]�A���|�b�O�B�B�z�^

        if (remain <= 0f && !_notified)
        {
            _notified = true;
            isTomatoTime = false;
            CancelInvoke(nameof(Tick));
            ClearRunningClock();

            // ��ɨƥ�]�A�b�O�B���o�Өƥ�h������/�B�z UI�^
            // EventManager.instance.Notify(new TomatoTimeoutEvent());
            eLog.Error("���X�������C");
            EventManager.instance.Notify(new TomatoTriggerEvent(false));
        }
    }

    #region �٭�/���[�ơ]���^�����ɥi�ߧY�o�쥿�T�Ѿl�^
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

        // �ߨ��@���Ѿl�èM�w�O�_�~�� Tick
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

    #region�]�i��^���^�e���ɥߧY��s�@���Ѿl��
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
