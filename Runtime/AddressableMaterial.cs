using UnityEngine;

namespace PassivePicasso.SimplyAddress
{
    [ExecuteAlways]
    public class AddressableMaterial : AddressableArrayAssigner<Renderer, Material>
    {
        protected override void Assign(Renderer renderer, Material material)
        {
            switch (renderer)
            {
                case SkinnedMeshRenderer smr:
                    smr.material = material;
                    break;
                case MeshRenderer mr:
                    mr.material = material;
                    break;
            }
        }

        protected override void Unassign(Renderer renderer, Material material)
        {
            switch (renderer)
            {
                case SkinnedMeshRenderer smr:
                    smr.material = null;
                    break;
                case MeshRenderer mr:
                    mr.material = null;
                    break;
            }
        }
    }
}