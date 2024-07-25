using HarmonyLib;
using PotionCraft.ManagersSystem;
using PotionCraft.ObjectBased.InteractiveItem.InventoryObject;
using PotionCraft.ScriptableObjects;
using PotionCraft.ScriptableObjects.Salts;
using UnityEngine;

namespace TooManyPotions.Scripts.CheatModules
{
	public class ItemDuplicator : MonoBehaviour
	{

		public void Update() // GetMouseButtonDown() works multiple times in FixedUpdate() and it's bad
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
			string find = inventoryObject.ObjectDebugInfo();
			var inventory = inventoryObject.itemsPanel.Inventory;
			InventoryItem targetItem = Traverse.Create(inventoryObject).Field("inventoryItem").GetValue() as InventoryItem;
			if (targetItem is Salt)
				count *= 1000;
			inventory.AddItem(targetItem, count);
		}

	}

}
