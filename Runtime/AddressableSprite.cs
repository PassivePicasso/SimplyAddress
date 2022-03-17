using UnityEngine;
using UnityEngine.UI;

namespace PassivePicasso.SimplyAddress
{
    [ExecuteAlways]
    public class AddressableSprite : AddressableArrayAssigner<Image, Sprite>
    {
        protected override void Assign(Image image, Sprite sprite)
        {
            image.sprite = sprite;
        }

        protected override void Unassign(Image image, Sprite sprite)
        {
            image.sprite = null;
        }
    }
}