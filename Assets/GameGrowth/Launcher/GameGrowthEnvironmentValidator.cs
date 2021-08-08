using UnityEngine;
using UnityEngine.GameGrowth;
using UnityEditor;

public static class GameGrowthEnvironmentValidator
{
    const string k_GameGrowthInSandboxMessage = "Game Growth - Sandbox";
    const string k_GameGrowthInStoreMessage = "Game Growth - Store";
    const string k_DeltaDnaInDevMessage = "DeltaDNA - Development";

#if UNITY_EDITOR
    static readonly (string, MessageType) k_GameGrowthObjectMissingMessage = 
        ("Drag this GameGrowthLauncher prefab into your Hierarchy to proceed with GameGrowth integration.", 
        MessageType.Info);
    static readonly (string, MessageType) k_DuplicateGameGrowthLauncherObjectsError = 
        ("Multiple GameGrowthLauncher objects exist in scene.\nExactly one GameGrowthLauncher should exist in your Hierarchy.",
        MessageType.Error);
    static readonly (string, MessageType) k_GameGrowthInSandboxWarning = 
        ("Game Growth is currently in Sandbox mode.\nBe sure to switch environment to Store for your release.",
        MessageType.Warning);
    static readonly (string, MessageType) k_DeltaDnaInDevWarning = 
        ("Game Growth is currently in Store mode, but DeltaDNA configuration is set to Dev Make sure to switch environment to Live for your release.",
        MessageType.Warning);

    static readonly Color k_InfoColor = Color.green;
    static readonly Color k_WarningColor = Color.yellow;
    static readonly Color k_ErrorColor = Color.red;
    static readonly Color k_DefaultColor = Color.clear;

    /// <summary>
    ///     Returns Inspector warning string and color based on environment, number of prefabs in scene, etc.
    /// </summary>
    public static (string str, MessageType messageType) GetVerboseWarning(GameGrowthEnvironment environment, 
        GameGrowthLauncher[] gameGrowthObjects)
    {
        if (gameGrowthObjects.Length < 1)
        {
            return k_GameGrowthObjectMissingMessage;
        }

        if (gameGrowthObjects.Length > 1)
        {
            return k_DuplicateGameGrowthLauncherObjectsError;
        }

        if (environment == GameGrowthEnvironment.Sandbox)
        {
            return k_GameGrowthInSandboxWarning;
        }

        if (DeltaDNAConfigurationHelper.IsInDevelopmentMode())
        {
            return k_DeltaDnaInDevWarning;
        }

        return default;
    }

    /// <summary>
    ///     Validate and update GameGrowthLauncher name if necessary.
    /// </summary>
    public static void EnforceGameGrowthLauncherObjectName(GameGrowthLauncher gameGrowthLauncherObject, 
        GameGrowthEnvironment environment)
    {
        var desiredName = environment == GameGrowthEnvironment.Sandbox ?
            UnityEditor.GameGrowth.GameGrowthSettings.gameGrowthLauncherSandboxPrefabName :
            UnityEditor.GameGrowth.GameGrowthSettings.gameGrowthLauncherPrefabName;

        if (gameGrowthLauncherObject.name != desiredName)
        {
            gameGrowthLauncherObject.name = desiredName;
        }
    }

    /// <summary>
    ///     Returns desired GameGrowth HelpBox color based on type of message.
    /// </summary>
    public static Color GetHelpBoxColor(MessageType messageType)
    {
        switch (messageType)
        {
            case MessageType.Info:
                return k_InfoColor;

            case MessageType.Warning:
                return k_WarningColor;

            case MessageType.Error:
                return k_ErrorColor;

            default:
                return k_DefaultColor;
        }
    }
#endif

    /// <summary>
    ///     Logs appropriate warning or message based on GameGrowth environment and DeltaDNA environment.
    /// </summary>
    public static void LogStatus(GameGrowthEnvironment environment, bool warningOnly = false)
    {
        if (environment == GameGrowthEnvironment.Sandbox)
        {
            Debug.LogWarning(k_GameGrowthInSandboxMessage);
        }
        else
        {
            if (DeltaDNAConfigurationHelper.IsInDevelopmentMode())
            {
                Debug.LogWarning(k_DeltaDnaInDevMessage);
            }
            else if (!warningOnly)
            {
                Debug.Log(k_GameGrowthInStoreMessage);
            }
        }
    }
}
