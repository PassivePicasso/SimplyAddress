using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace PassivePicasso.SimplyAddress
{
    [ExecuteAlways]
    public abstract class AddressableArrayAssigner<ComponentType, AssetType> : SimpleAddress where AssetType : UnityEngine.Object where ComponentType : Component
    {
        public ComponentType[] TargetComponents;
        [NonSerialized]
        public AssetType AssetInstance;

        void Update()
        {
            if (lastAddress == Address) return;

            lastAddress = Address;
            if (string.IsNullOrEmpty(Address)) return;
            Load();
        }

        private void Load()
        {
            var loadOperation = Addressables.LoadAssetAsync<AssetType>(Address);
            loadOperation.Completed += OnLoaded;
        }

        private void OnEnable()
        {
            if (!AssetInstance)
                Load();
            else
                AssignInternal();
        }

        private void OnDisable()
        {
            UnassignInternal();
        }

        void OnLoaded(AsyncOperationHandle<AssetType> obj)
        {
            if (!obj.Result) return;
            obj.Result.hideFlags = HideFlags.NotEditable | HideFlags.HideAndDontSave;
            AssetInstance = Instantiate(obj.Result);
            AssetInstance.hideFlags = HideFlags.NotEditable | HideFlags.HideAndDontSave;
            AssetInstance.name = AssetInstance.name.Replace("(Clone)", "(WeakReference)");
            AssignInternal();
        }

        private void AssignInternal()
        {
            if (AssetInstance)
                foreach (var component in TargetComponents)
                    Assign(component, AssetInstance);
        }
        private void UnassignInternal()
        {
            if (AssetInstance)
                foreach (var component in TargetComponents)
                    Unassign(component, AssetInstance);
        }

        protected abstract void Assign(ComponentType component, AssetType asset);
        protected abstract void Unassign(ComponentType component, AssetType asset);
    }
}