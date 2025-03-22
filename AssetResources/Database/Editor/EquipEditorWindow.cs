// WARNING: Generated file. Do not modify!
using UnityEditor;

namespace GameCore.Database.Editor
{
    public partial class EquipEditorWindow : DataEditorWindow<EquipData>
    {
        [MenuItem("Database/Database/Equip", priority = 0)]
        private static void OpenWindow()
        {
            EquipEditorWindow window = GetWindow<EquipEditorWindow>("Equip");
            window.Show();
            window.Focus();
        }

        [MenuItem("Database/Database/Equip", true)]
        private static bool OpenWindowValidate()
        {
            return DatabaseUser.HasUserName();
        }

        private static void OpenWindowWithKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }

            EquipEditorWindow window = GetWindow<EquipEditorWindow>("Equip");
            window.Show();
            window.Focus();
            window.SetKey(key);
        }
    }
}