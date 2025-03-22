// WARNING: Generated file. Do not modify!
using UnityEditor;

namespace GameCore.Database.Editor
{
    public partial class RewardEditorWindow : DataEditorWindow<RewardData>
    {
        [MenuItem("Database/Database/Reward", priority = 0)]
        private static void OpenWindow()
        {
            RewardEditorWindow window = GetWindow<RewardEditorWindow>("Reward");
            window.Show();
            window.Focus();
        }

        [MenuItem("Database/Database/Reward", true)]
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

            RewardEditorWindow window = GetWindow<RewardEditorWindow>("Reward");
            window.Show();
            window.Focus();
            window.SetKey(key);
        }
    }
}