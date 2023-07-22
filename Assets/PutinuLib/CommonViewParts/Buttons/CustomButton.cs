using System;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PutinuLib.CommonView
{
    /// <summary>
    /// ボタン
    /// </summary>
    public class CustomButton : UIMonoBehaviour, IPointerClickHandler,
        IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        /// <summary>
        /// ボタンクリック時
        /// </summary>
        public IObservable<Unit> OnButtonClicked => _buttonClickedSubject;
        
        /// <summary>
        /// ボタンを押した時
        /// </summary>
        public IObservable<Unit> OnButtonPressed => _buttonPressedSubject;
        
        /// <summary>
        /// ボタンを離した時
        /// </summary>
        public IObservable<Unit> OnButtonReleased => _buttonReleasedSubject;
        
        /// <summary>
        /// ボタンの領域にカーソルが入った時
        /// </summary>
        public IObservable<Unit> OnButtonEntered => _buttonEnteredSubject;
        
        /// <summary>
        /// ボタンの領域からカーソルが出た時
        /// </summary>
        public IObservable<Unit> OnButtonExited => _buttonExitedSubject;

        /// <summary>
        /// ボタンのアクティブ状態を持つReactiveProperty
        /// </summary>
        public IReadOnlyReactiveProperty<bool> IsActiveRP => _isActiveRP;

        private readonly Subject<Unit> _buttonClickedSubject = new();
        private readonly Subject<Unit> _buttonPressedSubject = new();
        private readonly Subject<Unit> _buttonReleasedSubject = new();
        private readonly Subject<Unit> _buttonEnteredSubject = new();
        private readonly Subject<Unit> _buttonExitedSubject = new();
        
        [Header("ボタンのアクティブ状態")]
        [SerializeField] private ReactiveProperty<bool> _isActiveRP = new(true);

        protected virtual void OnDestroy()
        {
            _buttonClickedSubject.Dispose();
            _buttonPressedSubject.Dispose();
            _buttonReleasedSubject.Dispose();
            _buttonEnteredSubject.Dispose();
            _buttonExitedSubject.Dispose();
            _isActiveRP.Dispose();
        }

        /// <summary>
        /// ボタンのアクティブ状態を取得する
        /// </summary>
        public bool IsActive() => _isActiveRP.Value;

        /// <summary>
        /// アクティブ状態を変更する
        /// </summary>
        public void SetActive(bool isActive)
        {
            _isActiveRP.Value = isActive;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _buttonClickedSubject?.OnNext(Unit.Default);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _buttonEnteredSubject?.OnNext(Unit.Default);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _buttonExitedSubject?.OnNext(Unit.Default);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _buttonPressedSubject?.OnNext(Unit.Default);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _buttonReleasedSubject?.OnNext(Unit.Default);
        }
    }
}
