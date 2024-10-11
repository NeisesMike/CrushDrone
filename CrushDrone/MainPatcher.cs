using HarmonyLib;
using System.Collections;
using System.Collections.Generic;
using BepInEx;
using VehicleFramework.VehicleTypes;
using VehicleFramework;
using BiomeData = LootDistributionData.BiomeData;
using VehicleFramework.Assets;
using UnityEngine;

using innateStorage = System.Collections.Generic.List<System.Tuple<System.String, float>>;

namespace CrushDrone
{
    [BepInPlugin("com.mikjaw.subnautica.crush.mod", "CrushDrone", "1.3.0")]
    [BepInDependency(VehicleFramework.PluginInfo.PLUGIN_GUID, VehicleFramework.PluginInfo.PLUGIN_VERSION)]
    [BepInDependency(Nautilus.PluginInfo.PLUGIN_GUID)]
    public class MainPatcher : BaseUnityPlugin
    {
        internal static CrushConfig config { get; private set; }
        public static TechType CrushArmTechType;
        public static VehicleFramework.Assets.VehicleAssets assets;
        public void Awake()
        {
            GetAssets();
        }
        public void Start()
        {
            config = Nautilus.Handlers.OptionsPanelHandler.RegisterModOptions<CrushConfig>();
            var harmony = new Harmony("com.mikjaw.subnautica.crush.mod");
            harmony.PatchAll();
            UWE.CoroutineHost.StartCoroutine(Register());
        }
        public static TechType RegisterCrushArmFragment(ModVehicle unlockVehicle)
        {
            const string classID = "CrushArmFragment";
            const string displayName = "Crush Arm Fragment";
            const string description = "A Scannable fragment of the Crush Drone";

            List<Vector3> spawnLocations = new List<Vector3>
            {
                new Vector3 (-911.0f, -156.3f, 551.5f),
                new Vector3 (-862.6f, -72.1f, 566.0f),
                new Vector3 (-959.0f, -189.9f, 499.2f),
                new Vector3 (-780.5f, -163.0f, 644.4f),
                new Vector3 (-1085.4f, -187.9f, 789.4f),
                new Vector3 (-1086.0f, -218.3f, 777.9f),
                new Vector3 (-1035.1f, -208.3f, 766.5f),
                new Vector3 (-846.4f, -200.3f, 1023.3f),
                new Vector3 (-835.4f, -203.5f, 955.9f),
                new Vector3 (-839.7f, -218.9f, 1033.6f),
                new Vector3 (-761.6f, -195.9f, 797.6f),
                new Vector3 (-662.0f, -118.0f, 785.7f),
                new Vector3 (-694.3f, -182.5f, 614.6f),
                new Vector3 (-737.0f, -200.1f, 851.3f),
                new Vector3 (-835.3f, -197.7f, 772.9f),
                new Vector3 (-742.4f, -171.8f, 755.2f)
            };
            FragmentData fragData = new FragmentData
            {
                fragment = assets.fragment,
                toUnlock = unlockVehicle.GetComponent<TechTag>().type,
                fragmentsToScan = 3,
                scanTime = 5f,
                classID = classID,
                displayName = displayName,
                description = description,
                spawnLocations = spawnLocations,
                spawnRotations = null,
                encyKey = "Crush"
            };
            return FragmentManager.RegisterFragment(fragData);
        }
        public static void GetAssets()
        {
            assets = AssetBundleInterface.GetVehicleAssetsFromBundle("crush", "Crush", "SpriteAtlas", "DronePing", "CrafterSprite", "ArmFragment", "UnlockSprite");
        }
        public static IEnumerator Register()
        {
            Drone crush = assets.model.EnsureComponent<Crush>() as Drone;
            yield return UWE.CoroutineHost.StartCoroutine(VehicleRegistrar.RegisterVehicle(crush));
            CrushArmTechType = RegisterCrushArmFragment(crush);

            //Nautilus.Handlers.StoryGoalHandler.RegisterBiomeGoal("CrushMushroomForest", Story.GoalType.Story, biomeName: "mushroomForest", minStayDuration: 3f, delay: 3f);
            //Nautilus.Handlers.StoryGoalHandler.RegisterBiomeGoal("CrushJellyShroom", Story.GoalType.Story, biomeName: "jellyshroomCaves", minStayDuration: 3f, delay: 3f);
            //Nautilus.Handlers.StoryGoalHandler.RegisterCompoundGoal("CrushBiomes", Story.GoalType.PDA, delay: 3f, new string[]{ "CrushMushroomForest", "CrushJellyShroom" });

            /*
            Nautilus.Handlers.StoryGoalHandler.RegisterBiomeGoal("CrushBiomes", Story.GoalType.Radio, biomeName: "mushroomForest", minStayDuration: 3f, delay: 3f);
            Nautilus.Handlers.StoryGoalHandler.RegisterBiomeGoal("CrushBiomes", Story.GoalType.Radio, biomeName: "jellyshroomCaves", minStayDuration: 3f, delay: 3f);
            Nautilus.Handlers.PDAHandler.AddLogEntry("CrushBiomes", "CrushBiomes", "soundpath", null); // TODO add a sound
            Nautilus.Handlers.LanguageHandler.SetLanguageLine("CrushBiomes", "You did it", "English");
            Nautilus.Handlers.StoryGoalHandler.RegisterCustomEvent("CrushBiomes", () =>
            {
            });
            */

            /*
            var but = UWE.PrefabDatabase.GetPrefabForFilenameAsync("WorldEntities/Environment/Wrecks/life_pod_exploded_13.prefab");
            but = UWE.PrefabDatabase.GetPrefabForFilenameAsync("WorldEntities/Environment/Wrecks/life_pod_exploded_12.prefab");
            but = UWE.PrefabDatabase.GetPrefabForFilenameAsync("WorldEntities/Environment/Wrecks/life_pod_exploded_3.prefab");
            but = UWE.PrefabDatabase.GetPrefabForFilenameAsync("WorldEntities/Environment/Wrecks/life_pod_exploded_19.prefab");
            but = UWE.PrefabDatabase.GetPrefabForFilenameAsync("WorldEntities/Environment/Wrecks/life_pod_exploded_17.prefab");
            but = UWE.PrefabDatabase.GetPrefabForFilenameAsync("WorldEntities/Environment/Wrecks/life_pod_exploded_2.prefab");
            but = UWE.PrefabDatabase.GetPrefabForFilenameAsync("WorldEntities/Environment/Wrecks/life_pod_exploded_6.prefab");
            but = UWE.PrefabDatabase.GetPrefabForFilenameAsync("WorldEntities/Environment/Wrecks/life_pod_exploded_7.prefab");
            but = UWE.PrefabDatabase.GetPrefabForFilenameAsync("WorldEntities/Environment/Wrecks/life_pod_exploded_4.prefab");
            */
        }

    }

    [Nautilus.Options.Attributes.Menu("Crush Drone Options")]
    public class CrushConfig : Nautilus.Json.ConfigFile
    {
        [Nautilus.Options.Attributes.Toggle("Fragment Experience", Tooltip = "Leave checked for the fragment experience.\nLeave unchecked to unlock Crush automatically.\nMust reboot Subnautica to take effect.")]
        public bool isFragmentExperience = true;
    }
}
