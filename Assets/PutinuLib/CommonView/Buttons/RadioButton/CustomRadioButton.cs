using System;
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
    }
}