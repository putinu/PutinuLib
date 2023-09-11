using System;
using System.Collections.Generic;
using UnityEngine;

namespace PutinuLib.Windows.Runtime
{
    [CreateAssetMenu(menuName = "PutinuLib/Windows/WindowGroup")]
    public class WindowGroupScriptableObject : ScriptableObject
    {
        [SerializeField] private WindowHogePresenter _hoge;
        // SERIALIZE_FIELD_END

        public TWindow GetWindowPrefab<TWindow>(WindowType windowType)
            where TWindow : WindowPresenterBase
        {
            return windowType switch
            {
                WindowType.None => null,
                WindowType.Hoge => _hoge as TWindow,
                // SWITCH_TYPE_END
                _ => throw new ArgumentOutOfRangeException(nameof(windowType), windowType, null)
            };
        }

        private readonly Dictionary<Type, WindowType> _windowDictionary = new Dictionary<Type, WindowType>()
        {
            {typeof(WindowHogePresenter), WindowType.Hoge},
            // WINDOW_DICTIONARY_END
        };

        public TWindow GetWindowPrefab<TWindow>(Type type)
            where TWindow : WindowPresenterBase
        {
            return GetWindowPrefab<TWindow>(_windowDictionary[type]);
        }
    }

    public enum WindowType
    {
        None = 0,
        Hoge,
        // WINDOW_TYPE_END
    }
}