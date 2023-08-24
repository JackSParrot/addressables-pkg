using System;
using UnityEditor;
using UnityEngine;

namespace JackSParrot.AddressablesEssentials.Editor
{
    public readonly struct BoxScope : IDisposable
    {
        private readonly Color m_color;

        public BoxScope(string title, int padding = 0) : this(title, Color.white, padding) { }

        public BoxScope(string title, Color textColor, int padding = 0)
        {
            GUILayout.BeginVertical(EditorStyles.helpBox);
            m_color = GUI.color;
            GUI.color = textColor;
            GUILayout.Label(title, EditorStyles.boldLabel);
            GUILayout.BeginHorizontal();
            GUILayout.Space(padding);
            GUILayout.BeginVertical();
        }

        public void Dispose()
        {
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUI.color = m_color;
            GUILayout.EndVertical();
        }
    }
}