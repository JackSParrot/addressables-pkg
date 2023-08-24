using System;
using UnityEngine;

namespace JackSParrot.AddressablesEssentials.Editor
{
    public static class EditorUtils
    {
        public static GUIStyle MultilineLabelStyle = new GUIStyle(GUI.skin.GetStyle("label"))
        {
            wordWrap = true
        };

        public static void DrawBox(string title, Action drawContent)
        {
            using (new BoxScope(title))
            {
                drawContent();
            }
        }

        public static void DrawErrorBox(string title, Action drawContent)
        {
            using (new BoxScope(title, Color.yellow, 10))
            {
                drawContent();
            }
        }
    }
}