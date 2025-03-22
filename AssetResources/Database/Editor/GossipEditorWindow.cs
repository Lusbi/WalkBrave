// WARNING: Generated file. Do not modify!
using UnityEditor;

namespace GameCore.Database.Editor
{
    public partial class GossipEditorWindow : DataEditorWindow<GossipData>
    {
        [MenuItem("Database/Database/Gossip", priority = 0)]
        private static void OpenWindow()
        {
            GossipEditorWindow window = GetWindow<GossipEditorWindow>("Gossip");
            window.Show();
            window.Focus();
        }

        [MenuItem("Database/Database/Gossip", true)]
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

            GossipEditorWindow window = GetWindow<GossipEditorWindow>("Gossip");
            window.Show();
            window.Focus();
            window.SetKey(key);
        }
    }
}