using UnityEngine;
using MelonLoader;

namespace FPSUnlocker
{
    public class FPSUnlocker : MelonMod
    {
        private const string ConfigCategory = "FPSUnlocker Config";
        private const string MaxFpsKey = "maxFps";
        private const string VSyncKey = "vSync";
        private const int DefaultFPS = 60;
        private const int DefaultVSyncCount = 0;

        public static int fps;
        public static bool vSync;

        public override void OnApplicationStart()
        {
            // Registering configuration settings in MelonPrefs
            MelonPrefs.RegisterCategory(ConfigCategory, ConfigCategory);
            MelonPrefs.RegisterInt(ConfigCategory, MaxFpsKey, 999, "Max FPS");
            MelonPrefs.RegisterBool(ConfigCategory, VSyncKey, false, "Enable/Disable vSync");
            OnModSettingsApplied();
        }

        public override void OnLevelWasLoaded(int level) => SetValues();

        public override void OnModSettingsApplied()
        {
            fps = MelonPrefs.GetInt(ConfigCategory, MaxFpsKey);
            vSync = MelonPrefs.GetBool(ConfigCategory, VSyncKey);
            SetValues(true);
        }

        public static void SetValues(bool shouldLog = false)
        {
            if (vSync)
            {
                if (shouldLog) MelonLogger.Log("vSync is enabled, MAX FPS will be ignored.");

                if (QualitySettings.vSyncCount != 1)
                    QualitySettings.vSyncCount = 1;
            }
            else
            {
                if (QualitySettings.vSyncCount != DefaultVSyncCount)
                    QualitySettings.vSyncCount = DefaultVSyncCount;

                if (fps < 1)
                {
                    if (shouldLog) MelonLogger.Log("FPS attempted to be set to less than 1: resetting to default value of 60.");
                    fps = DefaultFPS;
                }

                Application.targetFrameRate = fps;
                if (shouldLog) MelonLogger.Log($"FPS set to: {fps}");
            }
        }
    }
}
