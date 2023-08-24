using System;
using UnityEngine;
using UnityEngine.U2D;

namespace JackSParrot.AddressablesEssentials
{
    [Serializable]
    public class AtlasedSpriteReferenceEntry
    {
        public string key;
        // We do not want the hard reference to the sprite to be part of the build.
        // We only want the spriteReference to be populated, but we want to assign the sprite in the editor.
#if UNITY_EDITOR
        [SerializeField]
        internal Sprite sprite = null;
        [SerializeField, ReadOnly]
        internal SpriteAtlas spriteAtlas = null;
#endif

        [SerializeField, ReadOnly]
        internal string spriteAddress;
#if UNITY_EDITOR

        internal void Invalidate()
        {
            spriteAtlas = null;
            spriteAddress = null;
        }

        internal void OnValidate(Editor.SpriteAtlasContainer[] spriteAtlases)
        {
            if (sprite == null)
            {
                Invalidate();
                return;
            }

            if (string.IsNullOrEmpty(key))
            {
                key = sprite.name;
            }

            foreach (var atlasContainer in spriteAtlases)
            {
                SpriteAtlas atlas = atlasContainer.spriteAtlas;

                if (atlas.CanBindTo(sprite))
                {
                    spriteAtlas = atlas;
                    spriteAddress = spriteAtlas.name + "[" + sprite.name + "]";
                    break;
                }
            }
        }

        internal bool IsValid()
        {
            if (spriteAtlas == null)
            {
                return false;
            }

            if (sprite == null)
            {
                return false;
            }

            if (!spriteAtlas.CanBindTo(sprite))
            {
                return false;
            }

            return !string.IsNullOrWhiteSpace(spriteAddress);
        }
#endif
    }
}