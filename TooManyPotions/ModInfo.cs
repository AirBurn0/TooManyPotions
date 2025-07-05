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
		private const string GUID = "ReuloTeam.TooManyPotions";
		private const string MODNAME = "TooManyPotions";
		private const string VERSION = "2.0.0";
		private static new ManualLogSource Logger = new(MODNAME);

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

		public static GameObject? CheatHolder;

		public static void InitializeCheatGameObject()
		{
			CheatHolder = new("CheatHolder");
			DisplayToggler.Init();
			DontDestroyOnLoad(CheatHolder);
		}

		public static void Log(object message, LogLevel level = LogLevel.Info)
		{
			Logger.Log(level, message);
		}
		
	}
}
