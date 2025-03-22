// WARNING: Generated file. Do not modify!
using UnityEditor;

namespace GameCore.Database.Editor
{
    public partial class SkillEditorWindow : DataEditorWindow<SkillData>
    {
        [MenuItem("Database/Database/Skill", priority = 0)]
        private static void OpenWindow()
        {
            SkillEditorWindow window = GetWindow<SkillEditorWindow>("Skill");
            window.Show();
            window.Focus();
        }

        [MenuItem("Database/Database/Skill", true)]
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

            SkillEditorWindow window = GetWindow<SkillEditorWindow>("Skill");
            window.Show();
            window.Focus();
            window.SetKey(key);
        }
    }
}