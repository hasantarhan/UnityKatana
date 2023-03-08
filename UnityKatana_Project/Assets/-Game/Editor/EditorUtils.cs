using UnityEditor;

namespace Foxy.Editor
{
    [InitializeOnLoad]
    public static class EditorUtils
    {
        private const string MENU_NAME = "Foxy/Fast Play Mode";

        private static bool enabled_;

        static EditorUtils()
        {
            enabled_ = EditorSettings.enterPlayModeOptionsEnabled;
            EditorApplication.delayCall += () => { PerformAction(enabled_); };
        }

        [MenuItem(MENU_NAME)]
        private static void ToggleAction()
        {
            PerformAction(!enabled_);
        }

        public static void PerformAction(bool enabled)
        {
            Menu.SetChecked(MENU_NAME, enabled);
            EditorPrefs.SetBool(MENU_NAME, enabled);
            enabled_ = enabled;
            EditorSettings.enterPlayModeOptionsEnabled = enabled;
        }
        
    }
}