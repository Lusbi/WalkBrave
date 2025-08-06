using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Database;
using GameCore.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToyPanel : PanelBase
{
    [SerializeField] private ToyHud m_toyTemplate;
    [SerializeField] private HorizontalLayoutGroup m_hGroup;

    [SerializeField] private TextMeshProUGUI m_textCombineTip;
    [SerializeField] private UIButton m_btnCombine;
    [SerializeField] private UIButton m_btnUse;
    [SerializeField] private UIButton m_btnExit;

    [SerializeField] private CompositeHud[] m_compositeHuds;

    // 當前選擇的物件 index
    private int m_curSelectIndex;
    // 資料庫所有的玩具資料
    private ToyHud[] m_toyHuds;
    public override void Initlization(Action callBack = null)
    {
        base.Initlization(callBack);

        IReadOnlyList<ToyData> toyDatas = Database<ToyData>.GetAll().ToList();
        // 創建 hud
        m_toyHuds = new ToyHud[toyDatas.Count];
        for (int i = 0; i <  toyDatas.Count; i++)
        {
            ToyHud toyHud = Instantiate(m_toyTemplate, m_hGroup.transform);
            m_toyHuds[i] = toyHud;
            toyHud.gameObject.SetActive(true);
            toyHud.Apply(toyDatas[i].key , i);
            toyHud.ApplyOnClick(OnClicked);
        }

        var hRect = m_hGroup.GetComponent<RectTransform>();
        hRect.sizeDelta = new Vector2(m_toyHuds.Length * 150f , hRect.sizeDelta.y);
        m_curSelectIndex = 0;
        SetIndex(0);

        m_btnExit.onClicked = (btn) => { Active(false); };
    }

    private void SetIndex(int index)
    {
        string curKey = m_toyHuds[index].toyKey;
        Database<ToyData>.TryLoad(curKey, out var toyData);
        if (toyData == null)
        {
            return;
        }
        bool hasStorageData = StorageManager.instance.StorageData.GetToyStorageData(curKey) != null;
        bool canCombine = true;
        toyData.compositeReference.TryLoad(out var compositeData);
        if (compositeData == null)
        {
            canCombine = false;
        }
        else
        {
            // 檢查是否有足夠的玩具來合成
            foreach (var data in compositeData.compositeInfo.CompositeDatas)
            {
                string itemKey = data.itemReference.GetKey();
                var itemStorageData = StorageManager.instance.StorageData.GetItemStorageData(itemKey);
                // 材料不足
                if (itemStorageData == null || itemStorageData.ItemValue < data.amount)
                {
                    canCombine = false;
                    break;
                }
            }
            foreach (var item in m_compositeHuds)
            {
                item.gameObject.SetActive(false);
            }

            for (int i = 0; i < compositeData.compositeInfo.CompositeDatas.Count; i++)
            {
                if (i >= m_compositeHuds.Length)
                {
                    Debug.LogError("CompositeHuds array is not large enough to hold all composite data.");
                    return;
                }
                m_compositeHuds[i].Apply(compositeData.compositeInfo.CompositeDatas[i]);
                m_compositeHuds[i].gameObject.SetActive(true);
            }
        }
        ButtonStateUpdate(hasStorageData, canCombine);
    }

    private void ButtonStateUpdate(bool hasStorageData , bool canCombine)
    {
        m_btnCombine.gameObject.SetActive(!hasStorageData && canCombine);
        m_btnUse.gameObject.SetActive(hasStorageData);
        m_textCombineTip.gameObject.SetActive(!hasStorageData && !canCombine);
    }

    private void OnClicked(UIButton button)
    {
        var toyHud = button.data as ToyHud;
        SetIndex(toyHud.index);
    }
}
