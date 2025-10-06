using System;
using System.Collections;
using System.Collections.Generic;
using GameCore.Database;
using GameCore.Event;
using GameCore.Log;
using UnityEngine;
using UnityEngine.UI;
using GameCore.Utils;
using NUnit.Framework;
using Cinemachine;
using TreeEditor;
using BigMath;

public class BattlePanel : PanelBase
{
    private const string ANI_ENEMYSPAWN = "SpawnEnemyAnim";

    [SerializeField] private RectTransform m_enemyParent;

    [SerializeField] private Image m_bgImage;

    [SerializeField] private RoleSpawnHUD m_enemySpawn;

    [SerializeField] private Animation m_ani;

    [SerializeField] private TomatoButton m_tomatoBtn;

    [SerializeField] private HitPanel m_hitPanel;

    private Cinemachine.CinemachineVirtualCamera m_cinemachineVirtualCamera;

    private TomatoManager m_tomatoManager;
    private CinemachineBasicMultiChannelPerlin m_perlin;

    private EventListener m_eventListener;
    private RoleData m_curBattleRole;
    private bool m_isAlive = false;
    // ĤHܽ{
    private Coroutine m_enemyTalkCoroutine;
    // ƧQΪ֨MAΩziΪܤe
    private readonly List<TalkScriptableObjectName> m_talkCandidates = new List<TalkScriptableObjectName>();

    private int m_tomatoHitStack = 0; // 蕃茄鐘期間累積的傷害次數
    private BigNumber m_curHitValue = 0;  // 當前點擊傷害
    public override void Initlization(Action callBack = null)
    {
        base.Initlization(callBack);

        if (m_enemySpawn)
        {
            m_enemySpawn.Initlization();
            m_enemySpawn.ApplyDieAction(DieAction);
        }

        if (m_eventListener == null)
        {
            m_eventListener = new EventListener();
            m_eventListener.active = true;
            m_eventListener.AddListener<SetBattleRoleEvent>(OnSetBattleRoleEvent, true);
            m_eventListener.AddListener<TomatoTriggerEvent>(OnTomatoTriggerEvent, true);
        }

        if (m_tomatoBtn)
            m_tomatoBtn.Initlization();

        m_tomatoManager ??= TomatoManager.instance;

        if (m_hitPanel)
            m_hitPanel.Initlization();
    }

    /// <summary>
    /// 更新擊打數值
    /// 重選角色時更新
    /// </summary>
    private void HitValueUpdate()
    {
        m_curHitValue = Formula.GetDefaultDamageValue();
        // Debug Bouns
        m_curHitValue *= SettingsManager.instance.setting.debugDamageRate;
    }

    private void OnTomatoTriggerEvent(TomatoTriggerEvent eventData)
    {
        if (m_tomatoBtn)
            m_tomatoBtn.SetTrigger(eventData.enable);

        if (eventData.enable)
        {
            if (Database<RoleData>.TryLoad(StorageManager.instance.StorageData.BattleStorageData.EnemyKey, out var roleData))
                m_tomatoManager.Apply(roleData);
        }
        else
        {
            m_tomatoManager.Apply(null);

            BigNumber value = 0;
            for (int i = 0; i < m_tomatoHitStack; i++)
            {
                value += m_curHitValue;
            }

            if (m_enemySpawn)
                m_enemySpawn.Hit(value , true);

            PlayHit(value.ToString());
        }
        m_tomatoHitStack = 0;
    }

