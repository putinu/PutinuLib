using DG.Tweening;
using UnityEngine;

namespace PutinuLib.Windows.Runtime
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class WindowViewBase : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;

        protected CanvasGroup CanvasGroup => _canvasGroup;
        protected bool CanInput { get; private set; }
        
        protected const float TransitionTime = 0.4f;

        public void InitializeBase()
        {
        }

        /// <summary>
        /// ウィンドウを開ける
        /// </summary>
        public void OpenWindow()
        {
            CanInput = false;

            // 開ける処理をカスタマイズしたければここをいじる
            _canvasGroup.alpha = 0;
            _canvasGroup.DOFade(1, TransitionTime)
                .OnComplete(() => CanInput = true);
        }

        /// <summary>
        /// ウィンドウを閉じる
        /// </summary>
        public void CloseWindow()
        {
            CanInput = false;
            
            // 閉じる処理をカスタマイズしたければここをいじる
            _canvasGroup.DOFade(0, TransitionTime)
                .OnComplete(() =>
                {
                    CanInput = true;
                    Destroy(this.gameObject);
                });
        }

#if UNITY_EDITOR
        protected void OnValidate()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }
#endif
    }
}