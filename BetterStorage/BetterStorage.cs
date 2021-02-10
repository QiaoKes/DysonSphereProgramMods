using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BepInEx;
using HarmonyLib;
using BepInEx.Configuration;


namespace BetterStorage
{
    [BepInPlugin(GUID, NAME, VERSION)]

    public class BetterStorage : BaseUnityPlugin
    {
        private const string GUID = "tracing.plugin.BetterStorage";
        private const string NAME = "BetterStorage";
        private const string VERSION = "1.2";
        private const string GAME_VERSION = "0.6.15.5706";

        //private static ConfigEntry<int> collect_multiple;
        //private static ConfigEntry<int> product_multiple;
        private static ConfigEntry<int> storage_multiple;

        private static bool storage_is_change = false;

        void Start()
        {
            Harmony.CreateAndPatchAll(typeof(BetterStorage));
            LogInfo();
            ParameterSetting();
        }

        void LogInfo()
        {
            Logger.LogInfo("plugin version: " + VERSION);
            Logger.LogInfo("for game version: " + GAME_VERSION);
            Logger.LogInfo("curr game version: " + GameConfig.gameVersion.ToFullString());
        }

        void ParameterSetting()
        {
            //collect_multiple = Config.Bind("FastProduction", "collect_multiple", 40, new ConfigDescription("采集速度倍率"));
            //product_multiple = Config.Bind("FastProduction", "product_multiple", 10, new ConfigDescription("生产设备速度倍率"));
            storage_multiple = Config.Bind("BetterStorage", "storage_multiple", 10, new ConfigDescription("物品堆叠倍率"));
        }

        ////生产设备
        //[HarmonyPrefix]
        //[HarmonyPatch(typeof(AssemblerComponent), "InternalUpdate")]
        //public static void PatchProduct(ref float power)
        //{
        //    if (power < 0.100000001490116)
        //        return;
        //    power *= product_multiple.Value;
        //}


        ////采集速度
        //[HarmonyPrefix]
        //[HarmonyPatch(typeof(MinerComponent), "InternalUpdate")]
        //public static void PatchCollect(ref float miningSpeed)
        //{
        //    miningSpeed *= collect_multiple.Value;
        //}
        


        //物品堆叠
        [HarmonyPostfix]
        [HarmonyPatch(typeof(StorageComponent), "LoadStatic")]
        public static void MyLoadStatic(StorageComponent __instance)
        {
            if (!storage_is_change)
            {
                ItemProto[] dataArray = LDB.items.dataArray;
                for (int i = 0; i < dataArray.Length; i++)
                {
                    StorageComponent.itemStackCount[dataArray[i].ID] *= storage_multiple.Value;
                }
                storage_is_change = true;
            }
        }
    }
}
