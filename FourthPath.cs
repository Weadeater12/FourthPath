using MelonLoader;
using BTD_Mod_Helper;
using FourthPath;
using PathsPlusPlus;

using Il2CppAssets.Scripts.Models.Towers;
using BTD_Mod_Helper.Api.Enums;
using Il2Cpp;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppSystem.IO;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models;
using System;
using BTD_Mod_Helper.Api.Towers;
using Il2CppAssets.Scripts.Models.TowerSets;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Utils;
using Il2CppAssets.Scripts.Models.GenericBehaviors;
using BTD_Mod_Helper.Api.Display;
using System.Linq;
using BTD_Mod_Helper.Api;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using HarmonyLib;
using Il2CppAssets.Scripts.Simulation.Towers.Behaviors;
using Il2CppAssets.Scripts.Unity.Towers.Behaviors;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using Harmony;
using Il2CppAssets.Scripts.Models.Towers.Weapons;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;
using Il2CppAssets.Scripts.Unity.Display;
using FourthPath;

[assembly: MelonInfo(typeof(FourthPath.FourthPathMod), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace FourthPath;

public class FourthPathMod : BloonsTD6Mod
{
    public override void OnApplicationStart()
    {

    }

    public override void OnGameModelLoaded(GameModel model)
    {
        base.OnGameModelLoaded(model);
    }

    

    public class FourthPathNM : PathPlusPlus
    {
        public override string Tower => TowerType.NinjaMonkey;

        public override int UpgradeCount => 5;
    }
    public class NinjaT1 : UpgradePlusPlus<FourthPathNM>
    {
        public override int Cost => 250;
        public override int Tier => 1;
        public override string Icon => "T1NinjaIcon";
        public override string Portrait => "T1NinjaPortrait";
        public override string DisplayName => "Hot Shuriken";
        public override string Description => "Shuriken are hot and can pop lead Bloons";

        public override void ApplyUpgrade(TowerModel towerModel, int tier)
        {
            var attackModel = towerModel.GetAttackModel();
            attackModel.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
            attackModel.weapons[0].projectile.display = new PrefabReference() { guidRef = GetDisplayGUID<NinjaT1Proj>() };
         
            if (IsHighestUpgrade(towerModel))

            {
                towerModel.ApplyDisplay<NinjaT1Display>();
            }
        }
    }
    public class NinjaT1Display : ModDisplay
    {

        public override string BaseDisplay => GetDisplay(TowerType.NinjaMonkey);
        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            foreach (var renderer in node.genericRenderers)
            {
                renderer.material.mainTexture = GetTexture("T1Ninja");
                SetMeshOutlineColor(node, new UnityEngine.Color(255f / 255, 182f / 255, 0f / 255));
            }
        }
    }
    public class NinjaT1Proj : ModDisplay
    {

        public override string BaseDisplay => Generic2dDisplay;
        public override float Scale => 0.4f;

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            Set2DTexture(node, "HotShuriken");
        }
    }
    public class NinjaT2 : UpgradePlusPlus<FourthPathNM>
    {
        public override int Cost => 450;
        public override int Tier => 2;
        public override string Icon => "T2NinjaIcon";
        public override string Portrait => "T2NinjaPortrait";
        public override string DisplayName => "Flame Shuriken";
        public override string Description => "Set Bloons on fire";

        public override void ApplyUpgrade(TowerModel towerModel, int tier)
        {
            var attackModel = towerModel.GetAttackModel();
            foreach (var projectileModel in towerModel.GetDescendants<ProjectileModel>().ToArray())
            {
                projectileModel.collisionPasses = new int[] { -1, 0 };
                var LavaBehavior = Game.instance.model.GetTowerFromId("Alchemist").GetDescendant<AddBehaviorToBloonModel>().Duplicate();
                LavaBehavior.GetBehavior<DamageOverTimeModel>().interval = 2f;
                LavaBehavior.lifespan = 6;
                LavaBehavior.lifespanFrames = 288;
                LavaBehavior.overlayType = "Fire";
                //LavaBehavior.overlayType
                projectileModel.AddBehavior(LavaBehavior);
            }
            if (IsHighestUpgrade(towerModel))
            {
                towerModel.ApplyDisplay<NinjaT2Display>();
            }

        }
    }
    public class NinjaT2Display : ModDisplay
    {

        public override string BaseDisplay => GetDisplay(TowerType.NinjaMonkey);
        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            foreach (var renderer in node.genericRenderers)
            {
                renderer.material.mainTexture = GetTexture("T2Ninja");
              
            }
        }
    }
    public class NinjaT3 : UpgradePlusPlus<FourthPathNM>
    {
        public override int Cost => 1200;
        public override int Tier => 3;
        public override string Icon => "T3NinjaIcon";
        public override string Portrait => "T3NinjaPortrait";
        public override string DisplayName => "Molten Shuriken";
        public override string Description => "Fire get really strong and shoot faster";

        public override void ApplyUpgrade(TowerModel towerModel, int tier)
        {
            var attackModel = towerModel.GetAttackModel();
            attackModel.weapons[0].rate *= 0.7f;
            attackModel.weapons[0].projectile.display = new PrefabReference() { guidRef = GetDisplayGUID<NinjaT3Proj>() };
            foreach (var projectileModel in towerModel.GetDescendants<ProjectileModel>().ToArray())
            {
                projectileModel.RemoveBehavior<AddBehaviorToBloonModel>();
                projectileModel.collisionPasses = new int[] { -1, 0 };
                var LavaBehavior = Game.instance.model.GetTowerFromId("Alchemist").GetDescendant<AddBehaviorToBloonModel>().Duplicate();
                LavaBehavior.GetBehavior<DamageOverTimeModel>().interval = 1.8f;
                LavaBehavior.GetBehavior<DamageOverTimeModel>().damage += 1f;
                LavaBehavior.lifespan = 9;
                LavaBehavior.lifespanFrames = 432;
                LavaBehavior.overlayType = "Fire";
                //LavaBehavior.overlayType
                projectileModel.AddBehavior(LavaBehavior);
                
            }
            if (IsHighestUpgrade(towerModel))
            {
                towerModel.ApplyDisplay<NinjaT3Display>();
            }

        }
    }
    public class NinjaT3Display : ModDisplay
    {

        public override string BaseDisplay => GetDisplay(TowerType.NinjaMonkey);
        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            foreach (var renderer in node.genericRenderers)
            {
                renderer.material.mainTexture = GetTexture("T3Ninja");

            }
        }
    }
    public class NinjaT3Proj : ModDisplay
    {

        public override string BaseDisplay => Generic2dDisplay;
        public override float Scale => 0.5f;

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            Set2DTexture(node, "MoltenShuriken");
        }
    }
    public class NinjaT4 : UpgradePlusPlus<FourthPathNM>
    {
        public override int Cost => 10000;
        public override int Tier => 4;
        public override string Icon => "T4NinjaIcon";
        public override string Portrait => "T4NinjaPortrait";
        public override string DisplayName => "Light King";
        public override string Description => "Shoot a destroying light Ball every few seconds. Shoot even faster";

        public override void ApplyUpgrade(TowerModel towerModel, int tier)
        {

            var attackModel = towerModel.GetAttackModel();
            attackModel.weapons[0].rate *= 0.22f;
            towerModel.range += 10;
            attackModel.range += 10;
            attackModel.weapons[0].projectile.pierce += 3f;

            attackModel.weapons[0].projectile.display = new PrefabReference() { guidRef = GetDisplayGUID<NinjaT4Proj>() };
            
            var SM = Game.instance.model.GetTowerFromId("NinjaMonkey").GetAttackModel().Duplicate();
            SM.weapons[0].rate = 2.3f;
            SM.name = "LightBall";
            SM.weapons[0].projectile.name = "LightBall_Projectile";
            SM.weapons[0].projectile.display = new PrefabReference() { guidRef = "95e1b845816b6f748af84449fe6b7a59" };
            SM.weapons[0].projectile.GetDamageModel().damage = 40f;
            SM.weapons[0].projectile.pierce = 30;
            SM.weapons[0].projectile.GetBehavior<TravelStraitModel>().speed /= 4;
            SM.weapons[0].projectile.radius *= 1.6f;
            SM.weapons[0].projectile.scale *= 1.3f;
            SM.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
            SM.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
            SM.range = towerModel.range;
            if (towerModel.appliedUpgrades.Contains(UpgradeType.NinjaDiscipline))
            {
                SM.weapons[0].rate *= 0.85f;
                SM.weapons[0].projectile.GetDamageModel().damage += 2f;
            }
            if (towerModel.appliedUpgrades.Contains(UpgradeType.SharpShurikens))
            {
                SM.weapons[0].projectile.pierce += 10f;
            }
            towerModel.AddBehavior(SM);
            foreach (var projectileModel in towerModel.GetDescendants<ProjectileModel>().ToArray())
            {
                projectileModel.RemoveBehavior<AddBehaviorToBloonModel>();
                projectileModel.collisionPasses = new int[] { -1, 0 };
                var LavaBehavior = Game.instance.model.GetTowerFromId("Alchemist").GetDescendant<AddBehaviorToBloonModel>().Duplicate();
                LavaBehavior.GetBehavior<DamageOverTimeModel>().interval = 1.5f;
                LavaBehavior.GetBehavior<DamageOverTimeModel>().damage = 4f;
                LavaBehavior.lifespan = 14;
                LavaBehavior.lifespanFrames = 672;
                LavaBehavior.overlayType = "Fire";
                //LavaBehavior.overlayType
                projectileModel.AddBehavior(LavaBehavior);
       
            }

            if (IsHighestUpgrade(towerModel))
            {
                towerModel.ApplyDisplay<NinjaT4Display>();
            }
        }
    }
    public class NinjaT4Display : ModDisplay
    {

        public override string BaseDisplay => GetDisplay(TowerType.NinjaMonkey, 4, 2 ,0);
        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            foreach (var renderer in node.genericRenderers)
            {
                renderer.material.mainTexture = GetTexture("T4Ninja");

            }
        }
    }
    public class NinjaT4Proj : ModDisplay
    {

        public override string BaseDisplay => Generic2dDisplay;
        public override float Scale => 0.6f;

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            Set2DTexture(node, "LightShuriken");
        }
    }
    public class NinjaT5 : UpgradePlusPlus<FourthPathNM>
    {
        public override int Cost => 100000;
        public override int Tier => 5;
        public override string Icon => "T5NinjaIcon";
        public override string Portrait => "T5NinjaPortrait";
        public override string DisplayName => "True Light God";
        public override string Description => "Light ball and main shuriken get stronger";

        public override void ApplyUpgrade(TowerModel towerModel, int tier)
        {
            var attackModel = towerModel.GetAttackModel();
            var attackModel2 = towerModel.GetAttackModel("LightBall");
            attackModel.weapons[0].rate *= 0.25f;
            attackModel.weapons[0].projectile.pierce += 15f;
            attackModel.weapons[0].projectile.GetDamageModel().damage += 5;
            attackModel.weapons[0].projectile.display = new PrefabReference() { guidRef = "95e1b845816b6f748af84449fe6b7a59" };
            attackModel2.weapons[0].projectile.display = new PrefabReference() { guidRef = "dcd6cd8511c9a03458a32f42f860882c" };
            attackModel2.weapons[0].rate = 0.85f;
            attackModel2.weapons[0].projectile.pierce = 200;
            attackModel2.weapons[0].projectile.GetDamageModel().damage = 220;
            attackModel.weapons[0].projectile.GetBehavior<TravelStraitModel>().lifespan = 5;
            attackModel2.weapons[0].projectile.scale *= 1.5f;
            attackModel2.weapons[0].projectile.radius *= 1.5f;
            towerModel.range += 18;
            attackModel.range += 18;
            attackModel.weapons[0].projectile.display = new PrefabReference() { guidRef = GetDisplayGUID<NinjaT5Proj>() };
            attackModel2.range += 8;
            if (towerModel.appliedUpgrades.Contains(UpgradeType.NinjaDiscipline))
            {
                attackModel2.weapons[0].rate *= 0.85f;
                attackModel2.weapons[0].projectile.GetDamageModel().damage += 50f;
            }
            if (towerModel.appliedUpgrades.Contains(UpgradeType.SharpShurikens))
            {
                attackModel2.weapons[0].projectile.pierce += 80;
            }
            foreach (var projectileModel in towerModel.GetDescendants<ProjectileModel>().ToArray())
            {
                projectileModel.RemoveBehavior<AddBehaviorToBloonModel>();
                projectileModel.collisionPasses = new int[] { -1, 0 };
                var LavaBehavior = Game.instance.model.GetTowerFromId("Alchemist").GetDescendant<AddBehaviorToBloonModel>().Duplicate();
                LavaBehavior.GetBehavior<DamageOverTimeModel>().interval = 0.4f;
                LavaBehavior.GetBehavior<DamageOverTimeModel>().damage = 12f;
                LavaBehavior.lifespan = 10000;
                LavaBehavior.lifespanFrames = 10000000;
                LavaBehavior.overlayType = "Fire";
                //LavaBehavior.overlayType
                projectileModel.AddBehavior(LavaBehavior);
               
            }
            if (IsHighestUpgrade(towerModel))
            {
                towerModel.ApplyDisplay<NinjaT5Display>();
            }

        }
    }
    public class NinjaT5Display : ModDisplay
    {

        public override string BaseDisplay => GetDisplay(TowerType.NinjaMonkey, 5, 0, 0);
        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            foreach (var renderer in node.genericRenderers)
            {
                renderer.material.mainTexture = GetTexture("T4Ninja");

            }
        }
    }
    public class NinjaT5Proj : ModDisplay
    {

        public override string BaseDisplay => Generic2dDisplay;
        public override float Scale => 0.65f;

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            Set2DTexture(node, "LightShuriken");
        }
    }
}
