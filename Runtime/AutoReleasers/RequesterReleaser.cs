using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace JackSParrot.AddressablesEssentials
{
    internal class RequesterReleaser : MonoBehaviour
    {
        private List<AsyncOperationHandle> handles = new List<AsyncOperationHandle>();

        public void AddHandler(AsyncOperationHandle handle)
        {
            handles.Add(handle);
        }

        private void OnDestroy()
        {
            foreach (AsyncOperationHandle handle in handles)
            {
                if (handle.IsDone)
                {
                    Addressables.Release(handle);
                }
            }

            handles.Clear();
        }
    }
}