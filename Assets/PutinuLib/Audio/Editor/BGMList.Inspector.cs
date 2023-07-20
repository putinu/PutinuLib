using System.IO;
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
        private const string TemplateFilePass = "Assets/PutiJin/Common/Audio/BGM/BGMType_Template.txt";
        private const string CsFilePass = "Assets/PutiJin/Common/Audio/BGM/BGMType.cs";
        private const string StartOfDefine = "// START_OF_DEFINE";
        private ReorderableList _bgmList;

        private void OnEnable()
        {
            _bgmList = new ReorderableList(serializedObject, serializedObject.FindProperty("_bgmList"));
            
            _bgmList.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                var element = _bgmList.serializedProperty.GetArrayElementAtIndex(index);
                EditorGUI.PropertyField(rect, element);
            };
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            _bgmList.DoLayoutList();
            if (GUILayout.Button("リストを更新"))
            {
                RefreshType();
            }
            
            serializedObject.ApplyModifiedProperties();
        }

        private void RefreshType()
        {
            TextAsset bgmListTextAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(TemplateFilePass);
            StringBuilder audioNameBuilder = new();
            
            for (int i = 0; i < _bgmList.serializedProperty.arraySize; i++)
            {
                SerializedProperty element = _bgmList.serializedProperty.GetArrayElementAtIndex(i);
                
                audioNameBuilder.Append(element.objectReferenceValue.name);
                audioNameBuilder.Append(",\r        ");
            }

            string replacedText = bgmListTextAsset.text
                .Replace(StartOfDefine, audioNameBuilder.ToString());

            File.WriteAllText(CsFilePass, replacedText);
            AssetDatabase.Refresh();
        }
    }
}