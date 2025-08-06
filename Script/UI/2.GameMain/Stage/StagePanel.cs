using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GameCore.Database;
using GameCore.Log;
using GameCore.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StagePanel : PanelBase
{
    // �Ω���ܳ����W�٪� TextMeshProUGUI
    [SerializeField] private TextMeshProUGUI m_sceneNameText;

    // �Ω���ܳ����Ϥ��� Image
    [SerializeField] private Image m_sceneImage;

    // ���b�Y�� GameCore UIButton
    [SerializeField] private UIButton m_leftArrowButton;

    // �k�b�Y�� GameCore UIButton
    [SerializeField] private UIButton m_rightArrowButton;

    // �����ӭ��O�� GameCore UIButton
    [SerializeField] private UIButton m_exitButton;

    // �T�{�i�J�����d�� GameCore UIButton
    [SerializeField] private UIButton m_enterButton;

    // �Ω�s�����d�ĤH��T���}�C�]6 �ӱ��ء^
    [SerializeField] private StageEnemyInfo[] m_stageEnemies = new StageEnemyInfo[6];

    private List<string> m_scenemapKeys = new List<string>();
    private int m_currentIndex;
    private ScenemapData m_scenemapData;

    public override void Initlization(Action callBack = null)
    {
        base.Initlization();
        AddListenerButtonAction();
    }

    public override void Active(bool isActive)
    {
        base.Active(isActive);

        canvasGroup.blocksRaycasts = isActive;
    }

    // ���U���s���I���ƥ�
    private void AddListenerButtonAction()
    {
        m_leftArrowButton.onClicked = OnLeftArrowClick;
        m_rightArrowButton.onClicked = OnRightArrowClick;
        m_exitButton.onClicked = OnExitButtonClick;
        m_enterButton.onClicked = OnEnterButtonClick;
    }

    private void OnEnterButtonClick(UIButton button)
    {
        // ������e������ơA��������
        Active(false);

        StorageManager.instance.StorageData.SetCurrentSceneMap(m_scenemapKeys[m_currentIndex]);
        uiGameMainView.ActionCommand(CommandType.Battle);
    }

    private void OnExitButtonClick(UIButton button)
    {
        // ��������
        Active(false);
    }

    private void OnRightArrowClick(UIButton button)
    {
        if (m_currentIndex + 1 >= m_scenemapKeys.Count)
        {
            Debug.LogWarning("���ޭȶW�X�d��");
            return;
        }
        // �ˬd�U�@�����d�O�_�i�H���
        string key = m_scenemapKeys[m_currentIndex + 1];
        if (Database<ScenemapData>.TryLoad(key, out ScenemapData data))
        {
            if (data.SceneUnlockValid() == false)
            {
                // ���d�|������
                eLog.Error($"���d�|������G{data.key}");
                return;
            }
        }

        // ���� index ��U�@�����d
        SetCurrentIndex(m_currentIndex + 1);
    }

    private void OnLeftArrowClick(UIButton button)
    {
        if (m_currentIndex - 1 < 0)
        {
            Debug.LogWarning("���ޭȶW�X�d��");
            return;
        }

        string key = m_scenemapKeys[m_currentIndex - 1];
        if (Database<ScenemapData>.TryLoad(key, out ScenemapData data))
        {
            if (data.SceneUnlockValid() == false)
            {
                // ���d�|������
                eLog.Error($"���d�|������G{data.key}");
                return;
            }
        }

        // ���� index ��U�@�����d
        SetCurrentIndex(m_currentIndex - 1);
    }
    // Ū���Ҧ����d���
    private void LoadAllScenemapDatas()
    {
        if (m_scenemapKeys.Count > 0)
            return;
        // ����Ҧ����d��ƪ���
        var scenemapdatas = Database<ScenemapData>.GetAll();

        // �ϥ� LINQ ���X�Ҧ����d���W�١A�ë� scenemapNo �Ƨ�
        m_scenemapKeys = scenemapdatas
            .OrderBy(data => data.SceneMapNo)
            .Select(data => data.key)
            .ToList();

        string log = string.Empty;
        foreach (var key in m_scenemapKeys)
        {
            if (Database<ScenemapData>.TryLoad(key , out var data))
            {
                log += $"{data.SceneMapNo}:{data.key}";
            }
        }
        eLog.Log(log);
    }

    public override void ActiveOn()
    {
        LoadAllScenemapDatas();
        string _curSceneMapKey = StorageManager.instance.StorageData.CurrentSceneMap;
        if (string.IsNullOrEmpty(_curSceneMapKey))
        {
            _curSceneMapKey = "�_�I�J�f";
        }
        // ���o���������ޭ�
        m_currentIndex = m_scenemapKeys.IndexOf(_curSceneMapKey);
        StageUpdate();
    }

    public override void ActiveOff()
    {
        SetSceneInfo(string.Empty);

        // �]�m�����W��
        m_sceneNameText.text = string.Empty;
        // �]�m�����Ϥ�
        m_sceneImage.sprite = null;
        m_sceneImage.enabled = false;
    }

    private void StageUpdate()
    {
        SetSceneInfo(m_scenemapKeys[m_currentIndex]);
        SetEnemyInfo();
        SetDefaultIndex();
    }

    // �]�m�����W�٩M�Ϥ�
    private void SetSceneInfo(string scenemapName)
    {
        m_sceneNameText.text = string.Empty;
        m_sceneImage.sprite = null;
        if (Database<ScenemapData>.TryLoad(scenemapName, out m_scenemapData))
        {
            // �]�m�����W��
            m_sceneNameText.text = LocalizationManager.instance.GetLocalization(m_scenemapData.ScenemapName);
            // �]�m�����Ϥ�
            m_sceneImage.sprite = m_scenemapData.LevelSelectionThumbnail;
        }
        m_sceneImage.enabled = m_sceneImage.sprite;
    }

    // �]�m�ĤH��T
    private void SetEnemyInfo()
    {
        if (m_scenemapData == null)
            return;
        // �����e���d���ĤH���
        IReadOnlyList<RoleData> enemyDatas = m_scenemapData.enemies;
        // �M���ĤH��ƨó]�m�� UI �W
        for (int i = 0; i < m_stageEnemies.Length; i++)
        {
            if (i < enemyDatas.Count)
            {
                m_stageEnemies[i].SetEnemyInfo(enemyDatas[i]);
                m_stageEnemies[i].gameObject.SetActive(true);
            }
            else
            {
                m_stageEnemies[i].gameObject.SetActive(false);
            }
        }
    }

    // �]�m�q�{����
    private void SetDefaultIndex()
    {
        // ���o���������ޭ�
        m_currentIndex = m_scenemapKeys.IndexOf(m_scenemapData.key);

        // �]�m���b�Y�M�k�b�Y���i����
        m_leftArrowButton.gameObject.SetActive(m_currentIndex > 0);
        m_rightArrowButton.gameObject.SetActive(m_currentIndex < m_scenemapKeys.Count - 1);
    }

    // �s�W�@�� API �Ω�]�w��e�����ޭ�  
    public void SetCurrentIndex(int index)
    {
        // �T�O���ޭȦb���Ľd��  
        if (index < 0 || index >= m_scenemapKeys.Count)
        {
            Debug.LogWarning("���ޭȶW�X�d��");
            return;
        }

        // ��s��e����  
        m_currentIndex = index;

        // ��s������T  
        StageUpdate();
    }
}