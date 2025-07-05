using System.Threading;
using PotionCraft.ManagersSystem;
using PotionCraft.ObjectBased.InteractiveItem.InventoryObject;
using PotionCraft.ScriptableObjects;
using PotionCraft.ScriptableObjects.Salts;
using UnityEngine;

namespace TooManyPotions.Scripts.CheatModules
{
	public class ItemDuplicator : CheatBehaviour<ItemDuplicator>
	{
		private readonly float PRESS_TIME = 1f;
		private readonly float DELAY_TIME = 0.125f;
		private float timer = -1f;

		private void OnEnable()
		{
			GlobalConfigs.duplicateInInventory.onJustDownedEvent.AddListener(StartTimer);
			GlobalConfigs.duplicateInInventory.onJustUppedEvent.AddListener(EndTimer);
		}

		private void OnDisable()
		{
			timer = -1;
			GlobalConfigs.duplicateInInventory.onJustDownedEvent.RemoveListener(StartTimer);
			GlobalConfigs.duplicateInInventory.onJustUppedEvent.RemoveListener(EndTimer);
		}

		public void Update()
		{
			if (GlobalConfigs.IsDuplicating)
				DuplicateHoveredInventoryItem(1);
		}

		public void FixedUpdate()
		{
			if (GlobalConfigs.IsDuplicatingMultiple && IsTimerTriggered())
			{
				timer += DELAY_TIME;
				DuplicateHoveredInventoryItem(1);
			}
		}

		private void StartTimer()
		{
			if(GlobalConfigs.IsUEUnfocused)
				timer = Time.time;
		}

		private void EndTimer()
		{
			if(GlobalConfigs.IsUEUnfocused)
				timer = -1f;
		}

		private bool IsTimerTriggered()
		{
			return timer > 0 && Time.time - timer > PRESS_TIME;
		}

		private static void DuplicateHoveredInventoryItem(int count)
		{
			InventoryObject? inventoryObject = Managers.Cursor?.hoveredInteractiveItem as InventoryObject;
			if (inventoryObject == null)
				return;
			InventoryItem targetItem = inventoryObject.InventoryItem;
			if (targetItem is Salt)
				count *= 1000;
			inventoryObject.ItemsPanel.Inventory.AddItem(targetItem, count);
		}

	}

}
