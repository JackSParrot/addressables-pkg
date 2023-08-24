using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using Object = UnityEngine.Object;

namespace JackSParrot.AddressablesEssentials
{
    internal static class LoadAssetHandler
    {
        public static async Task<T> LoadAssetAsync<T>(string address, GameObject requester)
            where T : Object
        {
            if (string.IsNullOrEmpty(address))
            {
                Debug.LogError($"Address is null or empty!");
                return null;
            }

            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(address);
            T result = await handle.Task;
            if (result == null)
            {
                Debug.LogError($"Addressables failed loading asset at address [{address}]");
                return null;
            }

            //If the requester has already been destroyed, clean it up and exit
            if (requester == null)
            {
                Addressables.Release(handle);
                return null;
            }

            // Add auto releaser to Requester if not present
            if (!requester.TryGetComponent(out RequesterReleaser releaser))
            {
                releaser = requester.AddComponent<RequesterReleaser>();
            }

            releaser.AddHandler(handle);
            return result;
        }

        public static async Task<T> LoadAssetAsync<T>(AssetReference assetReference, GameObject requester)
            where T : Object
        {
            string address = await GetAddressFromAssetReferenceAsync(assetReference);
            return await LoadAssetAsync<T>(address, requester);
        }

        public static void LoadAsset<T>(string address, GameObject requester, Action<T> callback)
            where T : Object
        {
            LoadAssetBridge(address, requester, callback).HandleBackgroundException();
        }

        private static async Task LoadAssetBridge<T>(string address, GameObject requester,
            Action<T> callback)
            where T : Object
        {
            T result = await LoadAssetAsync<T>(address, requester);
            if (requester != null)
            {
                callback?.Invoke(result);
            }
        }

        public static void LoadAsset<T>(AssetReference assetReference, GameObject requester, Action<T> callback)
            where T : Object
        {
            LoadAssetBridge(assetReference, requester, callback).HandleBackgroundException();
        }

        private static async Task LoadAssetBridge<T>(AssetReference assetReference, GameObject requester,
            Action<T> callback)
            where T : Object
        {
            T result = await LoadAssetAsync<T>(assetReference, requester);
            if (requester != null)
            {
                callback?.Invoke(result);
            }
        }

        public static async Task<T> LoadPrefabAsync<T>(AssetReferencePrefab<T> assetReference, GameObject requester)
            where T : Component
        {
            GameObject resultGO = await LoadAssetAsync<GameObject>(assetReference, requester);
            return resultGO != null ? resultGO.GetComponent<T>() : null;
        }

        public static async Task<T> LoadPrefabAsync<T>(string address, GameObject requester)
            where T : Component
        {
            GameObject resultGO = await LoadAssetAsync<GameObject>(address, requester);
            return resultGO != null ? resultGO.GetComponent<T>() : null;
        }

        public static void LoadPrefab<T>(AssetReferencePrefab<T> assetReference, GameObject requester,
            Action<T> callback)
            where T : Component
        {
            LoadPrefabBridge(assetReference, requester, callback).HandleBackgroundException();
        }

        private static async Task LoadPrefabBridge<T>(AssetReferencePrefab<T> assetReference, GameObject requester,
            Action<T> callback)
            where T : Component
        {
            T result = await LoadPrefabAsync<T>(assetReference, requester);
            if (requester != null)
            {
                callback?.Invoke(result);
            }
        }

        public static void LoadPrefab<T>(string address, GameObject requester,
            Action<T> callback)
            where T : Component
        {
            LoadPrefabBridge(address, requester, callback).HandleBackgroundException();
        }

        private static async Task LoadPrefabBridge<T>(string address, GameObject requester,
            Action<T> callback)
            where T : Component
        {
            T result = await LoadPrefabAsync<T>(address, requester);
            if (requester != null)
            {
                callback?.Invoke(result);
            }
        }

        internal static async Task<string> GetAddressFromAssetReferenceAsync(AssetReference reference)
        {
            AsyncOperationHandle<IList<IResourceLocation>> loadResourceLocations =
                Addressables.LoadResourceLocationsAsync(reference);
            IList<IResourceLocation> result = await loadResourceLocations.Task;
            if (result.Count > 0)
            {
                string key = result[0].PrimaryKey;
                Addressables.Release(loadResourceLocations);
                return key;
            }

            Addressables.Release(loadResourceLocations);
            return string.Empty;
        }
    }
}