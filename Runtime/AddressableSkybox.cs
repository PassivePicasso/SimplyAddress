using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace PassivePicasso.SimplyAddress
{
    [ExecuteAlways]
    public class AddressableSkybox : SimpleAddress
    {
        [NonSerialized]
        public Material material;
        // Start is called before the first frame update
        void Update()
        {
            if (material && lastAddress == Address) return;
            if (string.IsNullOrEmpty(Address)) return;

            lastAddress = Address;
            var materialOp = Addressables.LoadAssetAsync<Material>(Address);
            materialOp.Completed += MaterialOp_Completed;
        }


        void MaterialOp_Completed(AsyncOperationHandle<Material> obj)
        {
            material = obj.Result;
            if (material)
            {
                material.hideFlags = HideFlags.HideAndDontSave;
                RenderSettings.skybox = material;
            }
        }
    }
}