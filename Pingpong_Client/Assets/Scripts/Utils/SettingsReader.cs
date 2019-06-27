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
            AssetDatabase.ImportAsset(SettingsPath);
            TextAsset txtAsset = (TextAsset) Resources.Load("settings");

            Settings settings = JsonUtility.FromJson<Settings>(txtAsset.text);

            return settings;
        }
    }
}