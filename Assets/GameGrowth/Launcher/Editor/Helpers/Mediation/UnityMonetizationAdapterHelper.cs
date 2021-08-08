#if GAMEGROWTH_UNITY_MONETIZATION
using System.Collections.Generic;
using Unity.Mediation.Adapters.Editor;

namespace UnityEditor.GameGrowth.Helpers.UnityMediation
{
    public class UnityMonetizationAdapterHelper
    {
        static readonly string[] k_RequiredAdapterIds = {"unityads-adapter", "admob-adapter", "facebook-adapter",
                                                         "adcolony-adapter", "ironsource-adapter", "applovin-adapter", "vungle-adapter"};

        public static void EnableRequiredAdNetworks()
        {
            List<AdapterInfo> installedAdapters = MediationSdkInfo.GetInstalledAdapters();

            foreach (var adapterId in k_RequiredAdapterIds)
            {
                if (!installedAdapters.Exists(adapter => adapter.Identifier.Equals(adapterId)))
                {
                    MediationSdkInfo.Install(adapterId);
                }
            }

            MediationSdkInfo.Apply();
        }
    }
}
#endif
