using System;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;


namespace FastLogistic
{
    [BepInPlugin("tracing.plugin.FastLogistic", "FastLogistic", "1.0")]
    public class FastLogistic : BaseUnityPlugin
    {
        private static ConfigEntry<float> DroneSpeed;
        private static ConfigEntry<float> ShipSpeed;
        private static ConfigEntry<int> DroneCarries;
        private static ConfigEntry<int> ShipCarries;
        void Start()
        {
            Harmony.CreateAndPatchAll(typeof(FastLogistic));
            DroneSpeed = Config.Bind("FastLogistic", "DroneSpeed", 100f,
                new ConfigDescription("无人机速度倍率"));
            ShipSpeed = Config.Bind("FastLogistic", "ShipSpeed", 100f,
                new ConfigDescription("运输船速度倍率"));
            DroneCarries = Config.Bind("FastLogistic", "DroneCarries", 1000,
                new ConfigDescription("无人机载货量"));
            ShipCarries = Config.Bind("FastLogistic", "ShipCarries", 2000,
                new ConfigDescription("运输船载货量"));
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(GameHistoryData), "Import")]
        public static void patchLogisticImport(ref GameHistoryData __instance)
        {
            __instance.logisticDroneSpeedScale = DroneSpeed.Value;
            __instance.logisticShipSpeedScale = ShipSpeed.Value;
            __instance.logisticDroneCarries = DroneCarries.Value;
            __instance.logisticShipCarries = ShipCarries.Value;
        }
    }
}
