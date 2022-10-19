using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheGame.MVP
{
    public interface IView
    {
        void Show();
        void Hide();
        void Init();
    }

    public interface IPresenter : IView
    {

    }

    public interface IView<TPresenter> : IView where TPresenter : IPresenter
    {
        TPresenter Presenter { get; }
        void InitPresenter(TPresenter presenter);
    }

    public interface IModel 
    { 

    }

    public interface IPresenter<TModel, TView> : IPresenter
        where TModel : IModel
        where TView : IView
    {
        TModel Model { get; }
        TView View { get; }
    }

    public interface IPresenter<TModel, TProxyView, TView> : IPresenter
    where TModel : IModel
    where TProxyView : IProxyView<TView>
    where TView : IView
    {
        TModel Model { get; }
        TProxyView ProxyView { get; }
    }

    public abstract class BasePresenter : IPresenter
    {
        protected bool _isInited = false;
        public abstract void Hide();
        public abstract void Init();
        public abstract void Show();
        protected virtual void PrepareViewData() 
        {

        }
    }
}


