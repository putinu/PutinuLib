using System;
using UniRx;
using UnityEngine;

namespace PutinuLib.CommonView
{
    /// <summary>
    /// ラジオボタンの各要素クラス
    /// </summary>
    public class CustomRadioButtonElement : CustomButton
    {
        /// <summary>
        /// 選択されたとき
        /// </summary>
        public IObservable<Unit> OnSelected => _selectedSubject;
        
        /// <summary>
        /// 選択が解除されたとき
        /// </summary>
        public IObservable<Unit> OnUnselected => _unselectedSubject;
        
        private readonly Subject<Unit> _selectedSubject = new();
        private readonly Subject<Unit> _unselectedSubject = new();

        private bool _isSelected;

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _selectedSubject.Dispose();
        }

        /// <summary>
        /// 選択状態をセットする
        /// </summary>
        public void SetSelected(bool isSelected)
        {
            if (isSelected == _isSelected) return;

            if (isSelected)
            {
                _selectedSubject?.OnNext(Unit.Default);
            }
            else
            {
                _unselectedSubject?.OnNext(Unit.Default);
            }

            _isSelected = isSelected;
        }
    }
}