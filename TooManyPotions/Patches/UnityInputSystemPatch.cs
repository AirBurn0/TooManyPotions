using HarmonyLib;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace TooManyPotions.Patches
{
	[HarmonyPatch(typeof(InputSystemUIInputModule), "Awake")]
	internal class UnityInputSystemPatch
	{
		public static void Postfix(InputSystemUIInputModule __instance)
		{
			__instance.leftClick.action.AddBinding("<Gamepad>/buttonSouth");
		}
	}
}