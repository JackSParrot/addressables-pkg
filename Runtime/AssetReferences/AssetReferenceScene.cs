using UnityEngine.AddressableAssets;

namespace JackSParrot.AddressablesEssentials
{
    [System.Serializable]
    public class AssetReferenceScene : AssetReference
    {
        public override bool ValidateAsset(string path)
        {
            return path.EndsWith(".unity");
        }
    }
}