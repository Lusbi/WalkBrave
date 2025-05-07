// WARNING: Generated file. Do not modify!
using UnityEditor;

namespace GameCore.Database.Editor
{
    public partial class CompositeEditorWindow : DataEditorWindow<CompositeData>
    {
        [MenuItem("Database/Database/Composite", priority = 0)]
        private static void OpenWindow()
        {
            CompositeEditorWindow window = GetWindow<CompositeEditorWindow>("Composite");
            window.Show();
            window.Focus();
        }

        [MenuItem("Database/Database/Composite", true)]
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

            CompositeEditorWindow window = GetWindow<CompositeEditorWindow>("Composite");
            window.Show();
            window.Focus();
            window.SetKey(key);
        }
    }
}