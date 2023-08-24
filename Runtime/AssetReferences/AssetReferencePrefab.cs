using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace JackSParrot.AddressablesEssentials
{
    [Serializable]
    public class AssetReferencePrefab<T> : AssetReferenceGameObject where T : Component
    {
        /// <summary>
        /// Constructs a new reference to a MyStuff.
        /// </summary>
        /// <param name="guid">The object guid.</param>
        public AssetReferencePrefab(string guid) : base(guid)
        {
        }

        public override bool ValidateAsset(string path)
        {
#if UNITY_EDITOR
            T loadedAsset = AssetDatabase.LoadAssetAtPath<T>(path);
            return loadedAsset != null;
#else
            return false;
#endif
        }

        // When loading assets this way, you will have to release it to not leak memory
        public new async Task<TComp> LoadAssetAsync<TComp>() where TComp : Component
        {
            GameObject go = await LoadAssetAsync().Task;
            return go.GetComponent<TComp>();
        }
    }
}