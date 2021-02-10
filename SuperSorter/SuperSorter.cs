using System;
using xiaoye97;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;

namespace SuperSorterEx
{
    [BepInDependency("me.xiaoye97.plugin.Dyson.LDBTool", BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin("tracing.plugin.SuperSorterEx", "SuperSorterEx", "1.0")]
    public class SuperSorterEx : BaseUnityPlugin
    {
        private static ConfigEntry<int> sorterCount;
        void Start()
        {
            sorterCount = Config.Bind("SuperSorterEx", "sorterCount", 20,
                new ConfigDescription("分拣器一次运送货物数"));
            Harmony.CreateAndPatchAll(typeof(SuperSorterEx));
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(GameHistoryData), "Import")]
        public static void PatchSorterImport(ref GameHistoryData __instance)
        {
            __instance.inserterStackCount = sorterCount.Value;
        }

    }
}
