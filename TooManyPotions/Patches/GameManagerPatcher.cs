using HarmonyLib;
using PotionCraft.ManagersSystem.Game;
using PotionCraft.SceneLoader;

namespace TooManyPotions.Patches
{

	[HarmonyPatch(typeof(GameManager), "Start")]
	internal class GameManagerPatcher
	{
		public static void Postfix()
		{
			ObjectsLoader.AddLast("SaveLoadManager.SaveNewGameState", ModInfo.InitializeCheatGameObject);
			ModInfo.Log("Patched start been called");
		}
	}
}
