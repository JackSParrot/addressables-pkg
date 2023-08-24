using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

#if UNITY_EDITOR
using JackSParrot.AddressablesEssentials.Editor;
#endif

namespace JackSParrot.AddressablesEssentials
{
    [CreateAssetMenu(fileName = nameof(AtlasedSpriteLibrary),
        menuName = "AddressablesEssentials/" + nameof(AtlasedSpriteLibrary))]
    public class AtlasedSpriteLibrary : ScriptableObject
    {
        [SerializeField]
        [Tooltip("Sprite to be loaded in case an invalid key is requested")]
        internal AtlasedSpriteReferenceEntry defaultMissingSprite;

        [Tooltip("In order to correctly dynamic load sprites that are part of an atlas, " +
                 "we need to have a reference to the Sprite Atlas and request the sprite from there")]
        [SerializeField]
        internal List<AtlasedSpriteReferenceEntry> sprites = new List<AtlasedSpriteReferenceEntry>();

        public string GetSpriteAddress(string key)
        {
            AtlasedSpriteReferenceEntry entry = sprites.Find(s => s.key == key);
            if (entry != null)
            {
                return entry.spriteAddress;
            }

            return defaultMissingSprite.spriteAddress;
        }

        public void LoadSprite(string key, GameObject requester, Action<Sprite> onLoaded)
        {
            LoadAssetHandler.LoadAsset(GetSpriteAddress(key), requester, onLoaded);
        }

        public async Task<Sprite> LoadSpriteAsync(string key, GameObject requester)
        {
            return await LoadAssetHandler.LoadAssetAsync<Sprite>(GetSpriteAddress(key), requester);
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            if (Application.isPlaying)
            {
                // Ok, this needs an explanation. Get a coffee and sit tight.
                // OnValidate will run in the Unity Editor when loading Scriptable Objects into memory
                // and when instantiating MonoBehaviours.
                // This doesn't happen on a build of your game! 

                // The big problem now is when testing the asset bundles you have in your project,
                // you usually go to the Addressables Group Window and set the Play Mode Script to "Use Existing Build"
                // This will force Unity to load the asset bundles and not get the assets from the Asset Library.
                // BUT because you are in the editor, it will run OnValidate!

                // Now because we rely on OnValidate to configure our AtlasedSpriteReferenceEntry correctly, it will actually 
                // configure it with bad references, and the AtlasedSpriteReferenceEntry will not be valid anymore.

                // So, long store short, we don't want OnValidate to EVER run if the Application is playing!    
                return;
            }

            SpriteAtlasContainer[] spriteAtlases = AssetDatabaseUtils.LoadAllSpriteAtlases();

            defaultMissingSprite.key = "missing_asset";
            for (int i = 0; i < sprites.Count; i++)
            {
                AtlasedSpriteReferenceEntry atlasedSpriteRef = sprites[i];
                atlasedSpriteRef.OnValidate(spriteAtlases);
            }
        }

        internal bool IsValid()
        {
            if (!defaultMissingSprite.IsValid())
            {
                return false;
            }

            for (int i = 0; i < sprites.Count; i++)
            {
                AtlasedSpriteReferenceEntry atlasedSpriteRef = sprites[i];
                if (!atlasedSpriteRef.IsValid())
                {
                    return false;
                }
            }

            return true;
        }
#endif
    }
}