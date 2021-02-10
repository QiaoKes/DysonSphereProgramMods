using System;
using xiaoye97;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;

namespace SuperBeltEX
{
    [BepInDependency("me.xiaoye97.plugin.Dyson.LDBTool", BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin("tracing.plugin.superbeltex", "superBeltEX", "1.0")]
    public class SuperBeltEX : BaseUnityPlugin
    {
        private static ConfigEntry<int> beltSpeed;
        void Start()
        {
            beltSpeed = Config.Bind("SuperBeltEX", "beltSpeed", 60,
                new ConfigDescription("传送带速度30-60/s 输入6的倍数 默认向下取整"));
            LDBTool.EditDataAction += EditBelt;
        }

        void EditBelt(Proto proto)
        {
            if (proto is ItemProto && proto.ID == 2003)
            {
                int belt_speed = beltSpeed.Value;
                if (belt_speed < 30) belt_speed = 30;
                if (belt_speed > 60) belt_speed = 60;

                var item = proto as ItemProto;
                item.prefabDesc.beltSpeed = (int)(belt_speed / 6);
            }
        }
    }
}
