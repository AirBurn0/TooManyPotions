using HarmonyLib;
using PotionCraft.ManagersSystem.Application;
using PotionCraft.SceneLoader;
using PotionCraft.Settings;
using System;

namespace TooManyPotions.Patches
{

	[HarmonyPatch(typeof(LoadingQueue), "Add")]
	internal class StartupPatch
	{
		[HarmonyPatch("Add", [typeof(string), typeof(Action)])]
		[HarmonyPrefix]
		public static bool Add(string name, Action action)
		{
			if (GlobalConfigs.IsForceDevMode && name.Equals("InitializeDeveloperMode"))
			{
				Settings<ApplicationManagerSettings>.Asset.developerModeOnStartInBuild = true;
			}
			return true;
		}
	}

}
