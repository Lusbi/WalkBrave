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
    // 用於顯示場景名稱的 TextMeshProUGUI
    [SerializeField] private TextMeshProUGUI m_sceneNameText;

    // 用於顯示場景圖片的 Image
    [SerializeField] private Image m_sceneImage;

    // 左箭頭的 GameCore UIButton
    [SerializeField] private UIButton m_leftArrowButton;

    // 右箭頭的 GameCore UIButton
    [SerializeField] private UIButton m_rightArrowButton;

    // 關閉該面板的 GameCore UIButton
    [SerializeField] private UIButton m_exitButton;

    // 確認進入該關卡的 GameCore UIButton
    [SerializeField] private UIButton m_enterButton;

    // 用於存放關卡敵人資訊的陣列（6 個條目）
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

    // 註冊按鈕的點擊事件
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
        // 替換當前場景資料，關閉視窗
        Active(false);
    }

    private void OnExitButtonClick(UIButton button)
    {
        // 關閉視窗
        Active(false);
    }

    private void OnRightArrowClick(UIButton button)
    {
        // 切換 index 到下一個關卡
        SetCurrentIndex(m_currentIndex + 1);
    }

    private void OnLeftArrowClick(UIButton button)
    {
        // 切換 index 到下一個關卡
        SetCurrentIndex(m_currentIndex - 1);
    }
    #endregion // Button Action
    // 讀取所有關卡資料
    private void LoadAllScenemapDatas()
    {
        if (m_scenemapKeys.Count > 0) 
            return;
        // 獲取所有關卡資料的鍵
        var scenemapdatas = Database<ScenemapData>.LoadAll();

        // 使用 LINQ 取出所有關卡的名稱
        m_scenemapKeys = scenemapdatas.Select(data => data.ScenemapName).ToList();

        // 如果沒有關卡資料，則返回
        if (m_scenemapKeys.Count == 0) 
            return;

        // 設置當前索引為 0
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

        // 設置場景名稱
        m_sceneNameText.text = string.Empty ;
        // 設置場景圖片
        m_sceneImage.sprite = null;
        m_sceneImage.enabled = false;
    }

    private void StageUpdate()
    {
        SetSceneInfo(string.Empty);
        SetEnemyInfo();
        SetDefaultIndex();
    }

    // 設置場景名稱和圖片
    private void SetSceneInfo(string sceneName)
    {
        if (Database<ScenemapData>.TryLoad(sceneName, out m_scenemapData))
        {
            // 設置場景名稱
            m_sceneNameText.text = m_scenemapData.ScenemapName;
            // 設置場景圖片
            m_sceneImage.sprite = m_scenemapData.LevelSelectionThumbnail;
            m_sceneImage.enabled = true;
        }
    }

    // 設置敵人資訊
    private void SetEnemyInfo()
    {
        // 獲取當前關卡的敵人資料
        IReadOnlyList<RoleData> enemyDatas = m_scenemapData.enemies;
        // 遍歷敵人資料並設置到 UI 上
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

    // 設置默認索引
    private void SetDefaultIndex()
    {
        // 取得對應的索引值
        m_currentIndex = m_scenemapKeys.IndexOf(m_scenemapData.key);

        // 設置左箭頭和右箭頭的可見性
        m_leftArrowButton.gameObject.SetActive(m_currentIndex > 0);
        m_rightArrowButton.gameObject.SetActive(m_currentIndex < m_scenemapKeys.Count - 1);
    }

    // 新增一個 API 用於設定當前的索引值  
    public void SetCurrentIndex(int index)
    {
        // 確保索引值在有效範圍內  
        if (index < 0 || index >= m_scenemapKeys.Count)
        {
            Debug.LogWarning("索引值超出範圍");
            return;
        }

        // 更新當前索引  
        m_currentIndex = index;

        // 更新場景資訊  
        StageUpdate();
    }


}