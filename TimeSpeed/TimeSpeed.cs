using System;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;

namespace TimeSpeed
{
    [BepInPlugin("tracing.plugin.TimeSpeed", "TimeSpeed", "1.0")]
    public class TimeSpeed : BaseUnityPlugin
    {
        private static ConfigEntry<int> gameSpeed;
        void Start()
        {
            gameSpeed = Config.Bind("SuperSorterEx", "gameSpeed", 1,
                new ConfigDescription("建议不要超过10，太快可能会出bug'\n'游戏速度"));
            Harmony.CreateAndPatchAll(typeof(TimeSpeed));
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(GameMain), "Begin")]
        public static void PatchTimeSpeed()
        {
            Time.timeScale = gameSpeed.Value;
        }
    }
}
