using GameCore.Database;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "GameDefaultSetting", menuName = "Scriptable Objects/GameDefaultSetting")]
public class GameDefaultSetting : ScriptableObject
{
    [LabelText("預設關卡")] public ScenemapReference defaultScenemapReference;
    [LabelText("單擊傷害")] public int defaultClickDamage = 1;
    [LabelText("預設敵人")] public RoleReference defaultEnemyReference;

    [LabelText("單擊傷害")] public int debugDamageRate = 1;
}
