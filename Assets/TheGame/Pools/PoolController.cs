using System.Collections.Generic;
using UnityEngine;
using System;

namespace TheGame
{
    public interface IPoolController
    {
        void InitPool(Enum name, IPoolable prefab, Transform defaultParent);
        void InitPool(Enum name, GameObject prefab, Transform defaultParent);
        IPoolable GetObject(Enum name, Transform parent = null);
        IPoolable GetObject(Enum name, Vector2 position, Transform parent = null);
        IPoolable GetObject(Enum name, Vector2 position, Quaternion rotation, Transform parent = null);
        void PutObject(IPoolable poolable);
        void ReleasePool(Enum name);
        void Clear();
    }

    public class PoolController : IPoolController
    {
        private readonly Dictionary<Enum, Pool> _pools = new();
        private readonly HashSet<Enum> _names = new();

        public void InitPool(Enum name, IPoolable prefab, Transform defaultParent)
        {
            if (!_names.Contains(name))
            {
                var pool = new Pool(prefab, defaultParent);
                _pools.Add(name, pool);
                _names.Add(name);
            }
        }

        public void InitPool(Enum name, GameObject prefab, Transform defaultParent)
        {
            if (prefab.TryGetComponent(out IPoolable poolable))
            {
                InitPool(name, poolable, defaultParent);
            }
            else
            {
                throw new NullReferenceException(
                    string.Format("{0} game object do not contains component inherited form IPoolable)", nameof(prefab)));
            }
        }

        public IPoolable GetObject(Enum name, Transform parent = null)
        {
            return GetObject(name, Vector2.zero, Quaternion.identity, parent);
        }

        public IPoolable GetObject(Enum name, Vector2 position, Transform parent = null)
        {
            return GetObject(name, position, Quaternion.identity, parent);
        }

        public IPoolable GetObject(Enum name, Vector2 position, Quaternion rotation, Transform parent = null)
        {
            if (!_pools.ContainsKey(name))
            {
                throw new System.ArgumentException(
                    string.Format("Pool of {0} does'n exist. Use InitPool() instead before GetObject() call.", name));
            }

            var poolable = _pools[name].Get(position, rotation, parent);
            return poolable;
        }

        public void PutObject(IPoolable poolable)
        {
            foreach (var pool in _pools.Values)
            {
                if (pool.Contains(poolable))
                {
                    pool.Store(poolable);
                    break;
                }
            }
        }

        public void ReleasePool(Enum name)
        {
            if (_names.Contains(name))
            {
                _pools[name].Clear();
                _pools.Remove(name);
                _names.Remove(name);
            }
        }

        public void Clear()
        {
            foreach (var pool in _pools.Keys)
            {
                ReleasePool(pool);
            }
        }
    }

    public interface ISettings
    {
        Enum Name { get; }
        IIdentifiers Identifiers { get; }
        Vector2 Position { get; }
        Quaternion Rotation { get; }
        IPoolable Referance { get; }
    }

    public class PoolableSettings : ISettings
    {
        public Enum Name { get; }
        public IIdentifiers Identifiers { get; }
        public Vector2 Position { get; }
        public Quaternion Rotation { get; }
        public IPoolable Referance { get; }


        public PoolableSettings(
            Enum name,
            IIdentifiers identifiers,
            Vector2 position,
            Quaternion rotation,
            IPoolable referance)
        {
            Name = name;
            Position = position;
            Rotation = rotation;
            Identifiers = identifiers;
            Referance = referance;
        }
    }

}

