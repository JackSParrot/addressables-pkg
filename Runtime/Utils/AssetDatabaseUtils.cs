#if UNITY_EDITOR
using UnityEngine.U2D;

namespace JackSParrot.AddressablesEssentials.Editor
{
    public static class AssetDatabaseUtils
    {
        public static SpriteAtlasContainer[] LoadAllSpriteAtlases()
        {
            string[] atlasesGuid = UnityEditor.AssetDatabase.FindAssets($"t:{nameof(SpriteAtlas)}");
            SpriteAtlasContainer[] spriteAtlases = new SpriteAtlasContainer[atlasesGuid.Length];
            for (int i = 0; i < atlasesGuid.Length; i++)
            {
                SpriteAtlasContainer spriteAtlasContainer = new SpriteAtlasContainer()
                {
                    guid = atlasesGuid[i]
                };
                spriteAtlases[i] = spriteAtlasContainer;
            }

            return spriteAtlases;
        }
    }
}
#endif