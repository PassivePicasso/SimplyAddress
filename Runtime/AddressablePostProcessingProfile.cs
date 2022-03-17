using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace PassivePicasso.SimplyAddress
{
    [ExecuteAlways]
    public class AddressablePostProcessingProfile : AddressableArrayAssigner<PostProcessVolume, PostProcessProfile>
    {
        protected override void Assign(PostProcessVolume volume, PostProcessProfile profile)
        {
            volume.hideFlags |= HideFlags.NotEditable;
            volume.profile = profile;
        }

        protected override void Unassign(PostProcessVolume volume, PostProcessProfile profile)
        {
            volume.hideFlags ^= HideFlags.NotEditable;
            volume.profile = null;
        }
    }
}