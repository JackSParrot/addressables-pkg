using UnityEngine;
using UnityEngine.U2D;

#if UNITY_EDITOR
namespace JackSParrot.AddressablesEssentials.Editor
{
    public class SpriteAtlasContainer
    {
        public  string      guid;
        private SpriteAtlas _spriteAtlas;

        public SpriteAtlas spriteAtlas
        {
            get
            {
                if (_spriteAtlas == null)
                {
                    string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                    _spriteAtlas = UnityEditor.AssetDatabase.LoadAssetAtPath<SpriteAtlas>(path);
                }

                return _spriteAtlas;
            }
        }
    }
}
#endif