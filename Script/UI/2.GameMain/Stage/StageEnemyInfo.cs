using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GameCore.Database;

public class StageEnemyInfo : MonoBehaviour
{
    [SerializeField] private Image m_enemyImage; // �Ϥ�
    [SerializeField] private TextMeshProUGUI m_dropRateText; // �����v
    [SerializeField] private Image m_dropItemImage; // �������Ϥ�

    // API to set enemy information
    public void SetEnemyInfo(RoleData roleData)
    {
        if (roleData == null) return;

        m_enemyImage.sprite = roleData.EnemyIcon;
        m_dropRateText.text = $"{roleData.DropInfo.dropRate}%"; // �ϥ� DropInfo �� dropRate
        m_dropItemImage.sprite = roleData.DropInfo.itemReference?.Load().itemIcon; // �ϥ� DropInfo �� itemReference
    }
}
