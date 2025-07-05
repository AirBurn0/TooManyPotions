using System;
using PotionCraft.ManagersSystem;
using PotionCraft.Utils;
using UnityEngine;
using UnityEngineInternal;

namespace TooManyPotions.Scripts.CheatModules
{
	public class PotionPositionModifier : CheatBehaviour<PotionPositionModifier>
	{
		private static Vector2 CursorPosition => Managers.Cursor.cursor.transform.position;
		private static Vector2 CursorMapPosition => Managers.RecipeMap.currentMap.referencesContainer.transform.InverseTransformPoint(Managers.RecipeMap.recipeMapObject.transmitterWindow.ViewToCamera(CursorPosition));

		public void Update()
		{
			if (Managers.Cursor == null || Managers.Cursor.grabbedInteractiveItem != null || !Managers.RecipeMap.recipeMapObject.visibilityZoneCollider.OverlapPoint(CursorPosition))
				return;
			if (GlobalConfigs.IsRotating)
				RotatePotionToCursor();
			if (GlobalConfigs.IsTeleporting)
				TranslatePotionToCursor();
		}

		public static void TranslatePotionToCursor()
		{
			Managers.RecipeMap.indicator.SetPositionOnMap(CursorMapPosition);
		}

		public static void RotatePotionToCursor()
		{
			Managers.RecipeMap.path.pathHints.RemoveAll(hint => hint.ingredient == null); // Yup, they still can be!!
			Vector2 distance = CursorMapPosition - (Vector2)Managers.RecipeMap.recipeMapObject.indicatorContainer.localPosition;
			Managers.RecipeMap.indicatorRotation.RotateTo(Mathf.Atan2(-distance.x, distance.y) * Mathf.Rad2Deg);
		}

	}

}
