using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VehicleFramework;
using System.IO;
using System.Reflection;

using UnityEngine.U2D;
using VehicleFramework.VehicleBuilding;
using VehicleFramework.VehicleTypes;
using VehicleFramework.StorageComponents;
using VehicleFramework.Engines;

namespace CrushDrone
{
    public class Crush : Drone
    {
        /* Set UnlockedWith to an impossible value
         * So that the Crush can be unlocked,
         * but not by scanning anything in the base game.
         * We add our own scanning logic in CrushFragment.
         */
        public override TechType UnlockedWith => MainPatcher.config.isFragmentExperience ? TechType.Fragment : TechType.Constructor;
        public override Sprite UnlockedSprite => MainPatcher.assets.unlock;
        public override string UnlockedMessage => "Drone Station Required";
        public override Transform CameraLocation => transform.Find("CameraLocation");
        public override string vehicleDefaultName => "Crush";
        public override GameObject VehicleModel => MainPatcher.assets.model;
        public override GameObject[] CollisionModel => new GameObject[] { transform.Find("CollisionModel").gameObject };
        public override GameObject StorageRootObject => transform.Find("StorageRoot").gameObject;
        public override GameObject ModulesRootObject => transform.Find("ModulesRoot").gameObject;
        private int defaultStorageHeight = 5;
        private int defaultStorageWidth = 4;
        public override List<VehicleStorage> InnateStorages
        {
            get
            {
                var list = new List<VehicleStorage>();
                VehicleStorage thisVS = new VehicleStorage();
                Transform thisStorage = transform.Find("ChassiTop");
                thisVS.Container = thisStorage.gameObject;
                thisVS.Height = defaultStorageHeight;
                thisVS.Width = defaultStorageWidth;
                list.Add(thisVS);
                return list;
            }
        }
        public override List<VehicleStorage> ModularStorages => null;
        public override List<VehicleUpgrades> Upgrades
        {
            get
            {
                var list = new List<VehicleUpgrades>();
                VehicleUpgrades vu = new VehicleUpgrades();
                vu.Interface = transform.Find("BackLeft").gameObject;
                vu.Flap = vu.Interface;
                list.Add(vu);
                return list;
            }
        }
        public override List<VehicleBattery> Batteries
        {
            get
            {
                var list = new List<VehicleBattery>();
                VehicleBattery vb1 = new VehicleBattery();
                vb1.BatterySlot = transform.Find("BackRight").gameObject;
                list.Add(vb1);
                return list;
            }
        }
        public override List<VehicleFloodLight> HeadLights
        {
            get
            {
                var list = new List<VehicleFloodLight>();
                VehicleFloodLight leftLight = new VehicleFloodLight
                {
                    Light = transform.Find("lights_parent/headlights/left").gameObject,
                    Angle = 100,
                    Color = Color.white,
                    Intensity = 0.65f,
                    Range = 30f
                };
                list.Add(leftLight);
                VehicleFloodLight rightLight = new VehicleFloodLight
                {
                    Light = transform.Find("lights_parent/headlights/right").gameObject,
                    Angle = 100,
                    Color = Color.white,
                    Intensity = 0.65f,
                    Range = 30f
                };
                list.Add(rightLight);
                return list;
            }
        }
        public override List<GameObject> WaterClipProxies => null;
        public override List<GameObject> CanopyWindows => null;
        public override BoxCollider BoundingBoxCollider => transform.Find("BoundingBox").gameObject.GetComponent<BoxCollider>();
        public override Dictionary<TechType, int> Recipe
        {
            get
            {
                Dictionary<TechType, int> recipe = new Dictionary<TechType, int>();
                recipe.Add(TechType.Titanium, 4);
                recipe.Add(TechType.PowerCell, 1);
                recipe.Add(TechType.Glass, 1);
                recipe.Add(TechType.Lubricant, 1);
                recipe.Add(TechType.Lead, 1);
                recipe.Add(TechType.ComputerChip, 1);
                return recipe;
            }
        }
        public override UnityEngine.Sprite PingSprite => MainPatcher.assets.ping;
        public override Sprite SaveFileSprite => MainPatcher.saveSprite;
        public override UnityEngine.Sprite CraftingSprite => MainPatcher.assets.crafter;
        public override string Description =>  "A small drone with powerful claws capable of collecting resources.";
        public override string EncyclopediaEntry
        {
            get
            {
                /*
                 * The Formula:
                 * 2 or 3 sentence blurb
                 * Features
                 * Advice
                 * Ratings
                 * Kek
                 */
                string ency = "The Crush is a remotely controlled drone designed for resource collection. ";
                ency += "Its powerful claws are what earned it its name. \n";
                ency += "\nIt features:\n";
                ency += "- Remote Connectivity \n";
                ency += "- Powerful claws for collecting resources \n";
                ency += "- One power cell capacity. \n";

                ency += "\nRatings:\n";
                ency += "- Top Speed: 11m/s \n";
                ency += "- Acceleration: 6m/s/s \n";
                ency += "- Distance per Power Cell: 7km \n";
                ency += "- Crush Depth: 300 \n";
                ency += "- Max Crush Depth (upgrade required): 1100 \n";
                ency += "- Upgrade Slots: 4 \n";
                ency += "- Dimensions: 3.5m x 3.5m x 3.1m \n";

                ency += "- Persons: 0\n";
                ency += "\n\"You can count on Crush.\" ";
                return ency;
            }
        }
        public override int BaseCrushDepth => 300;
        public override int CrushDepthUpgrade1 => 200;
        public override int CrushDepthUpgrade2 => 600;
        public override int CrushDepthUpgrade3 => 600;
        public override int MaxHealth => 250;
        public override int Mass => 500;
        public override int NumModules => 2;
        public override bool HasArms => true;
        public override VFEngine VFEngine => gameObject.EnsureComponent<CrushEngine>();
        public override VehicleArmsProxy Arms => new VehicleArmsProxy
        {
            leftArmPlacement = transform.Find("LeftArmPlace"),
            rightArmPlacement = transform.Find("RightArmPlace"),
            originalLeftArm = transform.Find("Robot3/body.Low").gameObject,
            originalRightArm = transform.Find("Robot3/body.Low.001").gameObject
        };
        public override void Start()
        {
            base.Start();
            SetupMagnetBoots();
        }
        public void SetupMagnetBoots()
        {
            var magBoots = gameObject.EnsureComponent<VehicleFramework.VehicleComponents.MagnetBoots>();
            magBoots.MagnetDistance = 0.1f;
            magBoots.Attach = Attach;
            magBoots.Detach = Detach;
            magBoots.recharges = true;
            magBoots.rechargeRate = 0.5f;
        }
        public override void OnUpgradeModuleChange(int slotID, TechType techType, bool added)
        {
            base.OnUpgradeModuleChange(slotID, techType, added);
            UWE.CoroutineHost.StartCoroutine(UpdateStorageSize());
        }
        public void Attach()
        {
            gameObject.EnsureComponent<VehicleFramework.VehicleComponents.VFArmsManager>().ShowArms(false);
        }
        public void Detach()
        {
            gameObject.EnsureComponent<VehicleFramework.VehicleComponents.VFArmsManager>().ShowArms(true);
        }

        public void CrushStorageModuleAction(int slotID, TechType techType, bool added)
        {
            UWE.CoroutineHost.StartCoroutine(UpdateStorageSize());
        }
        private IEnumerator UpdateStorageSize()
        {
            yield return null; // wait one frame for the removed storage modules to go away
            int storageClass = 0;
            ModularStorageInput.GetAllModularStorageContainers(this).ForEach(x => storageClass += GetStorageSizeClass(x));
            int desiredWidth = Math.Min(defaultStorageWidth + storageClass, 8);
            int desiredHeight = Math.Min(defaultStorageHeight + storageClass, 10);
            InnateStorages.First().Container.GetComponent<InnateStorageContainer>()
                .Container.Resize(desiredWidth, desiredHeight);
        }
        private int GetStorageSizeClass(ItemsContainer cont)
        {
            var storageRootModule = cont?.tr?.parent?.gameObject;
            if(storageRootModule == null)
            {
                return 0;
            }
            string name = storageRootModule.name;
            if (name.Contains("2"))
            {
                return 2;
            }
            if (name.Contains("3"))
            {
                return 3;
            }
            if (name.Contains("4"))
            {
                return 4;
            }
            if (name.Contains("5"))
            {
                return 5;
            }
            return 1;
        }
    }
}
