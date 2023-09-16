using PutinuLib.Windows.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;
using File = System.IO.File;

namespace PutinuLib.Windows.Editor
{
    /// <summary>
    /// ウィンドウを生成するエディタ拡張
    /// </summary>
    public class WindowGenerator : EditorWindow
    {
        [SerializeField] private PermanentWindowGeneratorData _permanentWindowGeneratorData;
        
        private string _windowName;
        private string _windowSmallName;
        private string _description;
        private WindowGroupScriptableObject _windowGroupData;
        private string _baseFilePass;
        private DefaultAsset _baseFilePassFolder;

        [MenuItem("PutinuLib/Window/WindowGenerator")]
        private static void OpenWindowGenerator()
        {
            var window = GetWindow<WindowGenerator>();
            window.titleContent = new GUIContent("WindowGenerator");
        }

        /// <summary>
        /// ウィンドウの中に表示する内容
        /// </summary>
        private void OnGUI()
        {
            if (_permanentWindowGeneratorData == null)
            {
                _permanentWindowGeneratorData = LoadPermanentData();
            }

            _windowName = EditorGUILayout.TextField("ウィンドウ名", _windowName);
            _windowSmallName = EditorGUILayout.TextField("private変数名(_abc)", _windowSmallName);
            _description = EditorGUILayout.TextField("説明", _description);
            
            // 以下２つのフィールドはPermanentWindowGeneratorDataから読み込み、変更があれば保存される
            EditorGUI.BeginChangeCheck();
            
            _windowGroupData = _permanentWindowGeneratorData.WindowGroupData;
            _baseFilePassFolder = _permanentWindowGeneratorData.BaseFilePassFolder;
            
            _windowGroupData = (WindowGroupScriptableObject) EditorGUILayout.ObjectField(
                "WindowGroupData", _windowGroupData, typeof(WindowGroupScriptableObject), false);
            _baseFilePassFolder = (DefaultAsset) EditorGUILayout.ObjectField
                ("格納する親ディレクトリ", _baseFilePassFolder, typeof(DefaultAsset), false);
            
            if (EditorGUI.EndChangeCheck())
            {
                _permanentWindowGeneratorData.WindowGroupData = _windowGroupData;
                _permanentWindowGeneratorData.BaseFilePassFolder = _baseFilePassFolder;
                EditorUtility.SetDirty(_permanentWindowGeneratorData);
            }
            
            _baseFilePass = AssetDatabase.GetAssetPath(_baseFilePassFolder);
            EditorGUILayout.LabelField("格納先", _baseFilePass, EditorStyles.wordWrappedLabel);
            
            bool canGenerate = _baseFilePassFolder != null && !string.IsNullOrEmpty(_windowName)
                && !string.IsNullOrEmpty(_windowSmallName) && _windowGroupData != null;
            string buttonText = canGenerate ? "ウィンドウ作成" : "入力が不十分です";
            using (new EditorGUI.DisabledScope(!canGenerate))
            {
                if (!GUILayout.Button(buttonText)) return;
                
                Generate();
                AddWindowGroup(); 
                AssetDatabase.Refresh();
            }
        }
        
        /// <summary>
        /// データをロードする
        /// </summary>
        private PermanentWindowGeneratorData LoadPermanentData()
        {
            return AssetDatabase.LoadAssetAtPath<PermanentWindowGeneratorData>
                ("Assets/PutinuLib/Windows/Editor/PermanentWindowGeneratorData.asset");
        }

        private const string TemplatePath = "Assets/PutinuLib/Windows/Editor/TemplateAssets";
        private const string TemplateTitleKey = "_TEMPLATE_TITLE";
        private const string TemplateDescriptionKey = "_TEMPLATE_DESCRIPTION";

        private void Generate()
        {
            var filePath = $"{_baseFilePass}/{_windowName}";
            Directory.CreateDirectory(filePath);

            var presenterFilePath = $"{filePath}/Window{_windowName}Presenter.cs";
            var modelFilePath = $"{filePath}/Window{_windowName}Model.cs";
            var viewFilePath = $"{filePath}/Window{_windowName}View.cs";

            var presenterTemplatePath = $"{TemplatePath}/Template_WindowPresenter.txt";
            var modelTemplatePath = $"{TemplatePath}/Template_WindowModel.txt";
            var viewTemplatePath = $"{TemplatePath}/Template_WindowView.txt";

            var presenterTextAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(presenterTemplatePath);
            var modelTextAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(modelTemplatePath);
            var viewTextAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(viewTemplatePath);
            
            var presenterReplacedText = ReplaceFromText(presenterTextAsset);
            var modelReplacedText = ReplaceFromText(modelTextAsset);
            var viewReplacedText = ReplaceFromText(viewTextAsset);
            
            File.WriteAllText(presenterFilePath, presenterReplacedText);
            File.WriteAllText(modelFilePath, modelReplacedText);
            File.WriteAllText(viewFilePath, viewReplacedText);
        }

        private string ReplaceFromText(TextAsset textAsset)
        {
            return textAsset.text
                .Replace(TemplateTitleKey, _windowName)
                .Replace(TemplateDescriptionKey, _description);
        }

        private const string EndOfSerializeField = "// SERIALIZE_FIELD_END";
        private const string EndOfSwitchType = "// SWITCH_TYPE_END";
        private const string EndOfWindowDictionary = "// WINDOW_DICTIONARY_END";
        private const string EndOfWindowType = "// WINDOW_TYPE_END";

        private void AddWindowGroup()
        {
            var filePath = $"{_baseFilePass}/WindowGroupScriptableObject.cs";
            var windowGroupTextAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(filePath);

            if (windowGroupTextAsset.text.Contains(_windowName)) return;

            var serializeFieldText = $"[SerializeField] private Window{_windowName}Presenter {_windowSmallName};";
            var switchTypeText = $"WindowType.{_windowName} => {_windowSmallName} as TWindow,"; //4tab
            var windowDictionaryText = $"{{typeof(Window{_windowName}Presenter), WindowType.{_windowName}}},";
            var windowTypeText = $"{_windowName},";

            var replacedText = windowGroupTextAsset.text
                .Replace(EndOfSerializeField, $"{serializeFieldText}\n        {EndOfSerializeField}")
                .Replace(EndOfSwitchType, $"{switchTypeText}\n                {EndOfSwitchType}")
                .Replace(EndOfWindowDictionary, $"{windowDictionaryText}\n            {EndOfWindowDictionary}")
                .Replace(EndOfWindowType, $"{windowTypeText}\n        {EndOfWindowType}");
            
            File.WriteAllText(filePath, replacedText);
        }
    }
}