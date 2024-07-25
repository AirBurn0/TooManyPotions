using HarmonyLib;
using TooManyPotions.Displays;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TooManyPotions.Patches
{
	[HarmonyPatch(typeof(PotionCraft.ManagersSystem.Input.ControlProviders.GamepadFreeCursorPositionUpdater), "UpdateCursorPosition")]
	internal class GamepadCursorPatch
	{
		public static Vector2 Postfix(Vector2 currentMouseWorldPosition)
		{
			if (!DisplayToggler.IsActive)
				return currentMouseWorldPosition;
			Vector2 newPosition = Camera.current.WorldToScreenPoint(currentMouseWorldPosition);
			Mouse.current.WarpCursorPosition(newPosition);
			return currentMouseWorldPosition;
		}
	}
}