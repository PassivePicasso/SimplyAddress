using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace PassivePicasso.RainOfStages.AddressableComponents
{
    [ExecuteAlways]
    public class AddressablePostProcessingProfile : AddressableArrayAssigner<PostProcessVolume, PostProcessProfile>
    {
        protected override void Assign(PostProcessVolume volume, PostProcessProfile profile)
        {
            volume.profile = profile;
        }

        protected override void Unassign(PostProcessVolume volume, PostProcessProfile profile)
        {
            volume.profile = null;
        }
    }
}