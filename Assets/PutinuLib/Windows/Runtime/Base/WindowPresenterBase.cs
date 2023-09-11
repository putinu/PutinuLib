using UnityEngine;

namespace PutinuLib.Windows.Runtime
{
    /// <summary>
    /// めちゃくちゃ苦肉の策（クソコードなのでどうにかしたい）
    /// WindowServiceでそれぞれのPresenterを同じタイプのものとして扱うための
    /// </summary>
    public abstract class WindowPresenterBase : MonoBehaviour
    {
        public abstract void InitializeBase();
        public abstract void Open();
        public abstract void Close();
    }

    public abstract class WindowPresenterDtoBase
    {
    }
    
    public abstract class WindowPresenterBase<TModel, TView> : WindowPresenterBase
        where TModel : WindowModelBase, new()
        where TView : WindowViewBase
    {
        protected TModel Model { get; private set; }

        protected TView View { get; private set; }
        
        public override void InitializeBase()
        {
            Model = new TModel();
            View = GetComponent<TView>();
            View.InitializeBase(); ;
        }
        
        protected virtual void OnDestroy()
        {
            Model.Dispose();
        }

        /// <summary>
        /// 開ける
        /// </summary>
        public override void Open()
        {
            View.OpenWindow();
        }

        /// <summary>
        /// 閉じる
        /// </summary>
        public override void Close()
        {
            View.CloseWindow();
        }
    }
}