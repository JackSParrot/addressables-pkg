using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace JackSParrot.AddressablesEssentials
{
    public static class AddressableAssetsUtility
    {
        public static async Task<T> InstantiateAssetAsync<T>(AssetReference assetReference, Transform parent,
            bool worldPositionStays)
            where T : Component =>
            await InstantiateAssetHandler.InstantiateAssetAsync<T>(assetReference, parent, worldPositionStays);

        public static void InstantiateAsset<T>(AssetReference assetReference, Transform parent, bool worldPositionStays,
            Action<T> callback)
            where T : Component =>
            InstantiateAssetHandler.InstantiateAsset<T>(assetReference, parent, worldPositionStays, callback);

        public static async Task<T> InstantiateAssetAsync<T>(AssetReference assetReference, Vector3 position,
            Quaternion rotation,
            Transform parent) where T : Component =>
            await InstantiateAssetHandler.InstantiateAssetAsync<T>(assetReference, position, rotation, parent);

        public static void InstantiateAsset<T>(AssetReference assetReference, Vector3 position, Quaternion rotation,
            Transform parent,
            Action<T> callback) where T : Component =>
            InstantiateAssetHandler.InstantiateAsset<T>(assetReference, position, rotation, parent, callback);

        public static async Task<T> InstantiateAssetAsync<T>(string address, Transform parent, bool worldPositionStays)
            where T : Component =>
            await InstantiateAssetHandler.InstantiateAssetAsync<T>(address, parent, worldPositionStays);

        public static void InstantiateAsset<T>(string address, Transform parent, bool worldPositionStays,
            Action<T> callback)
            where T : Component =>
            InstantiateAssetHandler.InstantiateAsset<T>(address, parent, worldPositionStays, callback);

        public static async Task<T> InstantiateAssetAsync<T>(string address, Vector3 position, Quaternion rotation,
            Transform parent) where T : Component =>
            await InstantiateAssetHandler.InstantiateAssetAsync<T>(address, position, rotation, parent);

        public static void InstantiateAsset<T>(string address, Vector3 position, Quaternion rotation, Transform parent,
            Action<T> callback) where T : Component =>
            InstantiateAssetHandler.InstantiateAsset<T>(address, position, rotation, parent, callback);


        public static async Task<T> InstantiatePrefabAsync<T>(AssetReferencePrefab<T> assetReference, Transform parent,
            bool worldPositionStays) where T : Component =>
            await InstantiateAssetHandler.InstantiatePrefabAsync<T>(assetReference, parent, worldPositionStays);

        public static void InstantiatePrefab<T>(AssetReferencePrefab<T> assetReference, Transform parent,
            bool worldPositionStays, Action<T> callback) where T : Component =>
            InstantiateAssetHandler.InstantiatePrefab<T>(assetReference, parent, worldPositionStays, callback);

        public static async Task<T> InstantiatePrefabAsync<T>(AssetReferencePrefab<T> assetReference, Vector3 position,
            Quaternion rotation, Transform parent) where T : Component =>
            await InstantiateAssetHandler.InstantiatePrefabAsync<T>(assetReference, position, rotation, parent);

        public static void InstantiatePrefab<T>(AssetReferencePrefab<T> assetReference, Vector3 position,
            Quaternion rotation,
            Transform parent, Action<T> callback) where T : Component =>
            InstantiateAssetHandler.InstantiatePrefab<T>(assetReference, position, rotation, parent, callback);


        public static async Task<T> InstantiatePrefabAsync<T>(string address, Transform parent,
            bool worldPositionStays) where T : Component =>
            await InstantiateAssetHandler.InstantiatePrefabAsync<T>(address, parent, worldPositionStays);

        public static void InstantiatePrefab<T>(string address, Transform parent,
            bool worldPositionStays, Action<T> callback) where T : Component =>
            InstantiateAssetHandler.InstantiatePrefab<T>(address, parent, worldPositionStays, callback);

        public static async Task<T> InstantiatePrefabAsync<T>(string address, Vector3 position,
            Quaternion rotation, Transform parent) where T : Component =>
            await InstantiateAssetHandler.InstantiatePrefabAsync<T>(address, position, rotation, parent);

        public static void InstantiatePrefab<T>(string address, Vector3 position,
            Quaternion rotation, Transform parent, Action<T> callback) where T : Component =>
            InstantiateAssetHandler.InstantiatePrefab<T>(address, position, rotation, parent, callback);


        public static async Task<T> LoadAssetAsync<T>(AssetReference assetReference, GameObject requester)
            where T : Object
            => await LoadAssetHandler.LoadAssetAsync<T>(assetReference, requester);

        public static void LoadAsset<T>(AssetReference assetReference, GameObject requester, Action<T> callback)
            where T : Object
            => LoadAssetHandler.LoadAsset<T>(assetReference, requester, callback);

        public static async Task<T> LoadAssetAsync<T>(string address, GameObject requester)
            where T : Object
            => await LoadAssetHandler.LoadAssetAsync<T>(address, requester);

        public static void LoadAsset<T>(string address, GameObject requester, Action<T> callback)
            where T : Object
            => LoadAssetHandler.LoadAsset<T>(address, requester, callback);


        public static async Task<T> LoadPrefabAsync<T>(AssetReferencePrefab<T> assetReference, GameObject requester)
            where T : Component
            => await LoadAssetHandler.LoadPrefabAsync(assetReference, requester);

        public static void LoadPrefab<T>(AssetReferencePrefab<T> assetReference, GameObject requester,
            Action<T> callback)
            where T : Component
            => LoadAssetHandler.LoadPrefab(assetReference, requester, callback);

        public static async Task<T> LoadPrefabAsync<T>(string address, GameObject requester)
            where T : Component
            => await LoadAssetHandler.LoadPrefabAsync<T>(address, requester);

        public static void LoadPrefab<T>(string address, GameObject requester, Action<T> callback)
            where T : Component
            => LoadAssetHandler.LoadPrefab(address, requester, callback);
    }
}