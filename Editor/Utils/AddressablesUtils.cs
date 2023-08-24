using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace JackSParrot.AddressablesEssentials.Editor
{
    public static class AddressablesUtils
    {
        public static bool IsAssetAddressable(Object obj)
        {
            return GetAddressableEntry(obj) != null;
        }

        public static bool IsAddressEqualToName(Object obj)
        {
            AddressableAssetEntry entry = GetAddressableEntry(obj);
            return entry.address == obj.name;
        }

        private static AddressableAssetEntry GetAddressableEntry(Object obj)
        {
            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
            if (settings == null)
            {
                Debug.LogError("You need to import addressables and create the settings");
                return null;
            }

            AddressableAssetEntry entry =
                settings.FindAssetEntry(AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(obj)));
            return entry;
        }
    }
}