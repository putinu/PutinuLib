﻿using System.IO;
using System.Text;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace PutinuLib.Audio.Editor
{
    /// <summary>
    /// SEListのインスペクタ拡張
    /// </summary>
    [CanEditMultipleObjects]
    [CustomEditor(typeof(SEList))]
    public class SEListInspector : UnityEditor.Editor
    {
        private const string StartOfDefine = "// START_OF_DEFINE";
        private bool _isSettingFoldoutOpened = false;
        private TextAsset _templateFile;
        private TextAsset _csFile;
        private ReorderableList _seList;
        
        private void OnEnable()
        {
            _seList = new ReorderableList(serializedObject, serializedObject.FindProperty("_seList"));
            
            _seList.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                var element = _seList.serializedProperty.GetArrayElementAtIndex(index);
                EditorGUI.PropertyField(rect, element);
            };
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            _isSettingFoldoutOpened = EditorGUILayout.BeginFoldoutHeaderGroup(_isSettingFoldoutOpened, "設定ファイル");
            if (_isSettingFoldoutOpened)
            {
                _templateFile = (TextAsset) EditorGUILayout.ObjectField(
                    "テンプレートファイル", _templateFile, typeof(TextAsset), false);
                _csFile = (TextAsset) EditorGUILayout.ObjectField(
                    "スクリプトファイル", _csFile, typeof(TextAsset), false);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            
            _seList.DoLayoutList();
            if (GUILayout.Button("リストを更新"))
            {
                RefreshType();
            }
            
            serializedObject.ApplyModifiedProperties();
        }

        private void RefreshType()
        {
            StringBuilder audioNameBuilder = new();
            
            for (int i = 0; i < _seList.serializedProperty.arraySize; i++)
            {
                SerializedProperty element = _seList.serializedProperty.GetArrayElementAtIndex(i);
                
                audioNameBuilder.Append(element.objectReferenceValue.name);
                audioNameBuilder.Append(",\r        ");
            }

            string replacedText = _templateFile.text
                .Replace(StartOfDefine, audioNameBuilder.ToString());

            string csFilePath = AssetDatabase.GetAssetPath(_csFile);
            File.WriteAllText(csFilePath, replacedText);
            AssetDatabase.Refresh();
        }
    }
}