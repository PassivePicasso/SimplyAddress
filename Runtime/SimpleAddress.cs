using UnityEngine;
using UnityEngine.Serialization;

namespace PassivePicasso.SimplyAddress
{
    public class SimpleAddress : MonoBehaviour
    {
        protected string lastAddress;
        [FormerlySerializedAs("Key")]
        public string Address;
    }
}