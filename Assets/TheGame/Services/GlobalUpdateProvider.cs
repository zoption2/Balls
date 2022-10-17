using UnityEngine;

namespace TheGame
{
    public class GlobalUpdateProvider : MonoBehaviour, IGlobalUpdateProvider
    {
        private System.Action OnUpdate;

        public void Add(IGlobalUpdateObserver observer)
        {
            OnUpdate += observer.UpdateGlobal;
        }

        public void Remove(IGlobalUpdateObserver observer)
        {
            OnUpdate -= observer.UpdateGlobal;
        }

        private void Update()
        {
            OnUpdate?.Invoke();
        }
    }

    public interface IGlobalUpdateObserver
    {
        void UpdateGlobal();
    }

    public interface IGlobalUpdateProvider
    {
        void Add(IGlobalUpdateObserver observer);
        void Remove(IGlobalUpdateObserver observer);
    }
}

