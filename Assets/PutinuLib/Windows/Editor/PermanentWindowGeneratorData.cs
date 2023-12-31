﻿using UnityEditor;
using UnityEngine;

namespace PutinuLib.Windows.Editor
{
    /// <summary>
    /// WindowGeneratorのEditorWindowのデータを保持するためのScriptableObject
    /// </summary>
    [CreateAssetMenu(menuName = "PutinuLib/Windows/Editor/PermanentWindowGeneratorData")]
    public class PermanentWindowGeneratorData : ScriptableObject
    {
        [SerializeField] private string _windowNamespace = "App";
        [SerializeField] private DefaultAsset _baseFilePassFolder;

        public string WindowNamespace
        {
            get => _windowNamespace;
            set => _windowNamespace = value;
        }

        public DefaultAsset BaseFilePassFolder
        {
            get => _baseFilePassFolder;
            set => _baseFilePassFolder = value;
        }
    }
}