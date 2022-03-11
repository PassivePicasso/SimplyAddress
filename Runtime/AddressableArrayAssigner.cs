using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace PassivePicasso.RainOfStages.AddressableComponents
{
    [ExecuteAlways]
    public abstract class AddressableArrayAssigner<ComponentType, AssetType> : MonoBehaviour where AssetType : UnityEngine.Object where ComponentType : Component
    {
        public ComponentType[] TargetComponents;
        public static string[] AvailableKeys;
        public string Key;
        private string LastKey;
        private AssetType assetInstance;

        void Update()
        {
            if (LastKey == Key) return;

            LastKey = Key;
            if (string.IsNullOrEmpty(Key)) return;
            Load();
        }

        private void Load()
        {
            var loadOperation = Addressables.LoadAssetAsync<AssetType>(Key);
            loadOperation.Completed += OnLoaded;
        }

        private void OnEnable()
        {
            if (!assetInstance)
                Load();
            else
                AssignInternal();
        }
        private void OnDisable()
        {
            
        }

        void OnLoaded(AsyncOperationHandle<AssetType> obj)
        {
            assetInstance = obj.Result;
            assetInstance.hideFlags = HideFlags.NotEditable | HideFlags.HideAndDontSave;
            AssignInternal();
        }

        private void AssignInternal()
        {
            if (assetInstance)
                foreach (var component in TargetComponents)
                    Assign(component, assetInstance);
        }

        protected abstract void Assign(ComponentType component, AssetType asset);
        protected abstract void Unassign(ComponentType component, AssetType asset);
    }
}