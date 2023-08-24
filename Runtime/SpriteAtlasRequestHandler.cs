using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.U2D;

namespace JackSParrot.AddressablesEssentials
{
    internal static class SpriteAtlasRequestHandler
    {
        [RuntimeInitializeOnLoadMethod]
        private static void Initialize()
        {
            SpriteAtlasManager.atlasRequested -= OnAtlasRequested;
            SpriteAtlasManager.atlasRequested += OnAtlasRequested;
        }

        // This method is called for Atlases that are not included in the build and need to be downloaded
        // when first requested.
        // WARNING: Unity will pass the Asset name and NOT THE ADDRESS.
        //          So the SpriteAtlases have to have the same name as their Addresses.
        //          Horrible
        private static async void OnAtlasRequested(string assetName, Action<SpriteAtlas> callback)
        {
            SpriteAtlas atlas = await Addressables.LoadAssetAsync<SpriteAtlas>(assetName).Task;
            callback(atlas);
        }
    }
}