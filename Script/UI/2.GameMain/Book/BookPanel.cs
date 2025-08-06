using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Database;
using GameCore.Log;
using GameCore.UI;
using TMPro;
using UnityEngine;

public class BookPanel : PanelBase
{
    [SerializeField] private TextMeshProUGUI m_textStageTitle;
    [SerializeField] private UIButton m_btnArrow_L;
    [SerializeField] private UIButton m_btnArrow_R;
    [SerializeField] private UIButton m_btnExit;
    [SerializeField] private BookItemHud[] m_bookHuds;
    [SerializeField] private BookToyHUD[] m_toyHuds;

    private int m_currentIndex;

    private List<string> m_scenemapKeys = new List<string>();

    private void Reset()
    {
        m_bookHuds = transform.GetComponentsInChildren<BookItemHud>();
        m_toyHuds = transform.GetComponentsInChildren<BookToyHUD>();
    }

    public override void Initlization(Action callBack = null)
    {
        AddListenerButtonAction();
    }

    public override void ActiveOn()
    {
        LoadAllScenemapDatas();
        string _curSceneMapKey = StorageManager.instance.StorageData.CurrentSceneMap;
        if (string.IsNullOrEmpty(_curSceneMapKey))
        {
            _curSceneMapKey = "冒險入口";
        }

        // 取得對應的索引值
        m_currentIndex = m_scenemapKeys.IndexOf(_curSceneMapKey);
        StageUpdate();

        base.ActiveOn();
    }

    private void AddListenerButtonAction()
    {
        m_btnArrow_L.onClicked = OnLeftArrowClick;
        m_btnArrow_R.onClicked = OnRightArrowClick;
        m_btnExit.onClicked = OnExitButtonClick;
    }

    private void StageUpdate()
    {
        LoadStageData(m_scenemapKeys[m_currentIndex]);
        UpdateArrow();
    }

    // 讀取所有關卡資料
    private void LoadAllScenemapDatas()
    {
        if (m_scenemapKeys.Count > 0)
            return;
        // 獲取所有關卡資料的鍵
        var scenemapdatas = Database<ScenemapData>.GetAll();

        // 使用 LINQ 取出所有關卡的名稱，並按 scenemapNo 排序
        m_scenemapKeys = scenemapdatas
            .OrderBy(data => data.SceneMapNo)
            .Select(data => data.key)
            .ToList();

        string log = string.Empty;
        foreach (var key in m_scenemapKeys)
        {
            if (Database<ScenemapData>.TryLoad(key, out var data))
            {
                log += $"{data.SceneMapNo}:{data.key}";
            }
        }
        eLog.Log(log);
    }

    void ResetUI()
    {
        m_textStageTitle.text = string.Empty;
        m_btnArrow_L.gameObject.SetActive(false);
        m_btnArrow_R.gameObject.SetActive(false);
        m_btnExit.gameObject.SetActive(true);
        foreach (var hud in m_bookHuds)
        {
            hud.ApplyFromRole(null);
        }
        foreach (var hud in m_toyHuds)
        {
            hud.ApplyToy(string.Empty);
        }
    }

    void LoadStageData(string scenemapKey)
    {
        ResetUI();
        Database<ScenemapData>.TryLoad(scenemapKey, out var scenemapData);
        if (scenemapData == null)
        {
            m_textStageTitle.text = "Unknown Stage";
            return;
        }
        m_textStageTitle.text = scenemapData.ScenemapName;
        var enemies = scenemapData.enemies;
        int index = 0;
        int enemyCount = enemies != null ? enemies.Count : 0;
        foreach (var hud in m_bookHuds)
        {
            if (index < enemyCount)
            {
                hud.ApplyFromRole(enemies[index]);
            }
            else
            {
                hud.ApplyFromRole(null);
            }
            index++;
        }
        index = 0;
        // 設定這個場景可以取得到的玩具
        // 從 ToyData 中取得所有玩具
        var toyDatas = Database<ToyData>.GetAll();
        foreach (var toyData in toyDatas)
        {
            if (toyData.scenemapReference.GetKey() == scenemapKey)
            {
                m_toyHuds[index].ApplyToy(toyData.key);
                index++;
            }
        }
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

    private void OnExitButtonClick(UIButton button)
    {
        // 關閉視窗
        Active(false);
    }

    void UpdateArrow()
    {
        // 設置左箭頭和右箭頭的可見性
        m_btnArrow_L.gameObject.SetActive(m_currentIndex > 0);
        m_btnArrow_R.gameObject.SetActive(m_currentIndex < m_scenemapKeys.Count - 1);
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
