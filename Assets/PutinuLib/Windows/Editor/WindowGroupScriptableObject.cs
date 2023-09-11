using System;
using System.Collections.Generic;
using UnityEngine;

namespace PutinuLib.Windows.Editor
{
    [CreateAssetMenu(menuName = "PutinuLib/Windows/WindowGroup")]
    public class WindowGroupScriptableObject : ScriptableObject
    {
        // SERIALIZE_FIELD_END

        public TWindow GetWindowPrefab<TWindow>(WindowType windowType)
            where TWindow : WindowPresenterBase
        {
            return windowType switch
            {
                WindowType.None => null,
                // SWITCH_TYPE_END
                _ => throw new ArgumentOutOfRangeException(nameof(windowType), windowType, null)
            };
        }

        private Dictionary<Type, WindowType> WindowDictionary = new Dictionary<Type, WindowType>()
        {
            // WINDOW_DICTIONARY_END
        };

        public TWindow GetWindowPrefab<TWindow>(Type type)
            where TWindow : WindowPresenterBase
        {
            return GetWindowPrefab<TWindow>(WindowDictionary[type]);
        }
    }

    public enum WindowType
    {
        None = 0,
        // WINDOW_TYPE_END
    }
}