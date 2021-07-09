// Copyright 2017-2021 Elringus (Artyom Sovetnikov). All rights reserved.

using UnityEditor;

namespace Naninovel
{
    [CustomEditor(typeof(PlayScript))]
    public class PlayScriptEditor : Editor
    {
        private SerializedProperty scriptName;
        private SerializedProperty scriptText;
        private SerializedProperty playOnAwake;

        private void OnEnable ()
        {
            scriptName = serializedObject.FindProperty("scriptName");
            scriptText = serializedObject.FindProperty("scriptText");
            playOnAwake = serializedObject.FindProperty("playOnAwake");
        }

        public override void OnInspectorGUI ()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(scriptName);
            if (string.IsNullOrEmpty(scriptName.stringValue))
                EditorGUILayout.PropertyField(scriptText);
            EditorGUILayout.PropertyField(playOnAwake);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
