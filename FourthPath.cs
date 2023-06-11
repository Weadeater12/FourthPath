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
using static MelonLoader.MelonLogger;

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

    public class FourthPathMS : PathPlusPlus
    {
        public override string Tower => TowerType.MonkeySub;

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
            attackModel.weapons[0].projectile.scale /= 2;
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
        public override float Scale => 0.1f;

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
        public override float Scale => 0.25f;

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
        public override float Scale => 0.3f;

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
        public override float Scale => 0.3f;

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            Set2DTexture(node, "LightShuriken");
        }
    }
    public class SubT1 : UpgradePlusPlus<FourthPathMS>
    {
        public override int Cost => 250;
        public override int Tier => 1;
        public override string Icon => VanillaSprites.KnockbackUpgradeIcon;
        public override string Portrait => "";
        public override string DisplayName => "Knockback";
        public override string Description => "Knockback Bloons for more safety";

        public override void ApplyUpgrade(TowerModel towerModel, int tier)
        {
            var attackModel = towerModel.GetAttackModel();
            //attackModel.weapons[0].projectile.display = new PrefabReference() { guidRef = GetDisplayGUID<NinjaT1Proj>() };
            var knockback = Game.instance.model.GetTowerFromId("NinjaMonkey-010").GetDescendant<WindModel>().Duplicate();
            knockback.chance = 0.2f;
            knockback.distanceMin *= 1.5f;
            knockback.distanceMax *= 1.5f;
            attackModel.weapons[0].projectile.AddBehavior(knockback);
            if (IsHighestUpgrade(towerModel))

            {
                towerModel.ApplyDisplay<SubT1Display>();
            }
        }
    }
    public class SubT1Display : ModDisplay
    {

        public override string BaseDisplay => GetDisplay(TowerType.MonkeySub);
        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            foreach (var renderer in node.genericRenderers)
            {
                renderer.material.mainTexture = GetTexture("T1Sub");

            }
        }
    }
    public class SubT2 : UpgradePlusPlus<FourthPathMS>
    {
        public override int Cost => 150;
        public override int Tier => 2;
        public override string Icon => VanillaSprites.AdvancedTargetingUpgradeIcon;
        public override string Portrait => "";
        public override string DisplayName => "Developped Smell";
        public override string Description => "Feel the presence of camo Bloons and command his darts to pop them!";

        public override void ApplyUpgrade(TowerModel towerModel, int tier)
        {
            var attackModel = towerModel.GetAttackModel();
            //attackModel.weapons[0].projectile.display = new PrefabReference() { guidRef = GetDisplayGUID<NinjaT1Proj>() };
            attackModel.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
            if (IsHighestUpgrade(towerModel))

            {
                towerModel.ApplyDisplay<SubT2Display>();
            }
        }
    }
    public class SubT2Display : ModDisplay
    {

        public override string BaseDisplay => GetDisplay(TowerType.MonkeySub);
        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            foreach (var renderer in node.genericRenderers)
            {
                renderer.material.mainTexture = GetTexture("T2Sub");

            }
        }
    }
    public class SubT3 : UpgradePlusPlus<FourthPathMS>
    {
        public override int Cost => 900;
        public override int Tier => 3;
        public override string Icon => VanillaSprites.ArmorPiercingDartsUpgradeIcon;
        public override string Portrait => "";
        public override string DisplayName => "Straight Shot";
        public override string Description => "No longer have a homing effect but deal more damage";

        public override void ApplyUpgrade(TowerModel towerModel, int tier)
        {
            var attackModel = towerModel.GetAttackModel();
            attackModel.weapons[0].projectile.display = new PrefabReference() { guidRef = GetDisplayGUID<SubT3Proj>() };
            
            attackModel.weapons[0].projectile.RemoveBehavior<TrackTargetModel>();
            attackModel.weapons[0].rate *= 0.85f;
            attackModel.weapons[0].projectile.GetDamageModel().damage += 1;
            attackModel.weapons[0].projectile.pierce += 2f;
            if (IsHighestUpgrade(towerModel))

            {
                towerModel.ApplyDisplay<SubT3Display>();
            }
        }
    }
    public class SubT3Display : ModDisplay
    {

        public override string BaseDisplay => GetDisplay(TowerType.MonkeySub);
        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            foreach (var renderer in node.genericRenderers)
            {
                renderer.material.mainTexture = GetTexture("T3Sub");

            }
        }
    }
    public class SubT3Proj : ModDisplay
    {

        public override string BaseDisplay => Generic2dDisplay;
        public override float Scale => 1.1f;

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            Set2DTexture(node, "T3SubProj");
        }
    }
    public class SubT4 : UpgradePlusPlus<FourthPathMS>
    {
        public override int Cost => 5000;
        public override int Tier => 4;
        public override string Icon => VanillaSprites.TwinGunsUpgradeIcon;
        public override string Portrait => "";
        public override string DisplayName => "Machine Gun";
        public override string Description => "Add a machine gun that deal extra damage.";

        public override void ApplyUpgrade(TowerModel towerModel, int tier)
        {
            var attackModel = towerModel.GetAttackModel();
            //attackModel.weapons[0].projectile.display = new PrefabReference() { guidRef = GetDisplayGUID<NinjaT1Proj>() };

            attackModel.weapons[0].projectile.RemoveBehavior<TrackTargetModel>();
            attackModel.weapons[0].projectile.GetDamageModel().damage += 1;
            attackModel.weapons[0].projectile.pierce += 1f;
            towerModel.range += 6;
            attackModel.range += 6;
            var machinegun = Game.instance.model.GetTowerFromId("SniperMonkey-100").GetAttackModel().Duplicate();
            machinegun.range = towerModel.range *= 1.3f;
            machinegun.weapons[0].rate = 0.035f;
            machinegun.weapons[0].projectile.GetDamageModel().damage = 1;
            machinegun.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
            machinegun.name = "machinegun";
            towerModel.AddBehavior(machinegun);
            if (IsHighestUpgrade(towerModel))

            {
                towerModel.ApplyDisplay<SubT4Display>();
            }
        }
    }
    public class SubT4Display : ModDisplay
    {

        public override string BaseDisplay => GetDisplay(TowerType.MonkeySub, 0 ,4 ,0);
        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            foreach (var renderer in node.genericRenderers)
            {
                renderer.material.mainTexture = GetTexture("T4Sub");
                SetMeshOutlineColor(node, new UnityEngine.Color(65f / 255, 105f / 255, 225f / 255));
            }
        }
    }
    public class SubT5 : UpgradePlusPlus<FourthPathMS>
    {
        public override int Cost => 55000;
        public override int Tier => 5;
        public override string Icon => VanillaSprites.PlasmaAcceleratorUpgradeIcon;
        public override string Portrait => "";
        public override string DisplayName => "Missile Luncher";
        public override string Description => "Every few seconds, lunch a missile luncher";

        public override void ApplyUpgrade(TowerModel towerModel, int tier)
        {
            var attackModel = towerModel.GetAttackModel();
            attackModel.weapons[0].projectile.display = new PrefabReference() { guidRef = GetDisplayGUID<SubT5Proj>() };
            attackModel.weapons[0].rate *= 0.15f;
            attackModel.weapons[0].projectile.pierce *= 2;
            attackModel.weapons[0].projectile.GetDamageModel().damage *= 3f;



            var machinegun = towerModel.GetAttackModel("machinegun");
            machinegun.range *= 1.5f;          
            machinegun.weapons[0].rate *= 0.8f;
            machinegun.weapons[0].projectile.GetDamageModel().damage += 2;
            towerModel.AddBehavior(machinegun);
            var missile = Game.instance.model.GetTowerFromId("MonkeySub-020").GetAttackModel().Duplicate();
            missile.weapons[0].rate = 3.5f;
            missile.weapons[0].projectile.pierce = 1;
            missile.weapons[0].projectile.GetDamageModel().damage = 50;
            missile.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
            missile.range = 999999999;
            var Bomb = Game.instance.model.GetTowerFromId("BombShooter-500").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().Duplicate();
            var Radius = Game.instance.model.GetTowerFromId("BombShooter-500").GetAttackModel().weapons[0].projectile.GetBehavior<CreateEffectOnContactModel>().Duplicate();
            Bomb.projectile.GetBehavior<DamageModel>().damage = 250f;
            Bomb.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
            Bomb.projectile.GetBehavior<DamageModel>().immuneBloonProperties = BloonProperties.None;
            missile.weapons[0].projectile.AddBehavior(Bomb);
            missile.weapons[0].projectile.AddBehavior(Radius);
            missile.name = "missile";
            missile.weapons[0].projectile.AddBehavior(Game.instance.model.GetTowerFromId("BombShooter-500").GetAttackModel().weapons[0].projectile.GetBehavior<CreateSoundOnProjectileCollisionModel>().Duplicate());
            towerModel.AddBehavior(missile);
            if (IsHighestUpgrade(towerModel))

            {
                towerModel.ApplyDisplay<SubT5Display>();
            }
        }
    }
    public class SubT5Display : ModDisplay
    {

       public override string BaseDisplay => GetDisplay(TowerType.MonkeySub, 0, 4, 0);
        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            foreach (var renderer in node.genericRenderers)
            {
                renderer.material.mainTexture = GetTexture("T5Sub");
                SetMeshOutlineColor(node, new UnityEngine.Color(100f / 255, 0f / 255, 50f / 255));
            }
        }
    }
    public class SubT5Proj : ModDisplay
    {

        public override string BaseDisplay => Generic2dDisplay;
        public override float Scale => 1.3f;

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            Set2DTexture(node, "T5SubProj");
        }
    }
}
