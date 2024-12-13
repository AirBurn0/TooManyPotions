using System;
using PotionCraft.ScriptableObjects;
using PotionCraft.ScriptableObjects.AlchemyMachineProducts;
using PotionCraft.ScriptableObjects.BuildableInventoryItem;
using PotionCraft.ScriptableObjects.Ingredient;
using PotionCraft.ScriptableObjects.Salts;
using TooManyPotions.Helpers;
using UnityEngine;

namespace TooManyPotions.Extensions
{
	public static class InventoryItemExtension
	{
		public static (Sprite, Vector2) GetIconData(this InventoryItem item)
		{
			switch (item)
			{
				case Ingredient ingredient:
					return (ingredient.smallIcon, new Vector2(5f, 0.0f));
				case Salt salt:
					return (salt.smallIcon, new Vector2(5f, 0.0f));
				case AlchemyMachineProduct crystal:
					return (crystal.GetActiveMarkerIcon(), Vector2.zero);
				case Seed semen:
					return (semen.smallIcon, new Vector2(5f, 0.0f));
				case Furniture furniture:
					SpriteRenderer[] renderers = furniture.prefab.visualObjectController.renderersToOutline;
					return (renderers[renderers.Length - 1].sprite, Vector2.zero);
				default:
					return (item.GetInventoryIcon(), Vector2.zero);
			}
		}
	}
}
