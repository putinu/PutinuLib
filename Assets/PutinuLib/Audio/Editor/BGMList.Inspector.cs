﻿using System.IO;
using System.Text;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace PutinuLib.Audio.Editor
{
    /// <summary>
    /// BGMListのインスペクタ拡張
    /// </summary>
    [CanEditMultipleObjects]
    [CustomEditor(typeof(BGMList))]
    public class BGMListInspector : UnityEditor.Editor
    {
        private const string StartOfDefine = "// START_OF_DEFINE";
        private bool _isSettingFoldoutOpened = false;
        private TextAsset _templateFile;
        private TextAsset _csFile;
        private ReorderableList _bgmList;

        private void OnEnable()
        {
            ReattachSettingFile();
            _bgmList = new ReorderableList(serializedObject, serializedObject.FindProperty("_bgmList"));
            
            _bgmList.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                SerializedProperty element = _bgmList.serializedProperty.GetArrayElementAtIndex(index);
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
                if (GUILayout.Button("設定ファイルを再設定"))
                {
                    ReattachSettingFile();
                }
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            _bgmList.DoLayoutList();
            if (GUILayout.Button("リストを更新"))
            {
                RefreshType();
            }
            
            serializedObject.ApplyModifiedProperties();
        }

        private void ReattachSettingFile()
        {
            const string templateFilePass = "Assets/PutinuLib/Audio/Editor/BGMList.Template.txt";
            const string csFilePass = "Assets/PutinuLib/Audio/Runtime/BGMType.cs";
            _templateFile ??= AssetDatabase.LoadAssetAtPath<TextAsset>(templateFilePass);
            _csFile ??= AssetDatabase.LoadAssetAtPath<TextAsset>(csFilePass);
        }

        private void RefreshType()
        {
            StringBuilder audioNameBuilder = new();
            
            for (int i = 0; i < _bgmList.serializedProperty.arraySize; i++)
            {
                SerializedProperty element = _bgmList.serializedProperty.GetArrayElementAtIndex(i);
                
                audioNameBuilder.Append(element.objectReferenceValue.name);
                audioNameBuilder.Append(",\r        ");
            }

            string replacedText = _templateFile.text
                .Replace(StartOfDefine, audioNameBuilder.ToString());

            string csFilePass = AssetDatabase.GetAssetPath(_csFile);
            File.WriteAllText(csFilePass, replacedText);
            AssetDatabase.Refresh();
        }
    }
}