// WARNING: Generated file. Do not modify!
using UnityEditor;

namespace GameCore.Database.Editor
{
    public partial class DialogueEditorWindow : DataEditorWindow<DialogueData>
    {
        [MenuItem("Database/Database/Dialogue", priority = 0)]
        private static void OpenWindow()
        {
            DialogueEditorWindow window = GetWindow<DialogueEditorWindow>("Dialogue");
            window.Show();
            window.Focus();
        }

        [MenuItem("Database/Database/Dialogue", true)]
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

            DialogueEditorWindow window = GetWindow<DialogueEditorWindow>("Dialogue");
            window.Show();
            window.Focus();
            window.SetKey(key);
        }
    }
}