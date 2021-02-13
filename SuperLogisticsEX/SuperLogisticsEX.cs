using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using xiaoye97;

namespace SuperLogisticsEX
{
    

    [BepInDependency("me.xiaoye97.plugin.Dyson.LDBTool", BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin("tracing.plugin.SuperLogisticsEX", "SuperLogisticsEX", "1.0")]
    public class SuperLogisticsEX : BaseUnityPlugin
    {
        private static ConfigEntry<int> MaxEnergyPerTick;
        private static ConfigEntry<int> MaxEnergyNum;
        private static ConfigEntry<int> MaxStroageNum;
        void Start()
        {
            MaxEnergyPerTick = Config.Bind("SuperLogisticsEX", "MaxEnergyPerTick", 2,
                new ConfigDescription("最大充电功率倍数"));
            MaxEnergyNum = Config.Bind("SuperLogisticsEX", "MaxEnergyNum", 2,
                new ConfigDescription("蓄电量倍数"));
            MaxStroageNum = Config.Bind("SuperLogisticsEX", "MaxStroageNum", 2,
                new ConfigDescription("仓储容量倍数"));
            LDBTool.EditDataAction += EditLogistics;
        }

        void EditLogistics(Proto proto)
        {
            if (proto is ItemProto && proto.ID == 2104)
            {
                var item = proto as ItemProto;
                item.prefabDesc.workEnergyPerTick *= MaxEnergyPerTick.Value;
                item.prefabDesc.idleEnergyPerTick *= MaxEnergyPerTick.Value;

                item.prefabDesc.stationMaxEnergyAcc *= MaxEnergyNum.Value;
                item.prefabDesc.stationMaxItemCount *= MaxStroageNum.Value;

            }
        }
    }
}
