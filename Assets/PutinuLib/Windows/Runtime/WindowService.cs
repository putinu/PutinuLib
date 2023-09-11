using System;
using System.Collections.Generic;
using UnityEngine;

namespace PutinuLib.Windows.Runtime
{
    /// <summary>
    /// ウィンドウ周りのサービスを提供してくれる
    /// </summary>
    public class WindowService : MonoBehaviour
    {
        [SerializeField] private Transform _root;
        [SerializeField] private WindowGroupScriptableObject _windowGroup;

        private readonly Stack<WindowPresenterBase> _windowStack = new();

        /// <summary>
        /// ウィンドウを開く
        /// </summary>
        /// <param name="presenterAction">対象のウィンドウに対して開くときに与えたいアクション</param>
        /// <param name="token">CancellationToken.</param>
        /// <typeparam name="TWindow">ウィンドウの種類</typeparam>
        public void OpenWindow<TWindow>(Action<TWindow> presenterAction)
            where TWindow : WindowPresenterBase
        {
            var windowPrefab = _windowGroup.GetWindowPrefab<TWindow>(typeof(TWindow));
            var instanceWindow = Instantiate(windowPrefab, _root, false);

            instanceWindow.InitializeBase();
            instanceWindow.Open();
            presenterAction?.Invoke(instanceWindow);
            
            _windowStack.Push(instanceWindow);
        }

        /// <summary>
        /// １つ戻る
        /// </summary>
        public void BackWindow()
        {
            var instanceWindow = _windowStack.Pop();
            instanceWindow.Close();
        }

        /// <summary>
        /// ウィンドウをすべて閉じる（特殊処理）
        /// </summary>
        public void ClearWindowAll()
        {
            foreach (var instanceWindow in _windowStack)
            {
                instanceWindow.Close();
            }
            
            _windowStack.Clear();
        }
    }
}