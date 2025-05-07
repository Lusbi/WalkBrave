using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GameCore.Database;

public class StageEnemyInfo : MonoBehaviour
{
    [SerializeField] private Image m_enemyImage; // 圖片
    [SerializeField] private TextMeshProUGUI m_dropRateText; // 掉落率
    [SerializeField] private Image m_dropItemImage; // 掉落物圖片

    // API to set enemy information
    public void SetEnemyInfo(RoleData roleData)
    {
        if (roleData == null) return;

        m_enemyImage.sprite = roleData.EnemyIcon;
        m_dropRateText.text = $"{roleData.DropInfo.dropRate}%"; // 使用 DropInfo 的 dropRate
        m_dropItemImage.sprite = roleData.DropInfo.itemReference?.Load().itemIcon; // 使用 DropInfo 的 itemReference
    }
}
