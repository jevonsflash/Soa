using Soa.GatewaySample.Debugging;

namespace Soa.GatewaySample
{
    public class GatewaySampleConsts
    {
        public const string LocalizationSourceName = "GatewaySample";

        public const string ConnectionStringName = "Default";

        public const bool MultiTenancyEnabled = true;


        /// <summary>
        /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
        /// </summary>
        public static readonly string DefaultPassPhrase =
            DebugHelper.IsDebug ? "gsKxGZ012HLL3MI5" : "7decde082a834c4f9d51d6600e3ea797";
    }
}
