using HarmonyLib;
using PotionCraft.InventorySystem;
using PotionCraft.ManagersSystem;
using PotionCraft.ObjectBased.InteractiveItem.InventoryObject;
using PotionCraft.ScriptableObjects;
using PotionCraft.ScriptableObjects.Salts;
using UnityEngine;

namespace TooManyPotions.Scripts.CheatModules
{
	public class ItemDuplicator : MonoBehaviour
	{

		public void Update()
		{
			if (GlobalConfigs.IsDuplicateOnClickAllowed && GlobalConfigs.IsDuplicating)
				DuplicateHoveredInventoryItem(1);
		}

		public void FixedUpdate()
		{
			if (GlobalConfigs.IsDuplicateOnClickAllowed && GlobalConfigs.IsDuplicatingMultiple)
				DuplicateHoveredInventoryItem(100);
		}

		private static void DuplicateHoveredInventoryItem(int count)
		{
			InventoryObject inventoryObject = Managers.Cursor?.hoveredInteractiveItem as InventoryObject;
			if (inventoryObject == null)
				return;
			ItemsPanel itemsPanel = Traverse.Create(inventoryObject).Property("ItemsPanel").GetValue() as ItemsPanel;
			InventoryItem targetItem = Traverse.Create(inventoryObject).Property("InventoryItem").GetValue() as InventoryItem;
			if (targetItem is Salt)
				count *= 1000;
			itemsPanel.Inventory.AddItem(targetItem, count);
		}

	}

}
