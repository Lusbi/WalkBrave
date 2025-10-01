using GameCore.Database;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "GameDefaultSetting", menuName = "Scriptable Objects/GameDefaultSetting")]
public class GameDefaultSetting : ScriptableObject
{
    [LabelText("�w�]���d")] public ScenemapReference defaultScenemapReference;
    [LabelText("�����ˮ`")] public int defaultClickDamage = 1;
    [LabelText("�w�]�ĤH")] public RoleReference defaultEnemyReference;

    [LabelText("�����ˮ`")] public int debugDamageRate = 1;
}
