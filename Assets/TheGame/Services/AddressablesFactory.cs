using UnityEngine;
using System;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.ResourceManagement.AsyncOperations;


namespace TheGame
{
    public class AddressablesFactory<T1, T2> : MonoBehaviour, IAddressableFactory<T1, T2> where T1 : UnityEngine.Object where T2 : Enum
    {
        [SerializeField] private Data<T2>[] data;
        private Dictionary<T2, T1> loaded = new Dictionary<T2, T1>();


        public async Task<T1> GetPrefab(T2 type)
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
                        if (opHandle.Result.TryGetComponent(out T1 product))
                        {
                            if (!loaded.ContainsKey(type))
                            {
                                loaded.Add(type, product);
                            }
                            return loaded[type];
                        }
                        else
                        {
                            throw new MissingComponentException();
                        }
                    }
                }
            }

            throw new System.ArgumentNullException();
        }
 

        [System.Serializable]
        public class Data<T3> where T3 : T2
        {
            [field: SerializeField] public T3 Type;
            [field: SerializeField] public AssetReferenceGameObject referance { get; private set; }
        }
    }

    public interface IAddressableFactory<T1, T2> where T1 : UnityEngine.Object where T2 : Enum
    {
        Task<T1> GetPrefab(T2 type);
    }
}

