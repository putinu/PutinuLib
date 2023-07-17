using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace PutinuLib.CommonView
{
    /// <summary>
    /// 複数のボタンから複数選択できるもの（チェックボックス）
    /// </summary>
    public class CustomCheckboxButton : UIMonoBehaviour
    {
        [Header("それぞれの選択肢ボタン")]
        [SerializeField] private CustomSelectableButton[] _buttons;

        private bool[] _activeIndexArray;

        /// <summary>
        /// ボタンがアクティブ状態になった時にindexを返す
        /// </summary>
        public IObservable<int> OnButtonActivated => _buttonActivatedSubject;
        
        /// <summary>
        /// ボタンが非アクティブ状態になった時にindexを返す
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

        private void SetEvent()
        {
            for (int i = 0; i < _buttons.Length; i++)
            {
                // for文内でindexとして再定義しないと、イベント実行時のiの値がオーバーするため
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
        public bool IsButtonSelected(int targetIndex)
        {
            return _activeIndexArray[targetIndex];
        }
    }
}