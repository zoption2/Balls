using UnityEngine;
using System.Collections.Generic;

namespace TheGame
{
    public interface IPoolable
    {
        GameObject gameObject { get; }
        Transform transform { get; }
        void OnCreate();
        void OnRestore();
        void OnStore();
    }


    public interface IPoolableBalls : IPoolable
    {
        void Launch(IIdentifiers identifiers, Vector2 direction);
    }

    public interface IPoolableEnemy : IPoolable
    {
        void Initialize(float health);
    }
}

