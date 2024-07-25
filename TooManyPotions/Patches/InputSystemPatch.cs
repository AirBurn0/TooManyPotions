using HarmonyLib;
using PotionCraft.ManagersSystem.Input;

namespace TooManyPotions.Patches
{

	[HarmonyPatch(typeof(InputManager), "HasInputGotToBeDisabled")]
	internal class InputSystemPatch
	{
		public static void Postfix(ref bool __result)
		{
			__result = __result || GlobalConfigs.IsUnfocused;
		}
	}

}
