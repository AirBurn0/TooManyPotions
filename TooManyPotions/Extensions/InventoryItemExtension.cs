using PotionCraft.ScriptableObjects;
using PotionCraft.ScriptableObjects.Ingredient;
using PotionCraft.ScriptableObjects.Salts;
using UnityEngine;

namespace TooManyPotions.Extensions
{
	public static class InventoryItemExtension
	{
		public static Sprite GetIcon(this InventoryItem item)
		{
			switch (item)
			{
				case Salt salt:
					return salt.smallIcon;
				case Ingredient ingredient:
					return ingredient.smallIcon;
				default:
					return item.inventoryIconObject;
			}
		}
	}
}
