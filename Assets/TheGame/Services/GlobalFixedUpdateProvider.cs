using UnityEngine;

namespace TheGame
{
    public class GlobalFixedUpdateProvider : MonoBehaviour, IGlobalFixedUpdateProvider
    {
        private System.Action OnFixedUpdate;

        public void Add(IGlobalFixedUpdateObserver observer)
        {
            OnFixedUpdate += observer.FixedUpdateGlobal;
        }

        public void Remove(IGlobalFixedUpdateObserver observer)
        {
            OnFixedUpdate -= observer.FixedUpdateGlobal;
        }

        private void FixedUpdate()
        {
            OnFixedUpdate?.Invoke();
        }
    }

    public interface IGlobalFixedUpdateObserver
    {
        void FixedUpdateGlobal();
    }

    public interface IGlobalFixedUpdateProvider
    {
        void Add(IGlobalFixedUpdateObserver observer);
        void Remove(IGlobalFixedUpdateObserver observer);
    }
}

