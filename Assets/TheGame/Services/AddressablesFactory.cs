using UnityEngine;
using System;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.ResourceManagement.AsyncOperations;


namespace TheGame
{
    public class AddressablesFactory<TObject, TType> : MonoBehaviour, IAddressableFactory<TObject, TType> where TObject : UnityEngine.Object where TType : Enum
    {
        [SerializeField] private Data<TType>[] data;
        private Dictionary<TType, TObject> loaded = new();

        public async Task<TObject> GetPrefab(TType type)
        {
            if (loaded.ContainsKey(type))
            {
                return loaded[type];
            }
            for (int i = 0, j = data.Length; i < j; i++)
            {
                if (data[i].Type.Equals(type))
                {
                    AsyncOperationHandle<GameObject> opHandle = Addressables.LoadAssetAsync<GameObject>(data[i].referance);
                    await opHandle.Task;
                    if (opHandle.Status == AsyncOperationStatus.Succeeded)
                    {
                        if (opHandle.Result.TryGetComponent(out TObject product))
                        {
                            loaded.Add(type, product);
                            return loaded[type];
                        }
                        else
                        {
                            throw new MissingComponentException(
                                string.Format("Component {0} at GameObject {1} is missing", typeof(TObject), nameof(opHandle.Result))
                                ) ;
                        }
                    }
                }
            }

            throw new System.ArgumentNullException();
        }

        public void ReleasePrefab(TType type)
        {
            if (loaded.ContainsKey(type))
            {
                Addressables.Release(loaded[type]);
                loaded.Remove(type);
            }
        }

        public void Clear()
        {
            foreach (var prefab in loaded.Values)
            {
                Addressables.Release(prefab);
            }
            loaded.Clear();
        }
 

        [System.Serializable]
        public class Data<TPreciseType> where TPreciseType : TType
        {
            [field: SerializeField] public TPreciseType Type;
            [field: SerializeField] public AssetReferenceGameObject referance { get; private set; }
        }
    }

    public interface IAddressableFactory<TObject, TType> where TObject : UnityEngine.Object where TType : Enum
    {
        Task<TObject> GetPrefab(TType type);
        void ReleasePrefab(TType type);
        void Clear();
    }
}

