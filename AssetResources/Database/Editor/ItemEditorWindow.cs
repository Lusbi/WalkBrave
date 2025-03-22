// WARNING: Generated file. Do not modify!
using UnityEditor;

namespace GameCore.Database.Editor
{
    public partial class ItemEditorWindow : DataEditorWindow<ItemData>
    {
        [MenuItem("Database/Database/Item", priority = 0)]
        private static void OpenWindow()
        {
            ItemEditorWindow window = GetWindow<ItemEditorWindow>("Item");
            window.Show();
            window.Focus();
        }

        [MenuItem("Database/Database/Item", true)]
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

            ItemEditorWindow window = GetWindow<ItemEditorWindow>("Item");
            window.Show();
            window.Focus();
            window.SetKey(key);
        }
    }
}