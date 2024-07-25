using PotionCraft.ManagersSystem;
using UnityEngine;

namespace TooManyPotions.Scripts.CheatModules
{
	public class PotionPositionModifier : MonoBehaviour
	{
		public static bool CanModify => GlobalConfigs.IsPositionModifyingAllowed && IsCursorOverMap();
		private static Vector2 CursorPosition => Managers.RecipeMap.recipeMapObject.transmitterWindow.ViewToCamera(Managers.Cursor.cursor.transform.position);

		public void Update()
		{
			if (!CanModify)
				return;
			if (GlobalConfigs.IsRotating)
				RotatePotionToCursor();
			if (GlobalConfigs.IsTeleporting)
				TranslatePotionToCursor();
		}

		public static bool IsCursorOverMap()
		{
			if (Managers.Cursor == null)    // game init / exit
				return false;
			return Managers.Cursor.grabbedInteractiveItem == null
				&& Managers.RecipeMap.recipeMapObject.visibilityZoneCollider.OverlapPoint(Managers.Cursor.cursor.transform.position);
		}

		public static void TranslatePotionToCursor()
		{
			Managers.RecipeMap.indicator.SetPositionOnMap(Managers.RecipeMap.currentMap.transform.InverseTransformPoint(CursorPosition));
		}

		public static void RotatePotionToCursor()
		{
			Vector2 distance = CursorPosition - (Vector2)Managers.RecipeMap.indicator.transform.position;
			Managers.RecipeMap.indicatorRotation.RotateTo(Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg - 90f);
		}

	}

}
