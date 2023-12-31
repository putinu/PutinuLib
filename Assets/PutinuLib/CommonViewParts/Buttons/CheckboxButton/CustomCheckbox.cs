﻿using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace PutinuLib.CommonViewParts
{
    /// <summary>
    /// 複数のボタンから複数選択できるもの（チェックボックス）
    /// </summary>
    public class CustomCheckbox : MonoBehaviour
    {
        [Header("それぞれの選択肢ボタン")]
        [SerializeField] private CustomSelectableButton[] _buttons = Array.Empty<CustomSelectableButton>();

        private bool[] _activeIndexArray;

        /// <summary>
        /// ボタンがアクティブ状態になった時に対象indexを返す
        /// </summary>
        public IObservable<int> OnButtonActivated => _buttonActivatedSubject;
        
        /// <summary>
        /// ボタンが非アクティブ状態になった時に対象indexを返す
        /// </summary>
        public IObservable<int> OnButtonDeactivated => _buttonDeactivatedSubject;
        
        private readonly Subject<int> _buttonActivatedSubject = new();
        private readonly Subject<int> _buttonDeactivatedSubject = new();

        private void OnDestroy()
        {
            _buttonActivatedSubject.Dispose();
            _buttonDeactivatedSubject.Dispose();
        }

        /// <summary>
        /// 初期化処理を実行する
        /// </summary>
        public void Initialize()
        {
            _activeIndexArray = new bool[_buttons.Length];
            SetEvent();
        }

        /// <summary>
        /// スクリプトからボタン情報を設定するときに使用する初期化処理
        /// </summary>
        public void Initialize(CustomSelectableButton[] buttons)
        {
            if (buttons.Length == 0)
            {
                throw new ArgumentOutOfRangeException();
            }
        
            _buttons = buttons;
            Initialize();
        }

        /// <summary>
        /// 対象のボタンのアクティブ状態をセットする
        /// </summary>
        public void SetButtonActive(int targetIndex, bool isActive)
        {
            _activeIndexArray[targetIndex] = isActive;
            _buttons[targetIndex].SetSelected(isActive);

            if (isActive)
            {
                _buttonActivatedSubject?.OnNext(targetIndex);
            }
            else
            {
                _buttonDeactivatedSubject?.OnNext(targetIndex);
            }
        }
        
        /// <summary>
        /// それぞれの選択肢が選択されているかのリストを返す
        /// </summary>
        public IReadOnlyList<bool> GetIsButtonSelectedList()
        {
            return Array.AsReadOnly(_activeIndexArray);
        }

        /// <summary>
        /// 対象のボタンが選択されているかどうか
        /// </summary>
        public bool GetIsSelected(int targetIndex)
        {
            return _activeIndexArray[targetIndex];
        }

        private void SetEvent()
        {
            for (int i = 0; i < _buttons.Length; i++)
            {
                int index = i;
                _buttons[index].OnButtonClicked
                    .Subscribe(_ => SwitchButtonActive(index))
                    .AddTo(this.gameObject);
            }
        }

        private void SwitchButtonActive(int targetIndex)
        {
            SetButtonActive(targetIndex, !_activeIndexArray[targetIndex]);
        }
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_buttons.Length == 0)
            {
                _buttons = transform.GetComponentsInChildren<CustomSelectableButton>();
            }
        }
#endif
    }
}
