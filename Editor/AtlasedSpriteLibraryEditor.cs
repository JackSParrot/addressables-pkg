using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace JackSParrot.AddressablesEssentials.Editor
{
    [CustomEditor(typeof(AtlasedSpriteLibrary))]
    public class AtlasedSpriteLibraryEditor : UnityEditor.Editor
    {
        private SerializedProperty _sprites;
        private SerializedProperty _defaultMissingSprite;

        private AtlasedSpriteLibrary _typedTarget;

        // Cached collections
        private List<string> _errorList   = new List<string>(10);
        private List<string> _warningList = new List<string>(10);

        private void OnEnable()
        {
            _sprites = serializedObject.FindProperty(nameof(AtlasedSpriteLibrary.sprites));
            _defaultMissingSprite = serializedObject.FindProperty(nameof(AtlasedSpriteLibrary.defaultMissingSprite));
            _typedTarget = serializedObject.targetObject as AtlasedSpriteLibrary;

            string label = nameof(AtlasedSpriteLibrary);
            string[] labels = AssetDatabase.GetLabels(_typedTarget);
            if (!labels.Contains(label))
            {
                List<string> newLabels = new List<string>(labels);
                newLabels.Add(label);
                AssetDatabase.SetLabels(_typedTarget, newLabels.ToArray());
                AssetDatabase.SaveAssets();
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.UpdateIfRequiredOrScript();

            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
            }

            if (!_typedTarget.IsValid())
            {
                SpriteAtlasContainer[] spriteAtlases = AssetDatabaseUtils.LoadAllSpriteAtlases();
                UpdateSpriteAtlasReferences(_typedTarget, spriteAtlases);
            }

            _errorList.Clear();
            _warningList.Clear();
            if (!ValidateList(_typedTarget, _errorList, _warningList))
            {
                if (_errorList.Count > 0)
                {
                    EditorGUILayout.Space();
                    EditorUtils.DrawErrorBox("Errors", () =>
                    {
                        for (int i = 0; i < _errorList.Count; i++)
                        {
                            EditorGUILayout.LabelField(" - " + _errorList[i], EditorUtils.MultilineLabelStyle);
                        }
                    });
                }

                if (_warningList.Count > 0)
                {
                    EditorGUILayout.Space();
                    EditorUtils.DrawBox("Warnings", () =>
                    {
                        for (int i = 0; i < _warningList.Count; i++)
                        {
                            EditorGUILayout.LabelField(" - " + _warningList[i], EditorUtils.MultilineLabelStyle);
                        }
                    });
                }
            }

            EditorGUILayout.PropertyField(_defaultMissingSprite);

            EditorGUILayout.PropertyField(_sprites);
            if (GUILayout.Button("Order List by Key"))
            {
                OrderListByKey();
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void OrderListByKey()
        {
            _typedTarget.sprites.Sort(OrderByKey);
        }


        internal static bool ValidateList(AtlasedSpriteLibrary settings, List<string> errorList,
            List<string> warningList)
        {
            ValidateAtlasedSpriteReferenceEntry(settings.defaultMissingSprite, errorList);

            HashSet<string> processedKeys = new HashSet<string>();
            HashSet<Sprite> processedSprites = new HashSet<Sprite>();

            for (var i = 0; i < settings.sprites.Count; i++)
            {
                AtlasedSpriteReferenceEntry entry = settings.sprites[i];
                ValidateAtlasedSpriteReferenceEntry(entry, errorList);

                if (processedKeys.Contains(entry.key))
                {
                    errorList.Add($"Entry {entry.key} is duplicated at position {(i + 1).ToString()}");
                }

                if (processedSprites.Contains(entry.sprite))
                {
                    warningList.Add(
                        $"Sprite at position {(i + 1).ToString()} [{entry.sprite.name}] is already present in another position");
                }

                processedKeys.Add(entry.key);
                processedSprites.Add(entry.sprite);
            }

            return errorList.Count == 0 && warningList.Count == 0;
        }

        private static void ValidateAtlasedSpriteReferenceEntry(AtlasedSpriteReferenceEntry entry,
            List<string> errorList)
        {
            if (string.IsNullOrEmpty(entry.key) && entry.sprite != null)
            {
                errorList.Add($"Entry found with no key. Should have been fixed with OnValidate");
            }

            if (entry.sprite == null)
            {
                errorList.Add($"Entry {entry.key} is missing the sprite");
                return;
            }

            if (entry.spriteAtlas == null)
            {
                errorList.Add($"Entry {entry.key} is not in an atlas");
                return;
            }

            if (!AddressablesUtils.IsAssetAddressable(entry.spriteAtlas))
            {
                errorList.Add($"SpriteAtlas {entry.spriteAtlas.name} is not marked as Addressable");
            }
            else if (!AddressablesUtils.IsAddressEqualToName(entry.spriteAtlas))
            {
                errorList.Add(
                    $"SpriteAtlases need to have addresses equal to their names. Please check: {entry.spriteAtlas.name}");
            }
        }

        internal static void UpdateSpriteAtlasReferences(AtlasedSpriteLibrary settings, SpriteAtlasContainer[] spriteAtlases)
        {
            UpdateSpriteAtlasEntry(settings.defaultMissingSprite, spriteAtlases);
            foreach (AtlasedSpriteReferenceEntry entry in settings.sprites)
            {
                UpdateSpriteAtlasEntry(entry, spriteAtlases);
            }

            EditorUtility.SetDirty(settings);
        }

        private static void UpdateSpriteAtlasEntry(AtlasedSpriteReferenceEntry entry, SpriteAtlasContainer[] spriteAtlases)
        {
            if (entry.sprite == null)
            {
                return;
            }

            entry.OnValidate(spriteAtlases);
            if (!entry.IsValid())
            {
                entry.Invalidate();
            }
        }

        private int OrderByKey(AtlasedSpriteReferenceEntry a, AtlasedSpriteReferenceEntry b)
        {
            return string.Compare(a.key, b.key, CultureInfo.InvariantCulture, CompareOptions.None);
        }
    }
}