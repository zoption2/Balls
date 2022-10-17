using System.Collections.Generic;
using UnityEngine;

namespace TheGame
{
    public class Pool
    {
        private IPoolable _referance;
        private Stack<IPoolable> _pool = new();
        private HashSet<IPoolable> _allPoolables = new();
        private Transform _defaultParent;

        public Pool(IPoolable referance, Transform defaultParent)
        {
            _referance = referance;
            _defaultParent = defaultParent;
        }

        public IPoolable Get(Vector3 position, Quaternion rotation, Transform parent = null)
        {
            var currentParent = parent == null ? _defaultParent : parent;
            IPoolable result;
            if (_pool.Count > 0)
            {
                result = _pool.Pop();
                result.transform.SetPositionAndRotation(position, rotation);
                result.transform.SetParent(currentParent);
                result.gameObject.SetActive(true);
                result.OnRestore();
            }
            else
            {
                var go = GameObject.Instantiate(_referance.gameObject, position, rotation, currentParent);
                result = go.GetComponent<IPoolable>();
                _allPoolables.Add(result);
                result.OnCreate();
            }
            return result;
        }

        public bool Contains(IPoolable poolable)
        {
            return _allPoolables.Contains(poolable) ? true : false;
        }

        public void Store(IPoolable poolable)
        {
            poolable.OnStore();
            poolable.gameObject.SetActive(false);
            poolable.transform.SetParent(_defaultParent);
            _pool.Push(poolable);
        }

        public void Clear()
        {
            foreach (var poolable in _allPoolables)
            {
                GameObject.Destroy(poolable.gameObject);
            }
            _pool.Clear();
            _allPoolables.Clear();
        }
    }
}

