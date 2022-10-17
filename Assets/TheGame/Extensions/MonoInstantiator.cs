using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace TheGame
{
    public interface IMonoInstantiator
    {
        GameObject CreateObject(IPoolable referance, Vector2 position, Quaternion rotation, Transform parent);
        GameObject CreateObject(GameObject prefab, Transform parent);
        GameObject CreateObject<T>(T prefab) where T : UnityEngine.Object;
        GameObject CreateObject<T>(T prefab, Vector2 position, Transform parent = null) where T : UnityEngine.Object;
        Coroutine StartCoroutine(IEnumerator routine);
    }

    public class MonoInstantiator : MonoBehaviour, IMonoInstantiator
    {
        [Inject] private DiContainer container;

        public GameObject CreateObject (IPoolable referance, Vector2 position, Quaternion rotation, Transform parent)
        {
            var newGO = container.InstantiatePrefab(referance.gameObject, position, rotation, parent);
            return newGO;
        }

        public GameObject CreateObject(GameObject prefab, Transform parent)
        {
            var newGO = container.InstantiatePrefab(prefab, parent.position, parent.rotation, parent);
            return newGO;
        }

        public GameObject CreateObject<T>(T prefab) where T : UnityEngine.Object
        {
            var newGO = container.InstantiatePrefab(prefab);
            return newGO;
        }

        public GameObject CreateObject<T>(T prefab, Vector2 position, Transform parent = null) where T : UnityEngine.Object
        {
            var newGO = container.InstantiatePrefab(prefab, position, Quaternion.identity, parent);
            return newGO;
        }
    }
}

