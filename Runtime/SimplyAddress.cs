using BepInEx;

namespace PassivePicasso.SimplyAddress
{
    using static PassivePicasso.SimplyAddress.Constants;
    public static class Constants
    {
        public const string Name = "SimplyAddress";
        public const string GUID = "com.passivepicasso.simplyaddress";
        public const string Version = "0.0.1";
    }

    [BepInPlugin(GUID, Name, Version)]
    public class SimplyAddress : BaseUnityPlugin
    {

    }
}