using System.Collections.Generic;
using GameCore.Database;
using SuperScrollView;
using UnityEngine;

public class RoleScrollRect : MonoBehaviour
{
    [SerializeField] private LoopListView2 m_loopListview2;
    private DataSourceMgr<RoleInfo> m_dataSourceMgr;
    private List<RoleInfo> m_filteredDataList = null;

    public void Start()
    {
        var datas = Database<RoleData>.GetAll();
        m_dataSourceMgr = new DataSourceMgr<RoleInfo>(datas.Count);
        m_filteredDataList = new List<RoleInfo>();
        foreach (var roleData in datas)
        {
            RoleInfo enemyInfo = new RoleInfo();
            enemyInfo.roleData = roleData;
            m_filteredDataList.Add(enemyInfo);
        }
        m_filteredDataList.Sort((x, y) =>
        {
            return x.roleData.roleSortId.CompareTo(y.roleData.roleSortId);
        });
        m_loopListview2.InitListView(m_dataSourceMgr.TotalItemCount, OnGetItemByIndex);
    }

    private LoopListViewItem2 OnGetItemByIndex(LoopListView2 view, int index)
    {
        LoopListViewItem2 item = view.NewListViewItem("RoleItemHUD");
        if (item == null)
        {
            return null;
        }
        
        RoleItemHUD roleItemHUD = item.GetComponent<RoleItemHUD>();
        if (item.IsInitHandlerCalled == false)
        {
            item.IsInitHandlerCalled = true;
            roleItemHUD.Initlization();
        }

        roleItemHUD.Apply(m_filteredDataList[index]);
        return item;
    }

    /// <summary>
    /// 檢查需要顯示列表的敵人項目
    /// </summary>
    public void CheckRoleValid()
    {

    }

    /// <summary>
    /// 更新列表裡的項目
    /// </summary>
    internal void ListUpdate()
    {
        m_loopListview2.RefreshAllShownItem();
    }
}
