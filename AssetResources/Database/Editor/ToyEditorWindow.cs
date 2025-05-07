// WARNING: Generated file. Do not modify!
using UnityEditor;

namespace GameCore.Database.Editor
{
    public partial class ToyEditorWindow : DataEditorWindow<ToyData>
    {
        [MenuItem("Database/Database/Toy", priority = 0)]
        private static void OpenWindow()
        {
            ToyEditorWindow window = GetWindow<ToyEditorWindow>("Toy");
            window.Show();
            window.Focus();
        }

        [MenuItem("Database/Database/Toy", true)]
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

            ToyEditorWindow window = GetWindow<ToyEditorWindow>("Toy");
            window.Show();
            window.Focus();
            window.SetKey(key);
        }
    }
}