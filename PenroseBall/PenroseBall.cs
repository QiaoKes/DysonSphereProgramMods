using System;
using BepInEx;
using xiaoye97;
using HarmonyLib;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;

namespace PenroseBallEX
{

    [BepInDependency("me.xiaoye97.plugin.Dyson.LDBTool", "1.7.0")]
    [BepInPlugin("tracing.plugin.PenroseBall", "PenroseBall", "1.1")]
    public class PenroseBallEX : BaseUnityPlugin
    {
        private Sprite icon;

        void Start()
        {
            var ab = AssetBundle.LoadFromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("PenroseBall.pb"));
            icon = ab.LoadAsset<Sprite>("pb");

            LDBTool.PostAddDataAction += AddBall;
            LDBTool.PreAddDataAction += AddLanguage;
        }

        
        void AddBall()
        {
            ItemProto oldBall = LDB.items.Select(2210);
            ItemProto PenroseBall = oldBall.Copy();
            RecipeProto PenroseBallRecipe = LDB.recipes.Select(43).Copy();

            PenroseBallRecipe.ID = 311;
            PenroseBallRecipe.Name = "人造黑洞";
            PenroseBallRecipe.name = "人造黑洞".Translate();
            PenroseBallRecipe.Description = "人造黑洞描述";
            PenroseBallRecipe.description = "人造黑洞描述".Translate();

            PenroseBallRecipe.Items = new int[] { 2210, 1125, 1127, 1501, 6006 };
            PenroseBallRecipe.ItemCounts = new int[] { 1, 50, 50, 50, 50 };
            PenroseBallRecipe.Results = new int[] { 9011 };

            PenroseBallRecipe.GridIndex = 2605;
            PenroseBallRecipe.preTech = LDB.techs.Select(1508);
            PenroseBallRecipe.SID = PenroseBallRecipe.GridIndex.ToString();
            PenroseBallRecipe.sid = PenroseBallRecipe.GridIndex.ToString();

            PenroseBall.ID = 9011;
            PenroseBall.Name = "人造黑洞";
            PenroseBall.name = "人造黑洞".Translate();
            PenroseBall.Description = "人造黑洞描述";
            PenroseBall.description = "人造黑洞描述".Translate();
            PenroseBall.BuildIndex = 1011;
            PenroseBall.GridIndex = PenroseBallRecipe.GridIndex;

            PenroseBall.handcraft = PenroseBallRecipe;
            PenroseBall.maincraft = PenroseBallRecipe;
            PenroseBall.handcrafts = new List<RecipeProto>() { PenroseBallRecipe };
            PenroseBall.recipes = new List<RecipeProto>() { PenroseBallRecipe };
            PenroseBall.makes = new List<RecipeProto>() { };

            PenroseBall.prefabDesc = oldBall.prefabDesc.Copy();
            PenroseBall.prefabDesc.modelIndex = PenroseBall.ModelIndex;


            PenroseBall.prefabDesc.isCollectStation = false;
            PenroseBall.prefabDesc.stationCollectSpeed = 0;

            Traverse.Create(PenroseBall).Field("_iconSprite").SetValue(icon);
            Traverse.Create(PenroseBallRecipe).Field("_iconSprite").SetValue(icon);

           
            

            //改燃料类型
            List<int[]> fuelNeedCopy = new List<int[]>();
            foreach (int[] line in ItemProto.fuelNeeds)
            {
                fuelNeedCopy.Add(line);
            }

            List<int> addFuel = new List<int>();
            foreach(int id in ItemProto.itemIds)
            {
                addFuel.Add(id);
            }
            fuelNeedCopy.Add(addFuel.ToArray());
            ItemProto.fuelNeeds = fuelNeedCopy.ToArray();
            PenroseBall.prefabDesc.fuelMask = ItemProto.fuelNeeds.Length - 1;

            //改为用电设备
            PenroseBall.prefabDesc.isPowerCharger = true;
            PenroseBall.prefabDesc.workEnergyPerTick = 350000 * 5000;
            PenroseBall.prefabDesc.idleEnergyPerTick = 350000 * 5000;
            PenroseBall.prefabDesc.genEnergyPerTick = (long)350000 * (long)10000;
            PenroseBall.prefabDesc.useFuelPerTick *= 1000;

            foreach (var mat in PenroseBall.prefabDesc.materials)
            {
                mat.color = new Color(34f / 255f, 217f / 255f, 188f / 255f);
            }

            LDBTool.PostAddProto(ProtoType.Item, PenroseBall);
            LDBTool.PostAddProto(ProtoType.Recipe, PenroseBallRecipe);

            try
            {
                LDBTool.SetBuildBar(8, 5, PenroseBall.ID);
            }
            catch
            {

            }
            
        }

        //void Update()
        //{
        //    if (LDB._items != null)
        //    {
        //        foreach (var itemData in LDB._items.dataArray)
        //        {
        //            if (itemData.ID == 9011)
        //            {
        //                LDB._
        //            }
        //        }

        //    }
        //}
        void AddLanguage()
        {
            StringProto PenroseBallName = new StringProto();
            StringProto PenroseBallDesc = new StringProto();
            PenroseBallName.ID = 10012;
            PenroseBallName.Name = "人造黑洞";
            PenroseBallName.name = "人造黑洞";
            PenroseBallName.ZHCN = "人造黑洞";
            PenroseBallName.ENUS = "Artificial black holes";
            PenroseBallName.FRFR = "Artificial black holes";

            PenroseBallDesc.ID = 10013;
            PenroseBallDesc.Name = "人造黑洞描述";
            PenroseBallDesc.name = "人造黑洞描述";
            PenroseBallDesc.ZHCN = "建立于人造黑洞上的彭罗斯球体，利用黑洞能层加速电磁波，使得电磁波强度呈指数倍增长，是通过超辐射散射收集能量的装置，不过小心，它可能会爆炸！(注意时刻向黑洞内投送物质维持其稳定，好在它不挑食)";
            PenroseBallDesc.ENUS = "The Penrose sphere, built on a man-made black hole, uses the black hole's ergosphere to accelerate electromagnetic waves, making them exponentially stronger, a device that collects energy through hyperradiation scattering, but watch out, it could explode! (Pay attention to throwing matter into the black hole at all times to keep it stable, but it's not a picky eater.).";
            PenroseBallDesc.FRFR = "The Penrose sphere, built on a man-made black hole, uses the black hole's ergosphere to accelerate electromagnetic waves, making them exponentially stronger, a device that collects energy through hyperradiation scattering, but watch out, it could explode! (Pay attention to throwing matter into the black hole at all times to keep it stable, but it's not a picky eater.).";

            LDBTool.PreAddProto(ProtoType.String, PenroseBallName);
            LDBTool.PreAddProto(ProtoType.String, PenroseBallDesc);
        }

    }
}

