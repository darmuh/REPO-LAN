using BepInEx.Logging;
using BepInEx;
using HarmonyLib;
using System.Reflection;
using System.IO;
using System;

namespace REPOLAN
{
    [BepInPlugin("com.github.darmuh.REPOLAN", "REPOLAN", (PluginInfo.PLUGIN_VERSION))]

    public class Plugin : BaseUnityPlugin
    {
        public static Plugin instance;
        public static class PluginInfo
        {
            public const string PLUGIN_GUID = "com.github.darmuh.REPOLAN";
            public const string PLUGIN_NAME = "REPOLAN";
            public const string PLUGIN_VERSION = "0.1.0";
        }

        internal static ManualLogSource Log;

        private void Awake()
        {
            instance = this;
            Log = base.Logger;
            Log.LogInfo($"{PluginInfo.PLUGIN_NAME} is loading with version {PluginInfo.PLUGIN_VERSION}!");

            //Config.ConfigReloaded += OnConfigReloaded;
            //MenuStuff.Init();
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
            Log.LogInfo($"{PluginInfo.PLUGIN_NAME} load complete!");
        }
        
    }
}
