using GameCore.Database;
using GameCore.Database.Editor;
using UnityEditor;

public class SpriteUpdate
{
    [MenuItem("Tools/����Ϥ����J")]
    private static void RoleTransfer()
    {
        var roles = DatabaseEditorUtils.GetDatas<RoleData>();
        foreach (var role in roles)
        {
            // �ഫ����Ϥ�
            role.SetSprite();
            DatabaseEditorUtils.SaveData(role);
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    [MenuItem("Tools/�����Ϥ����J")]
    private static void SceneMapTransfer()
    {
        var sceneMaps = DatabaseEditorUtils.GetDatas<ScenemapData>();
        foreach (var sceneMap in sceneMaps)
        {
            // �ഫ����Ϥ�
            sceneMap.SetSprite();
            DatabaseEditorUtils.SaveData(sceneMap);
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
