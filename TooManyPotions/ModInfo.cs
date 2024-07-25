using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Logging;
using HarmonyLib;
using System.Linq;
using System.Reflection;
using TooManyPotions.Displays;
using TooManyPotions.Helpers;
using TooManyPotions.Scripts.CheatModules;
using UnityEngine;

namespace TooManyPotions
{
	[BepInPlugin(GUID, MODNAME, VERSION)]
	[BepInProcess("Potion Craft.exe")]
	public class ModInfo : BaseUnityPlugin
	{
		private static new ManualLogSource Logger;
		private const string GUID = "ReuloTeam.TooManyPotions";
		private const string MODNAME = "TooManyPotions";
		private const string VERSION = "1.0.0";

		public void Awake()
		{
			Logger = base.Logger;
			GlobalConfigs.IsForceDevMode = Config.Bind(
											"Game",
											"ForceDevMode",
											false,
											"Determines should game start up in Dev Mode.\t\n Setting this to 'true' will make it impossible to disable DevMode ingame.\t\n Setting this to 'false' still allows to toggle DevMode ingame."
											)
											.Value;
			ReLocalization.Localization.AddLocalizationFor(this);
			HandleUnityExplorer();
			Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
			Logger.LogInfo($"{MODNAME} succesfully loaded");
		}

		private static void HandleUnityExplorer()
		{
			bool isExplorerLoaded = Chainloader.PluginInfos.Any(plugin => plugin.Value.Metadata.Name == "UnityExplorer");
			UnityExplorerHelper.IsExplorerLoaded = isExplorerLoaded;
		}

		public static void InitializeCheatGameObject()
		{
			GameObject cheatHolder = new GameObject("CheatHolder");
			// Display Module
			DisplayToggler.Init();
			// Cheat Modules
			cheatHolder.AddComponent<PotionPositionModifier>();
			cheatHolder.AddComponent<ItemDuplicator>();
			cheatHolder.AddComponent<HaggleAutoplayer>();

			DontDestroyOnLoad(cheatHolder);
		}

		public static void Log(object message, LogLevel level = LogLevel.Info)
		{
			Logger.Log(level, message);
		}
	}
}
