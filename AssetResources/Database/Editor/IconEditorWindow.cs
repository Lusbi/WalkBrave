// WARNING: Generated file. Do not modify!
using UnityEditor;

namespace GameCore.Database.Editor
{
    public partial class IconEditorWindow : DataEditorWindow<IconData>
    {
        [MenuItem("Database/Database/Icon", priority = 0)]
        private static void OpenWindow()
        {
            IconEditorWindow window = GetWindow<IconEditorWindow>("Icon");
            window.Show();
            window.Focus();
        }

        [MenuItem("Database/Database/Icon", true)]
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

            IconEditorWindow window = GetWindow<IconEditorWindow>("Icon");
            window.Show();
            window.Focus();
            window.SetKey(key);
        }
    }
}