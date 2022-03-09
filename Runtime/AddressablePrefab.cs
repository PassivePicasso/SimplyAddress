using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace AddressablesKit
{
    [ExecuteAlways]
    public class AddressablePrefab : MonoBehaviour
    {
        public bool replaceSelf;
        private string lastAddress;
        public string Address;

        private GameObject prefab;
        private GameObject instance;

        async void Update()
        {
            if (lastAddress != Address)
                try
                {
                    if (transform.childCount > 0)
                    {
                        DestroyChildren(transform);
                        DestroyImmediate(instance);
                    }

                    prefab = await Addressables.LoadAssetAsync<GameObject>(Address).Task;
                    if (prefab)
                    {
                        instance = Instantiate(prefab);
                        instance.transform.position = transform.position;
                        instance.transform.rotation = transform.rotation;
                        instance.transform.localScale = transform.localScale;
                        if (Application.isPlaying && replaceSelf)
                        {
                            Destroy(this);
                        }
                        else
                            instance.transform.parent = transform;


                        SetRecursiveFlags(instance.transform);
                    }
                }
                finally
                {
                    lastAddress = Address;
                }
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
            DestroyImmediate(instance);
            lastAddress = null;
            Addressables.Release(prefab);
        }
    }
}