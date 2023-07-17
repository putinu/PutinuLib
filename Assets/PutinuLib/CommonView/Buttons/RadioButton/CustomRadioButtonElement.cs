﻿using UniRx;

namespace PutinuLib.CommonView
{
    /// <summary>
    /// ラジオボタンの各要素クラス
    /// </summary>
    public class CustomRadioButtonElement : CustomButton
    {
        /// <summary>
        /// 選択状態かどうかを示すReactiveProperty
        /// </summary>
        public IReadOnlyReactiveProperty<bool> IsSelectedRP => _isSelectedRP;
        private readonly ReactiveProperty<bool> _isSelectedRP = new();

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _isSelectedRP.Dispose();
        }

        /// <summary>
        /// 選択状態をセットする
        /// </summary>
        public void SetSelected(bool isSelected)
        {
            _isSelectedRP.Value = isSelected;
        }
    }
}