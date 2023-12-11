using System;
using UniRx;
using UnityEngine;

namespace PutinuLib.CommonViewParts.RadioButton
{
    /// <summary>
    /// 複数のボタンから１つだけを選択できるもの（ラジオボタン）
    /// </summary>
    public class CustomRadioButton : MonoBehaviour
    {
        [Header("それぞれの選択肢ボタン")]
        [SerializeField] private CustomSelectableButton[] _buttons = Array.Empty<CustomSelectableButton>();

        private int _beforeSelectedButtonIndex;
        
        /// <summary>
        /// ボタンがアクティブ状態になった時に対象indexを返す
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
        
        /// <summary>
        /// 対象のボタンが選択されているかどうか
        /// </summary>
        public bool GetIsSelected(int targetIndex)
        {
            return _buttons[targetIndex].IsSelectedRP.Value;
        }
        
        /// <summary>
        /// 現在選択されているボタンのindexを取得する
        /// </summary>
        public int GetSelectedButtonIndex()
        {
            return _beforeSelectedButtonIndex;
        }

        private void SetEvent()
        {
            for (int i = 0; i < _buttons.Length; i++)
            {
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
            _buttonActivatedSubject?.OnNext(targetIndex);
        }

        private void Deactivate(int targetIndex)
        {
            _buttons[targetIndex].SetSelected(false);
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
