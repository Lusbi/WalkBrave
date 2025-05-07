using System;
using System.Collections.Generic;
using System.Linq;
using GameCore;
using GameCore.Database;
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

    #region Button Action
    private void OnEnterButtonClick(UIButton button)
    {
        // ������e������ơA��������
        Active(false);
    }

    private void OnExitButtonClick(UIButton button)
    {
        // ��������
        Active(false);
    }

    private void OnRightArrowClick(UIButton button)
    {
        // ���� index ��U�@�����d
        SetCurrentIndex(m_currentIndex + 1);
    }

    private void OnLeftArrowClick(UIButton button)
    {
        // ���� index ��U�@�����d
        SetCurrentIndex(m_currentIndex - 1);
    }
    #endregion // Button Action
    // Ū���Ҧ����d���
    private void LoadAllScenemapDatas()
    {
        if (m_scenemapKeys.Count > 0) 
            return;
        // ����Ҧ����d��ƪ���
        var scenemapdatas = Database<ScenemapData>.LoadAll();

        // �ϥ� LINQ ���X�Ҧ����d���W��
        m_scenemapKeys = scenemapdatas.Select(data => data.ScenemapName).ToList();

        // �p�G�S�����d��ơA�h��^
        if (m_scenemapKeys.Count == 0) 
            return;

        // �]�m��e���ެ� 0
        m_currentIndex = 0;
    }

    public override void ActiveOn()
    {
        StageUpdate();
    }

    public override void ActiveOff()
    {
        SetSceneInfo(string.Empty);
        LoadAllScenemapDatas();

        // �]�m�����W��
        m_sceneNameText.text = string.Empty ;
        // �]�m�����Ϥ�
        m_sceneImage.sprite = null;
        m_sceneImage.enabled = false;
    }

    private void StageUpdate()
    {
        SetSceneInfo(string.Empty);
        SetEnemyInfo();
        SetDefaultIndex();
    }

    // �]�m�����W�٩M�Ϥ�
    private void SetSceneInfo(string sceneName)
    {
        if (Database<ScenemapData>.TryLoad(sceneName, out m_scenemapData))
        {
            // �]�m�����W��
            m_sceneNameText.text = m_scenemapData.ScenemapName;
            // �]�m�����Ϥ�
            m_sceneImage.sprite = m_scenemapData.LevelSelectionThumbnail;
            m_sceneImage.enabled = true;
        }
    }

    // �]�m�ĤH��T
    private void SetEnemyInfo()
    {
        // �����e���d���ĤH���
        IReadOnlyList<RoleData> enemyDatas = m_scenemapData.enemies;
        // �M���ĤH��ƨó]�m�� UI �W
        for (int i = 0; i < m_stageEnemies.Length; i++)
        {
            if (i < enemyDatas.Count)
            {
                m_stageEnemies[i].SetEnemyInfo(enemyDatas[i]);
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