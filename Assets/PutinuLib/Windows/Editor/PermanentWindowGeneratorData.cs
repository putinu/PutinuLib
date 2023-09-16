using PutinuLib.Windows.Runtime;
using UnityEditor;
using UnityEngine;

namespace PutinuLib.Windows.Editor
{
    /// <summary>
    /// WindowGeneratorのEditorWindowのデータを保持するためのScriptableObject
    /// </summary>
    [CreateAssetMenu(menuName = "PutinuLib/Windows/Editor/PermanentWindowGeneratorData")]
    public class PermanentWindowGeneratorData : ScriptableObject
    {
        [SerializeField] private WindowGroupScriptableObject _windowGroupData;
        [SerializeField] private DefaultAsset _baseFilePassFolder;

        public WindowGroupScriptableObject WindowGroupData
        {
            get => _windowGroupData;
            set => _windowGroupData = value;
        }

        public DefaultAsset BaseFilePassFolder
        {
            get => _baseFilePassFolder;
            set => _baseFilePassFolder = value;
        }
    }
}