// WARNING: Generated file. Do not modify!
using UnityEditor;

namespace GameCore.Database.Editor
{
    public partial class JobEditorWindow : DataEditorWindow<JobData>
    {
        [MenuItem("Database/Database/Job", priority = 0)]
        private static void OpenWindow()
        {
            JobEditorWindow window = GetWindow<JobEditorWindow>("Job");
            window.Show();
            window.Focus();
        }

        [MenuItem("Database/Database/Job", true)]
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

            JobEditorWindow window = GetWindow<JobEditorWindow>("Job");
            window.Show();
            window.Focus();
            window.SetKey(key);
        }
    }
}