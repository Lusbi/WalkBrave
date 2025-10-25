using System;
using GameCore.Database;
using GameCore.Event;
using GameCore.Log;
using UnityEngine;
using UnityEngine.UI;
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

    private TomatoManager m_tomatoManager;

    private EventListener m_eventListener;
    private RoleData m_curBattleRole;
    private bool m_isAlive = false;
    private int m_tomatoHitStack = 0; // ���X�������ֿn���ˮ`����
    private BigNumber m_curHitValue = 0;  // ��e�I���ˮ`
    private RoleTalkHandler m_roleTalkHandler;
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

        m_roleTalkHandler = new RoleTalkHandler();
    }

    /// <summary>
    /// ��s�����ƭ�
    /// ���﨤��ɧ�s
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
            eLog.Log($"�]�w��e�̫�԰������ơG{data.key} �A�Ѿl���ơG{data.HitCount}");
            StorageManager.instance.StorageData.ApplyLastBattleStorageData(data.key, data.HitCount);
            HitValueUpdate();
            m_curBattleRole = data;
            m_enemySpawn.LoadRoleData(data);
            UpdateSceneBackground(data.SceneReference.Load());
            m_roleTalkHandler.Apply(data);
        }
    }

    private void UpdateSceneBackground(ScenemapData scenemapData)
    {
        m_bgImage.SetImage(scenemapData.ImgBgReference).AutoEnable();
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
            SettingsManager.instance.setting.defaultEnemyReference.TryLoad(out m_curBattleRole);
            if (m_curBattleRole == null)
            {
                eLog.Error("���]�w�w�]�԰�����A�Э��s�ˬd�]�w�ɡC");
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

    // ���s���w�ͦ��ĤH
    private void CreateBattleEnemy()
    {
        uiGameMainView.ApplyRoleRect();
        OnSetBattleRoleEvent(new SetBattleRoleEvent(m_curBattleRole.key));
        PlayAnimation(ANI_ENEMYSPAWN, () => m_isAlive = true);
    }

    private void DieAction(bool isTomatoModel = false)
    {
        m_isAlive = false;
        // ��s���⦺�`����
        StorageManager.instance.StorageData.AddEnemyKillCount(m_curBattleRole.key , isTomatoModel);
        // ��s�X��
        if (m_curBattleRole.KillToAddFlagReference != null)
        {
            StorageManager.instance.StorageData.AddFlagStorageValue(m_curBattleRole.KillToAddFlagReference?.GetKey());
        }

        TalkManager.instance.Clear();
        EnemyDieAnimation(CreateBattleEnemy);
    }

    public void Hit(string hit)
    {
        if (active == false)
            return;
        // �ͦ��t�X�����B�z
        if (m_isAlive == false)
            return;

        if (m_tomatoManager.isTomatoTime)
        {
            // �x�W��q
            m_tomatoHitStack ++;
            PlayHit("?");
            // �ݭn�@���x�W��q����{
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

        if (m_roleTalkHandler != null)
            m_roleTalkHandler.Tick(deltaTime);
    }

    /// <summary>
    /// ����ˮ`�ӷ��O�_�� TomatoTime
    /// </summary>
    /// <param name="hitValue"></param>
    /// <param name="isFromTomatoTime"></param>
    public void PlayHit(string hitValue)
    {
        if (m_hitPanel)
            m_hitPanel.PlayHit(hitValue);

    }
}
