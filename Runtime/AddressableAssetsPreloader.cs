using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace JackSParrot.AddressablesEssentials
{
    public class AddressableAssetsPreloader : IDisposable
    {
        private readonly List<AsyncOperationHandle<IList<Object>>> _preloadHandles;

        public AddressableAssetsPreloader()
        {
            _preloadHandles = new List<AsyncOperationHandle<IList<Object>>>();
        }

        ~AddressableAssetsPreloader() => Dispose();

        public async Task PreLoadAssetsAsync(List<string> addresses)
        {
            AsyncOperationHandle<IList<Object>> _handle = Addressables.LoadAssetsAsync<Object>(addresses, null, Addressables.MergeMode.Union);
            _preloadHandles.Add(_handle);
            await _handle.Task;
        }

        public async Task PreLoadAssetsAsync(List<string> addresses, Task cancellationTask)
        {
            await Task.WhenAny(new List<Task>
            {
                PreLoadAssetsAsync(addresses),
                cancellationTask
            });
        }

        public void PreLoadAssets(List<string> addresses, Action callback)
        {
            PreLoadAssetsBridge(addresses, callback).HandleBackgroundException();
        }

        private async Task PreLoadAssetsBridge(List<string> addresses, Action callback)
        {
            await PreLoadAssetsAsync(addresses);
            callback.Invoke();
        }

        public void Dispose()
        {
            Clear();
        }

        public void Clear()
        {
            for (int i = 0; i < _preloadHandles.Count; i++)
            {
                AsyncOperationHandle<IList<Object>> handle = _preloadHandles[i];
                if (handle.IsValid() && handle.IsDone)
                {
                    Addressables.Release(_preloadHandles);
                }
            }

            _preloadHandles.Clear();
        }
    }
}