    private void OnSetBattleRoleEvent(SetBattleRoleEvent eventData)
    {
        if (Database<RoleData>.TryLoad(eventData.roleKey, out var data))
        {
            StartEnemyTalkRoutine(); // sҰʼĤHܬy{
        }
        else
        {
            StopEnemyTalkRoutine();
            if (Database<RoleData>.TryLoad(StorageManager.instance.StorageData.BattleStorageData.EnemyKey, out var roleData))
            {
                m_curBattleRole = roleData;
                UpdateSceneBackground(roleData.SceneReference.Load());
                StartEnemyTalkRoutine(); // qxs٭ɤ]nܼĤH
            }
            else
            {
                StopEnemyTalkRoutine();
            }
        StopEnemyTalkRoutine(); // OɰĤH
        StopEnemyTalkRoutine(); // ĤH`ɰܽ
    // ҰʼĤHܪy{A|̾ڳ]wƽ
    private void StartEnemyTalkRoutine()
    {
        StopEnemyTalkRoutine();

        if (m_enemySpawn == null)
        {
            return;
        }

        if (m_curBattleRole == null || m_curBattleRole.TalkScriptableObject == null)
        {
            m_enemySpawn.SetTalkContent(string.Empty);
            return;
        }

        var talkAsset = m_curBattleRole.TalkScriptableObject;
        if (talkAsset.Entries == null || talkAsset.Entries.Count == 0)
        {
            m_enemySpawn.SetTalkContent(string.Empty);
            return;
        }

        // ϥΨ{̧H
        m_enemyTalkCoroutine = StartCoroutine(EnemyTalkRoutine(talkAsset));
    }

    // ĤHܨòMܤr
    private void StopEnemyTalkRoutine()
    {
        if (m_enemyTalkCoroutine != null)
        {
            StopCoroutine(m_enemyTalkCoroutine);
            m_enemyTalkCoroutine = null;
        }

        if (m_enemySpawn != null)
        {
            m_enemySpawn.SetTalkContent(string.Empty);
        }
    }

    // ĤHܨ{G̾ TalkScriptableObject ƽe
    private IEnumerator EnemyTalkRoutine(TalkScriptableObject talkAsset)
    {
        while (true)
        {
            if (talkAsset == null || m_curBattleRole == null || m_curBattleRole.TalkScriptableObject != talkAsset)
            {
                m_enemySpawn?.SetTalkContent(string.Empty);
                yield break;
            }

            if (!active || m_isAlive == false)
            {
                m_enemySpawn?.SetTalkContent(string.Empty);
                yield return null;
                continue;
            }

            if (!TryGetRandomTalkEntry(talkAsset, out var talkEntry))
            {
                m_enemySpawn?.SetTalkContent(string.Empty);
                yield return null;
                continue;
            }

            string localizedContent = GetLocalizedTalkContent(talkEntry.Content);
            m_enemySpawn?.SetTalkContent(localizedContent);

            float duration = Mathf.Max(0.5f, talkEntry.Duration);
            float elapsed = 0f;
            while (elapsed < duration)
            {
                if (!active || m_isAlive == false || m_curBattleRole == null || m_curBattleRole.TalkScriptableObject != talkAsset)
                {
                    break;
                }

                elapsed += Time.deltaTime;
                yield return null;
            }

            if (!active || m_isAlive == false)
            {
                m_enemySpawn?.SetTalkContent(string.Empty);
            }

            yield return null;
        }
    }

    // ըoŦXHܱ
    private bool TryGetRandomTalkEntry(TalkScriptableObject talkAsset, out TalkScriptableObjectName talkEntry)
    {
        var entries = talkAsset.Entries;
        m_talkCandidates.Clear();

        if (entries != null)
        {
            for (int i = 0; i < entries.Count; i++)
            {
                var entry = entries[i];
                if (IsTalkEntryValid(entry))
                {
                    m_talkCandidates.Add(entry);
                }
            }
        }

        if (m_talkCandidates.Count == 0)
        {
            talkEntry = default;
            return false;
        }

        talkEntry = m_talkCandidates[UnityEngine.Random.Range(0, m_talkCandidates.Count)];
        return true;
    }

    // ˬdܱO_
    private bool IsTalkEntryValid(TalkScriptableObjectName entry)
    {
        var conditions = entry.Conditions;
        if (conditions == null || conditions.Count == 0)
        {
            return true;
        }

        for (int i = 0; i < conditions.Count; i++)
        {
            var condition = conditions[i];

            if (condition.RemainingCount > 0 && m_enemySpawn != null)
            {
                if (m_enemySpawn.CurrentRemainHit > condition.RemainingCount)
                {
                    return false;
                }
            }

            var flagReference = condition.FlagReference;
            if (flagReference != null)
            {
                if (flagReference.TryLoad(out var flagData))
                {
                    var storageData = StorageManager.instance?.StorageData;
                    if (storageData == null)
                    {
                        return false;
                    }

                    if (storageData.GetFlagStorageValue(flagData.key) <= 0)
                    {
                        return false;
                    }
                }
                else
                {
                    eLog.Error($"䤣XиơG{flagReference.GetKey()}");
                    return false;
                }
            }
        }

        return true;
    }

    // Nhyt key নܤr
    private string GetLocalizedTalkContent(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            return string.Empty;
        }

        return LocalizationManager.instance != null ? LocalizationManager.instance.GetLocalization(key) : key;
    }

            eLog.Log($"設定當前最後戰鬥角色資料：{data.key} ，剩餘次數：{data.HitCount}");
            StorageManager.instance.StorageData.ApplyLastBattleStorageData(data.key, data.HitCount);
            HitValueUpdate();
            m_curBattleRole = data;
            m_enemySpawn.LoadRoleData(data);
            UpdateSceneBackground(data.SceneReference.Load());
        }
    }

    private void UpdateSceneBackground(ScenemapData scenemapData)
    {
        m_bgImage.SetImage(scenemapData.ImgBgReference).AutoEnable();
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
            SettingsManager.instance.setting.defaultEnemyReference.TryLoad(out m_curBattleRole);
            if (m_curBattleRole == null)
            {
                eLog.Error("未設定預設戰鬥角色，請重新檢查設定檔。");
                return;
            }
            CreateBattleEnemy();
        }

        BattleManager.instance.Register(Hit);
    }

    public override void ActiveOff()
    {
        BattleManager.instance.Register(null);
    }

    // 重新指定生成敵人
    private void CreateBattleEnemy()
    {
        uiGameMainView.ApplyRoleRect();
        OnSetBattleRoleEvent(new SetBattleRoleEvent(m_curBattleRole.key));
        PlayAnimation(ANI_ENEMYSPAWN, () => m_isAlive = true);
    }

    private void DieAction(bool isTomatoModel = false)
    {
        m_isAlive = false;
        // 更新角色死亡次數
        StorageManager.instance.StorageData.AddEnemyKillCount(m_curBattleRole.key , isTomatoModel);
        // 更新旗標
        if (m_curBattleRole.KillToAddFlagReference != null)
        {
            StorageManager.instance.StorageData.AddFlagStorageValue(m_curBattleRole.KillToAddFlagReference?.GetKey());
        }

        EnemyDieAnimation(CreateBattleEnemy);
    }

    public void Hit(string hit)
    {
        if (active == false)
            return;
        // 生成演出中不處理
        if (m_isAlive == false)
            return;

        if (m_tomatoManager.isTomatoTime)
        {
            // 儲蓄能量
            m_tomatoHitStack ++;
            PlayHit("?");
            // 需要一個儲蓄能量的表現
            return;
        }

        if (m_enemySpawn)
        {
            m_enemySpawn.Hit(m_curHitValue);
            //PlayHit(UnityEngine.Random.Range(1,10).ToString());
            PlayHit(m_curHitValue.ToString());
        }
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

    public override void Tick(float deltaTime)
    {
        if (m_tomatoBtn)
            m_tomatoBtn.ApplyTomatoTime();
    }

    /// <summary>
    /// 播放傷害來源是否為 TomatoTime
    /// </summary>
    /// <param name="hitValue"></param>
    /// <param name="isFromTomatoTime"></param>
    public void PlayHit(string hitValue)
    {
        if (m_hitPanel)
            m_hitPanel.PlayHit(hitValue);

    }
}
