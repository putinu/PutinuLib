using System;
using PutinuLib.CommonViewParts;
using UniRx;
using UnityEngine;

namespace PutinuLib.CommonView
{
    /// <summary>
    /// 複数のボタンから１つだけを選択できるもの（ラジオボタン）
    /// </summary>
    public class CustomRadioButton : UIMonoBehaviour
    {
        [Header("それぞれの選択肢ボタン")]
        [SerializeField] private CustomSelectableButton[] _buttons;

        private int _beforeSelectedButtonIndex;
        
        /// <summary>
        /// ボタンがアクティブ状態になった時にindexを返す
        /// </summary>
        public IObservable<int> OnButtonActivated => _buttonActivatedSubject;
        private readonly Subject<int> _buttonActivatedSubject = new();

        private void OnDestroy()
        {
            _buttonActivatedSubject.Dispose();
        }

        /// <summary>
        /// 初期化処理を実行する
        /// </summary>
        public void Initialize(int startIndex)
        {
            if (startIndex >= _buttons.Length)
            {
                throw new IndexOutOfRangeException();
            }
            
            Activate(startIndex);
            _beforeSelectedButtonIndex = startIndex;
            
            SetEvent();
        }

        /// <summary>
        /// スクリプトからボタン情報を設定するときに使用する初期化処理
        /// </summary>
        public void Initialize(CustomSelectableButton[] buttons, int startIndex)
        {
            _buttons = buttons;
            Initialize(startIndex);
        }

        private void SetEvent()
        {
            for (int i = 0; i < _buttons.Length; i++)
            {
                // for文内でindexとして再定義しないと、イベント実行時のiの値がオーバーするため
                int index = i;
                _buttons[index].OnButtonClicked
                    .Subscribe(_ => OnAnyButtonSelected(index))
                    .AddTo(this.gameObject);
            }
        }

        private void OnAnyButtonSelected(int selectedIndex)
        {
            Deactivate(_beforeSelectedButtonIndex);
            Activate(selectedIndex);
            _beforeSelectedButtonIndex = selectedIndex;
        }

        private void Activate(int targetIndex)
        {
            _buttons[targetIndex].SetSelected(true);
        }

        private void Deactivate(int targetIndex)
        {
            _buttons[targetIndex].SetSelected(false);
        }

        /// <summary>
        /// 対象のボタンが選択されているかどうか
        /// </summary>
        public bool IsButtonSelected(int targetIndex)
        {
            return _buttons[targetIndex].IsSelectedRP.Value;
        }
    }
}