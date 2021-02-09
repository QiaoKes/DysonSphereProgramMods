using System;
using xiaoye97;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;

namespace SuperFactoryEX
{
    [BepInDependency("me.xiaoye97.plugin.Dyson.LDBTool", BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin("tracing.plugin.SuperFactoryEX", "SuperFactoryEX", "1.0")]
    public class SuperFactoryEX : BaseUnityPlugin
    {
        //挖矿 抽水 抽水
        private static ConfigEntry<int> Mining_multiply;
        //生产设备
        private static ConfigEntry<int> Factory_multiply;
        //粒子对撞机
        private static ConfigEntry<int> Particle_multiply;
        //科技
        private static ConfigEntry<int> Teach_multiply;
        //分馏塔比率
        private static ConfigEntry<int> Fract_multiply;
        void Start()
        {
            new ConfigDescription("");

            Mining_multiply = Config.Bind("SuperFactoryEX", "Mining_multiply", 10,
                new ConfigDescription("倍率上限为100倍 随着倍率x 耗电倍数为1.1^x\n采集倍率"));
            Particle_multiply = Config.Bind("SuperFactoryEX", "Particle_multiply", 10,
                new ConfigDescription("粒子对撞机倍率"));
            Fract_multiply = Config.Bind("SuperFactoryEX", "Fract_multiply", 10,
                new ConfigDescription("分馏塔倍率"));
            Teach_multiply = Config.Bind("SuperFactoryEX", "Teach_multiply", 20,
                new ConfigDescription("科研设备倍率"));
            Factory_multiply = Config.Bind("SuperFactoryEX", "Factory_multiply", 20,
                new ConfigDescription("其余生产设备倍率"));
            
            LDBTool.EditDataAction += EditFactory;
            LDBTool.EditDataAction += EditFract;
        }

        void EditFactory(Proto proto)
        {
            if (proto is ItemProto)
            {
                var item = proto as ItemProto;
                if (item.ID == 2301 || item.ID == 2306 || item.ID == 2307)
                {
                    int Mining_multiply_val = Mining_multiply.Value;
                    if (Mining_multiply_val < 0) return ;
                    if (Mining_multiply_val > 100) Mining_multiply_val = 100;

                    int em = (int)Math.Pow(1.1, Mining_multiply_val) + 1;
                    item.prefabDesc.workEnergyPerTick *= em;
                    item.prefabDesc.idleEnergyPerTick *= em;
                    item.prefabDesc.minerPeriod = (int)(item.prefabDesc.minerPeriod / Mining_multiply_val);
                    
                }
                else if (item.ID == 2308 || item.ID == 2304 ||
                    item.ID == 2302 || item.ID == 2309)
                {
                    int Factory_multiply_val = Factory_multiply.Value;
                    if (Factory_multiply_val < 0) return;
                    if (Factory_multiply_val > 100) Factory_multiply_val = 100;

                    int em = (int)Math.Pow(1.1, Factory_multiply_val) + 1;
                    item.prefabDesc.workEnergyPerTick *= em;
                    item.prefabDesc.idleEnergyPerTick *= em;
                    item.prefabDesc.assemblerSpeed *= Factory_multiply_val;
                    
                }
                else if (item.ID == 2310)
                {
                    int Particle_multiply_val = Particle_multiply.Value;
                    if (Particle_multiply_val < 0) return;
                    if (Particle_multiply_val > 100) Particle_multiply_val = 100;

                    int em = (int)Math.Pow(1.1, Particle_multiply_val) + 1;
                    item.prefabDesc.workEnergyPerTick *= em;
                    item.prefabDesc.idleEnergyPerTick *= em;
                    item.prefabDesc.assemblerSpeed *= Particle_multiply_val;
                }
                else if (item.ID == 2314)
                {
                    int Fract_multiply_val = Fract_multiply.Value;
                    if (Fract_multiply_val < 0) return;
                    if (Fract_multiply_val > 100) Fract_multiply_val = 100;

                    int em = (int)Math.Pow(1.1, Fract_multiply_val) + 1;
                    item.prefabDesc.workEnergyPerTick *= em;
                    item.prefabDesc.idleEnergyPerTick *= em;
                    item.prefabDesc.labAssembleSpeed *= Fract_multiply_val;
                }
                else if (item.ID == 2901)
                {
                    int Teach_multiply_val = Teach_multiply.Value;
                    if (Teach_multiply_val < 0) return;
                    if (Teach_multiply_val > 100) Teach_multiply_val = 100;

                    int em = (int)Math.Pow(1.1, Teach_multiply_val) + 1;
                    item.prefabDesc.workEnergyPerTick *= em;
                    item.prefabDesc.idleEnergyPerTick *= em;
                    item.prefabDesc.labAssembleSpeed *= Teach_multiply_val;
                    item.prefabDesc.labResearchSpeed *= Teach_multiply_val;
                }
            }
        }

        void EditFract(Proto proto)
        {
            if (proto is RecipeProto && proto.ID == 115)
            {
                var recipe = proto as RecipeProto;
                var Fract_val = Fract_multiply.Value;
                if (Fract_val <= 0) return ;
                if (Fract_val > 100) Fract_val = 100;
                recipe.ResultCounts = new int[] { Fract_val };
            }
        }


    }
}
