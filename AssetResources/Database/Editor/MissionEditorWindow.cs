// WARNING: Generated file. Do not modify!
using UnityEditor;

namespace GameCore.Database.Editor
{
    public partial class MissionEditorWindow : DataEditorWindow<MissionData>
    {
        [MenuItem("Database/Database/Mission", priority = 0)]
        private static void OpenWindow()
        {
            MissionEditorWindow window = GetWindow<MissionEditorWindow>("Mission");
            window.Show();
            window.Focus();
        }

        [MenuItem("Database/Database/Mission", true)]
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

            MissionEditorWindow window = GetWindow<MissionEditorWindow>("Mission");
            window.Show();
            window.Focus();
            window.SetKey(key);
        }
    }
}