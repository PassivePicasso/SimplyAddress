using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace AddressablesKit
{
    [RequireComponent(typeof(MeshRenderer)), ExecuteAlways]
    public class AddressableLoader : MonoBehaviour
    {
        public static string[] AvailableKeys;
        public string Key;
        private string LastKey;

        void Update()
        {
            if (LastKey == Key) return;
            if (string.IsNullOrEmpty(Key)) return;

            LastKey = Key;
            var materialOp = Addressables.LoadAssetAsync<Material>(Key);
            materialOp.Completed += MaterialOp_Completed;
        }

        void MaterialOp_Completed(AsyncOperationHandle<Material> obj)
        {
            var material = obj.Result;
            var renderer = GetComponent<MeshRenderer>();
            if (renderer && material)
            {
                renderer.material = material;
            }
        }
    }
}