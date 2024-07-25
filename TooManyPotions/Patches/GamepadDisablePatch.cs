using HarmonyLib;
using TooManyPotions.Displays;

namespace TooManyPotions.Patches
{
	[HarmonyPatch(typeof(PotionCraft.ManagersSystem.Input.ControlProviders.Gamepad), "OnDisable")]
	internal class GamepadDisablePatch
	{
		public static bool Prefix()
		{
			return !DisplayToggler.IsActive;
		}
	}
}