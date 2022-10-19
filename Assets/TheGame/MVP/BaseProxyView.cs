using System;
using UnityEngine;

namespace TheGame.MVP
{
    public interface IProxyView<TView> where TView : IView
    {
        TView View { get; }
        bool IsPrepared { get; }
        void Prepare(System.Action onComplete = null);
    }

    public abstract class BaseProxyView<TView> : IProxyView<TView> where TView : IView
    {
        public enum ProxyState
        {
            None,
            InProgress,
            Prepared
        }
        protected ProxyState _proxyState;

        public bool IsPrepared => View != null;
        public ProxyState State => _proxyState;
        public TView View { get; protected set; }


        public abstract void Prepare(System.Action onComplete = null);

        public BaseProxyView()
        {
            
        }

        public virtual void Release()
        {
            _proxyState = ProxyState.None;
        }
    }


    public interface IPlayerManager
    {

    }

    public class PlayerProxyView : BaseProxyView<IPlayerView>
    {
        private IPlayerManager _manager;
        public PlayerProxyView(IPlayerManager manager) : base()
        {
            _manager = manager;
        }

        public override void Prepare(Action onComplete = null)
        {
            throw new NotImplementedException();
        }
    }
}


