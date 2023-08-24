using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace JackSParrot.AddressablesEssentials
{
    internal static class InstantiateAssetHandler
    {
        public static async Task<T> InstantiateAssetAsync<T>(AssetReference assetReference, Transform parent,
            bool worldPositionStays) where T : Component
        {
            GameObject result = await Addressables.InstantiateAsync(assetReference, parent, worldPositionStays).Task;

            if (!TryReturnComponent(result, out T component))
            {
                Debug.LogError(
                    $"{assetReference} failed: Valid Result? {(result != null).ToString()}, " +
                    $"Found component of type {typeof(T)}? {(component != null).ToString()}");
            }

            return component;
        }

        public static async Task<T> InstantiateAssetAsync<T>(string address, Transform parent, bool worldPositionStays)
            where T : Component
        {
            GameObject result = await Addressables.InstantiateAsync(address, parent, worldPositionStays).Task;
            if (!TryReturnComponent<T>(result, out T component))
            {
                Debug.LogError(
                    $"{address} failed: Valid Result? {(result != null).ToString()}, " +
                    $"Found component of type {typeof(T)}? {(component != null).ToString()}");
            }

            return component;
        }

        public static async Task<T> InstantiateAssetAsync<T>(AssetReference assetReference, Vector3 position,
            Quaternion rotation, Transform parent) where T : Component
        {
            GameObject result = await Addressables.InstantiateAsync(assetReference, position, rotation, parent).Task;
            if (!TryReturnComponent<T>(result, out T component))
            {
                Debug.LogError(
                    $"{assetReference} failed: Valid Result? {(result != null).ToString()}, " +
                    $"Found component of type {typeof(T)}? {(component != null).ToString()}");
            }

            return component;
        }

        public static async Task<T> InstantiateAssetAsync<T>(string address, Vector3 position, Quaternion rotation,
            Transform parent) where T : Component
        {
            GameObject result = await Addressables.InstantiateAsync(address, position, rotation, parent).Task;
            if (!TryReturnComponent<T>(result, out T component))
            {
                Debug.LogError(
                    $"{address} failed: Valid Result? {(result != null).ToString()}, " +
                    $"Found component of type {typeof(T)}? {(component != null).ToString()}");
            }

            return component;
        }

        public static void InstantiateAsset<T>(string address, Vector3 position, Quaternion rotation, Transform parent,
            Action<T> callback) where T : Component
        {
            InstantiateAssetBridge(address, position, rotation, parent, callback).HandleBackgroundException();
        }

        public static void InstantiateAsset<T>(string address, Transform parent, bool worldPositionStays,
            Action<T> callback)
            where T : Component
        {
            InstantiateAssetBridge(address, parent, worldPositionStays, callback).HandleBackgroundException();
        }

        public static void InstantiateAsset<T>(AssetReference assetReference, Transform parent,
            bool worldPositionStays, Action<T> callback)
            where T : Component
        {
            InstantiateAssetBridge(assetReference, parent, worldPositionStays, callback).HandleBackgroundException();
        }

        public static void InstantiateAsset<T>(AssetReference assetReference, Vector3 position, Quaternion rotation,
            Transform parent,
            Action<T> callback) where T : Component
        {
            InstantiateAssetBridge(assetReference, position, rotation, parent, callback).HandleBackgroundException();
        }

        private static async Task InstantiateAssetBridge<T>(string address, Vector3 position,
            Quaternion rotation, Transform parent, Action<T> callback) where T : Component
        {
            T result = await InstantiateAssetAsync<T>(address, position, rotation, parent);
            callback(result);
        }

        private static async Task InstantiateAssetBridge<T>(string address, Transform parent, bool worldPositionStays,
            Action<T> callback) where T : Component
        {
            T result = await InstantiateAssetAsync<T>(address, parent, worldPositionStays);
            callback(result);
        }

        private static async Task InstantiateAssetBridge<T>(AssetReference assetReference, Transform parent,
            bool worldPositionStays, Action<T> callback)
            where T : Component
        {
            T component = await InstantiateAssetAsync<T>(assetReference, parent, worldPositionStays);
            callback?.Invoke(component);
        }

        private static async Task InstantiateAssetBridge<T>(AssetReference assetReference, Vector3 position,
            Quaternion rotation, Transform parent, Action<T> callback) where T : Component
        {
            T result = await InstantiateAssetAsync<T>(assetReference, position, rotation, parent);
            callback(result);
        }


        public static async Task<T> InstantiatePrefabAsync<T>(AssetReferencePrefab<T> assetReference, Transform parent,
            bool worldPositionStays)
            where T : Component
        {
            GameObject result = await Addressables.InstantiateAsync(assetReference, parent, worldPositionStays).Task;
            if (!TryReturnComponent(result, out T component))
            {
                Debug.LogError($"{assetReference} failed: Component [{typeof(T)}] is null");
            }

            return component;
        }

        public static async Task<T> InstantiatePrefabAsync<T>(string address, Transform parent,
            bool worldPositionStays)
            where T : Component
        {
            GameObject result = await Addressables.InstantiateAsync(address, parent, worldPositionStays).Task;
            if (!TryReturnComponent(result, out T component))
            {
                Debug.LogError($"{address} failed: Component [{typeof(T)}] is null");
            }

            return component;
        }

        public static void InstantiatePrefab<T>(AssetReferencePrefab<T> assetReference, Transform parent,
            bool worldPositionStays, Action<T> callback) where T : Component
        {
            InstantiatePrefabBridge(assetReference, parent, worldPositionStays, callback).HandleBackgroundException();
        }

        public static void InstantiatePrefab<T>(string address, Transform parent,
            bool worldPositionStays, Action<T> callback) where T : Component
        {
            InstantiatePrefabBridge(address, parent, worldPositionStays, callback).HandleBackgroundException();
        }

        private static async Task InstantiatePrefabBridge<T>(string address, Transform parent,
            bool worldPositionStays, Action<T> callback) where T : Component
        {
            T result = await InstantiatePrefabAsync<T>(address, parent, worldPositionStays);
            callback?.Invoke(result);
        }

        private static async Task InstantiatePrefabBridge<T>(AssetReferencePrefab<T> assetReference, Transform parent,
            bool worldPositionStays, Action<T> callback) where T : Component
        {
            T result = await InstantiatePrefabAsync<T>(assetReference, parent, worldPositionStays);
            callback?.Invoke(result);
        }

        public static async Task<T> InstantiatePrefabAsync<T>(AssetReferencePrefab<T> assetReference, Vector3 position,
            Quaternion rotation,
            Transform parent) where T : Component
        {
            GameObject result = await Addressables.InstantiateAsync(assetReference, position, rotation, parent).Task;
            if (!TryReturnComponent(result, out T component))
            {
                Debug.LogError($"{assetReference} failed: Component [{typeof(T)}] is null");
            }

            return component;
        }

        public static async Task<T> InstantiatePrefabAsync<T>(string address, Vector3 position,
            Quaternion rotation, Transform parent) where T : Component
        {
            GameObject result = await Addressables.InstantiateAsync(address, position, rotation, parent).Task;
            if (!TryReturnComponent(result, out T component))
            {
                Debug.LogError($"{address} failed: Component [{typeof(T)}] is null");
            }

            return component;
        }

        public static void InstantiatePrefab<T>(AssetReferencePrefab<T> assetReference, Vector3 position,
            Quaternion rotation,
            Transform parent, Action<T> callback) where T : Component
        {
            InstantiatePrefabBridge(assetReference, position, rotation, parent, callback).HandleBackgroundException();
        }

        public static void InstantiatePrefab<T>(string address, Vector3 position, Quaternion rotation,
            Transform parent, Action<T> callback) where T : Component
        {
            InstantiatePrefabBridge(address, position, rotation, parent, callback).HandleBackgroundException();
        }

        private static async Task InstantiatePrefabBridge<T>(string address, Vector3 position,
            Quaternion rotation, Transform parent, Action<T> callback) where T : Component
        {
            T result = await InstantiatePrefabAsync<T>(address, position, rotation, parent);
            callback?.Invoke(result);
        }

        private static async Task InstantiatePrefabBridge<T>(AssetReferencePrefab<T> assetReference, Vector3 position,
            Quaternion rotation, Transform parent, Action<T> callback) where T : Component
        {
            T result = await InstantiatePrefabAsync<T>(assetReference, position, rotation, parent);
            callback?.Invoke(result);
        }


        private static bool TryReturnComponent<T>(GameObject loadedGameObject, out T component) where T : Component
        {
            if (loadedGameObject != null)
            {
                component = loadedGameObject.GetComponent<T>();
                return TryAddInstanceReleaser(component);
            }

            component = null;
            return false;
        }

        private static bool TryAddInstanceReleaser<T>(T component) where T : Component
        {
            if (component != null)
            {
                GameObject loadedGameObject = component.gameObject;
                if (!loadedGameObject.TryGetComponent(out InstanceReleaser releaser))
                {
                    loadedGameObject.AddComponent<InstanceReleaser>();
                }

                return true;
            }

            return false;
        }
    }
}