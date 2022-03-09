using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace AddressablesKit
{
    [ExecuteAlways]
    public class AddressableSkybox : MonoBehaviour
    {
        public string Key;
        private string LastKey;
        // Start is called before the first frame update
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
            if (material)
            {
                RenderSettings.skybox = material;
            }
        }
    }
}