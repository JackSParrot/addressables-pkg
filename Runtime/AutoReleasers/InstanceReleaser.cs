using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace JackSParrot.AddressablesEssentials
{
    public class InstanceReleaser : MonoBehaviour
    {
        private void OnDestroy()
        {
            Addressables.ReleaseInstance(gameObject);
        }
    }
}