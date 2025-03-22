// WARNING: Generated file. Do not modify!
using UnityEditor;

namespace GameCore.Database.Editor
{
    public partial class StoryEditorWindow : DataEditorWindow<StoryData>
    {
        [MenuItem("Database/Database/Story", priority = 0)]
        private static void OpenWindow()
        {
            StoryEditorWindow window = GetWindow<StoryEditorWindow>("Story");
            window.Show();
            window.Focus();
        }

        [MenuItem("Database/Database/Story", true)]
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

            StoryEditorWindow window = GetWindow<StoryEditorWindow>("Story");
            window.Show();
            window.Focus();
            window.SetKey(key);
        }
    }
}