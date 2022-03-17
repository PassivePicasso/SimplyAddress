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
        public static readonly Dictionary<string, GameObject> PrefabCache = new Dictionary<string, GameObject>();
        public bool replaceSelf;

        [NonSerialized]
        public GameObject instance;
        private AsyncOperationHandle<GameObject> asyncOperation;

        void Update()
        {
            if (instance && lastAddress == Address) return;

            lastAddress = Address;
            if (transform.childCount > 0)
                DestroyChildren(transform);

            if (!asyncOperation.IsValid())
            {
                asyncOperation = Addressables.LoadAssetAsync<GameObject>(Address);
                PrefabCache[Address] = null;
            }

            switch (asyncOperation.Status)
            {
                case AsyncOperationStatus.None:
                    break;
                case AsyncOperationStatus.Succeeded:
                    PrefabCache[Address] = asyncOperation.Result;
                    break;
                case AsyncOperationStatus.Failed:
                    break;
            }

            if (PrefabCache[Address])
            {
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
            for (int i = 0; i < transform.childCount;)
                DestroyImmediate(transform.GetChild(i).gameObject);
        }

        private void OnDisable()
        {
            DestroyChildren(transform);
            lastAddress = null;
        }
    }
}