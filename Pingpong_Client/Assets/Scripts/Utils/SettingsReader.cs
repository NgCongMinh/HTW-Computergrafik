using Model;
using UnityEditor;
using UnityEngine;

namespace Utils
{
    public static class SettingsReader
    {
        private const string SettingsPath = "Assets/Resources/settings.json";

        public static Settings ReadSettings()
        {
            // workaround --> else cannot start without unity
#if UNITY_EDITOR
            AssetDatabase.ImportAsset(SettingsPath);
#endif
            TextAsset txtAsset = (TextAsset) Resources.Load("settings");

            Settings settings = JsonUtility.FromJson<Settings>(txtAsset.text);

            return settings;
        }
    }
}