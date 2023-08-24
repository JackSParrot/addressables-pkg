using System.Collections.Generic;
using System.Diagnostics;
using JackSParrot.AddressablesEssentials.Editor;
using UnityEditor;

namespace JackSParrot.AddressablesEssentials
{
    [InitializeOnLoad]
    public class AddressablesAssetLoaderSettingsGuard
    {
        static AddressablesAssetLoaderSettingsGuard()
        {
            EditorApplication.playModeStateChanged += OnPlayModeChanged;
        }

        private static void OnPlayModeChanged(PlayModeStateChange newState)
        {
            switch (newState)
            {
                case PlayModeStateChange.EnteredPlayMode:
                    Stopwatch stopwatch = Stopwatch.StartNew();
                    string[] librariesGuid = AssetDatabase.FindAssets($"l:{nameof(AtlasedSpriteLibrary)}");
                    if (librariesGuid.Length <= 0)
                    {
                        stopwatch.Stop();
                        UnityEngine.Debug.Log($"Didn't find any AtlasedSpriteLibrary :: {stopwatch.ElapsedMilliseconds.ToString()}ms");
                        return;
                    }

                    long elapsed1 = stopwatch.ElapsedMilliseconds;
                    UnityEngine.Debug.Log($"Get AtlasedSpriteLibraries GUIDs :: {elapsed1.ToString()}ms");

                    SpriteAtlasContainer[] spriteAtlases = AssetDatabaseUtils.LoadAllSpriteAtlases();
                    long elapsed2 = stopwatch.ElapsedMilliseconds - elapsed1;
                    UnityEngine.Debug.Log($"Get Sprite Atlases Loaded :: {elapsed2.ToString()}ms");

                    List<string> libraryErrors = new List<string>();
                    List<string> libraryWarnings = new List<string>();
                    foreach (string libraryGuid in librariesGuid)
                    {
                        string path = AssetDatabase.GUIDToAssetPath(libraryGuid);
                        AtlasedSpriteLibrary library = AssetDatabase.LoadAssetAtPath<AtlasedSpriteLibrary>(path);

                        // This will force an Update to the reference to AtlasedSpriteReferences.
                        // This is useful when you move a sprite from an Atlas to another
                        // and press play without clicking on the AtlasedSpriteLibrary asset.
                        AtlasedSpriteLibraryEditor.UpdateSpriteAtlasReferences(library, spriteAtlases);

                        libraryErrors.Clear();
                        libraryWarnings.Clear();
                        if (!AtlasedSpriteLibraryEditor.ValidateList(library, libraryErrors, libraryWarnings))
                        {
                            if (libraryErrors.Count > 0)
                            {
                                UnityEngine.Debug.LogError("Errors found in " + library.name);
                                foreach (string errorMessage in libraryErrors)
                                {
                                    UnityEngine.Debug.LogError(errorMessage);
                                }
                            }

                            if (libraryWarnings.Count > 0)
                            {
                                UnityEngine.Debug.LogWarning("Warnings found in " + library.name);
                                foreach (string warningMessage in libraryWarnings)
                                {
                                    UnityEngine.Debug.LogWarning(warningMessage);
                                }
                            }
                        }
                    }

                    long elapsed3 = stopwatch.ElapsedMilliseconds - elapsed2;
                    UnityEngine.Debug.Log($"All libraries updated :: {elapsed3.ToString()}ms");

                    AssetDatabase.SaveAssets();

                    long elapsed4 = stopwatch.ElapsedMilliseconds - elapsed3;
                    UnityEngine.Debug.Log($"AssetDatabase.SaveAssets :: {elapsed4.ToString()}ms");

                    stopwatch.Stop();
                    UnityEngine.Debug.Log($"Total time to update " +
                                                 $"AtlasedSpriteLibraries: {stopwatch.ElapsedMilliseconds.ToString()}ms :: " +
                                                 $"Libraries found: {librariesGuid.Length} :: " +
                                                 $"SpriteAtlas found: {spriteAtlases.Length}");
                    break;
            }
        }
    }
}