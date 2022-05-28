using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace PassivePicasso.SimplyAddress
{
    [ExecuteAlways]
    public class AddressablePrefab : SimpleAddress
    {
        public static readonly Dictionary<string, GameObject> PrefabCache = new Dictionary<string, GameObject>();

        public bool replaceSelf;
        public UnityEvent OnInstantiated;

        [NonSerialized]
        public GameObject instance;
        private AsyncOperationHandle<GameObject> prefabLoadOperation;
        void Update()
        {
            if (instance && lastAddress == Address) return;
            if (string.IsNullOrEmpty(Address)) return;

            lastAddress = Address;
            if (PrefabCache.ContainsKey(Address) && PrefabCache[Address])
            {
                CreateInstance();
            }
            else if (!prefabLoadOperation.IsValid() || prefabLoadOperation.Status != AsyncOperationStatus.None)
            {
                prefabLoadOperation = Addressables.LoadAssetAsync<GameObject>(Address);
                prefabLoadOperation.Completed += OnCompleted;
                prefabLoadOperation.Destroyed += (AsyncOperationHandle _) => Debug.Log("Operation has been destroyed");
            }
        }
        private void OnCompleted(AsyncOperationHandle<GameObject> aOp)
        {
            if (aOp.Status == AsyncOperationStatus.Succeeded && aOp.Result)
            {
                PrefabCache[Address] = aOp.Result;
            }
        }

        private void CreateInstance()
        {
            DestroyChildren(transform);
            instance = Instantiate(PrefabCache[Address]);
            instance.hideFlags = HideFlags.DontSave;
            instance.transform.position = transform.position;
            instance.transform.rotation = transform.rotation;
            instance.transform.localScale = transform.localScale;
            if (Application.isPlaying && replaceSelf)
            {
                Destroy(gameObject);
            }
            else
                instance.transform.parent = transform;

            SetRecursiveFlags(instance.transform);
            OnInstantiated?.Invoke();
        }


        static void SetRecursiveFlags(Transform transform)
        {
            transform.gameObject.hideFlags = HideFlags.DontSave;
            for (int i = 0; i < transform.childCount; i++)
                SetRecursiveFlags(transform.GetChild(i));
        }
        static void DestroyChildren(Transform transform)
        {
            while (transform.childCount > 0)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }
        }

        private void OnDisable()
        {
            DestroyChildren(transform);
            lastAddress = null;
        }
    }
}