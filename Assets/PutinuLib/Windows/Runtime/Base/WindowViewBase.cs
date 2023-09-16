using UnityEngine;

namespace PutinuLib.Windows
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class WindowViewBase : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;

        protected CanvasGroup CanvasGroup => _canvasGroup;

        public void InitializeBase()
        {
        }

        /// <summary>
        /// ウィンドウを開ける
        /// </summary>
        public virtual void OpenWindow()
        {
            _canvasGroup.alpha = 1;
        }

        /// <summary>
        /// ウィンドウを閉じる
        /// </summary>
        public virtual void CloseWindow()
        {
            _canvasGroup.alpha = 0;
        }

#if UNITY_EDITOR
        protected void OnValidate()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }
#endif
    }
}