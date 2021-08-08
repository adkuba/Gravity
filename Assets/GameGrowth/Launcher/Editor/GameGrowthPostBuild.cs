using UnityEditor.Callbacks;
using UnityEngine.GameGrowth;
using UnityEngine;

namespace UnityEditor.GameGrowth
{
    public static class GameGrowthPostBuild
    {
        [PostProcessBuild(100)]
        public static void OnPostprocessBuild(BuildTarget buildTarget, string buildPath)
        {
            var gameGrowthConfiguration = GameGrowthConfiguration.LoadMainAsset();
            if (gameGrowthConfiguration == null)
            {
                return;
            }
            GameGrowthEnvironmentValidator.LogStatus(gameGrowthConfiguration.environment);

            GameGrowthPackageVersion.RequestPostPackageVersion(packageVersionResponse =>
            {
                if (packageVersionResponse != null && !packageVersionResponse.isSuccess)
                {
                    Debug.LogError("Failed to update Game Growth Package Version: " + packageVersionResponse.error);
                }
            });
        }
    }
}
