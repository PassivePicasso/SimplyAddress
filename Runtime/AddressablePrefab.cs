using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace PassivePicasso.SimplyAddress
{
    [ExecuteAlways]
    public class AddressablePrefab : SimpleAddress
    {
        [SerializeField]
        public static readonly Dictionary<string, GameObject> PrefabCache = new Dictionary<string, GameObject>();
        public bool replaceSelf;

        [NonSerialized]
        public GameObject instance;
        private AsyncOperationHandle<GameObject> baseOp;
        void Update()
        {
            if (instance && lastAddress == Address) return;

            lastAddress = Address;
            if(PrefabCache.ContainsKey(Address))
            {
                CreateInstance();
            }
            
            if (!baseOp.IsValid() || baseOp.Status != AsyncOperationStatus.None)
            {
                baseOp = Addressables.LoadAssetAsync<GameObject>(Address);
                baseOp.Completed += OnCompleted;
                baseOp.Destroyed += (AsyncOperationHandle _) => Debug.Log("Operation has been destroyed");
            }
        }
        private void OnCompleted(AsyncOperationHandle<GameObject> aOp)
        {
            Debug.Log("Operation completed");
            if(aOp.Status == AsyncOperationStatus.Succeeded)
            {
                PrefabCache[Address] = aOp.Result;
                if (transform.childCount > 0)
                {
                    DestroyChildren(transform);
                }
                CreateInstance();
            }
        }
        private void CreateInstance()
        {
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
        }


        static void SetRecursiveFlags(Transform transform)
        {
            transform.gameObject.hideFlags = HideFlags.DontSave;
            for (int i = 0; i < transform.childCount; i++)
                SetRecursiveFlags(transform.GetChild(i));
        }
        static void DestroyChildren(Transform transform)
        {
            for (int i = 0; i < transform.childCount; i++)
                DestroyImmediate(transform.GetChild(i).gameObject);
        }

        private void OnDisable()
        {
            DestroyChildren(transform);
            lastAddress = null;
        }
    }
}