using HarmonyLib;
using PotionCraft;
using PotionCraft.ManagersSystem;
using PotionCraft.ManagersSystem.Game.Providers;
using PotionCraft.ObjectBased;
using PotionCraft.ObjectBased.RecipeMap;
using PotionCraft.SceneLoader;
using PotionCraft.ScriptableObjects;
using PotionCraft.ScriptableObjects.AlchemyMachineProducts;
using PotionCraft.ScriptableObjects.BuildableInventoryItem;
using PotionCraft.ScriptableObjects.Ingredient;
using PotionCraft.ScriptableObjects.Salts;
using PotionCraft.ScriptableObjects.WateringPot;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TooManyPotions.Helpers
{
	public static class PlayerStatsHelper
	{
		public static int Gold
		{
			get => Managers.Player.Gold;
			set
			{
				if (value < 0)
					return;
				Managers.Player.AddGold(value - Gold);
			}
		}
		public static int Popularity
		{
			get => Managers.Player.popularity.Popularity;
			set => Managers.Player.popularity.Popularity = value;
		}
		public static int Karma
		{
			get => Managers.Player.karma.Karma;
			set => Managers.Player.karma.Karma = value;
		}

		public readonly static ReadOnlyCollection<InventoryItem> Items;

		public readonly static ReadOnlyCollection<PotionEffect> Effects;

		public readonly static ReadOnlyCollection<MapState> MapBases;

		static PlayerStatsHelper()
		{
			// items
			List<InventoryItem> items = new List<InventoryItem>();
			items.AddRange(Ingredient.allIngredients.OrderBy(x => x.GetItemType()));
			items.AddRange(AlchemyMachineProduct.allProducts.Where(x => !x.name.Contains("Useless") && !x.name.Contains("Salt Pile"))); // exclude useless poop and salts
			items.AddRange(Salt.allSalts);

			var buildable = Traverse.Create(typeof(BuildableInventoryItem)).Field("allBuildableItems").GetValue() as Dictionary<BuildableInventoryItemType, List<BuildableInventoryItem>>;
			// ensures order - seeds first, pots and decor in middle, furniture last
			// can be also achieved via BuildableInventoryItem.ForEach call but it's gross
			items.AddRange(buildable[BuildableInventoryItemType.Seed]);

			items.AddRange(Traverse.Create(typeof(WateringPot)).Property("AllPots").GetValue() as List<WateringPot>);
			items.AddRange(DecorDynamic.allDecorItems);

			items.AddRange(buildable[BuildableInventoryItemType.Furniture]);

			// last ones?
			Items = items.AsReadOnly();

			// potion effects
			List<PotionEffect> effects = new List<PotionEffect>();
			effects.AddRange(PotionEffect.allPotionEffects);
			Effects = effects.AsReadOnly();

			// potion bases
			List<MapState> bases = new List<MapState>();
			bases.AddRange(MapStatesManager.MapStates);
			MapBases = bases.AsReadOnly();
		}

		public static void AddItem(InventoryItem item, int amount)
		{
			try
			{
				Managers.Player.Inventory.AddItem(item, amount);
			}
			catch (System.Exception e)
			{
				ModInfo.Log(e.Message, BepInEx.Logging.LogLevel.Warning);
			}
		}


		public static bool CanAddEffect(PotionEffect effect)
		{
			if (Managers.Potion.GetEffectTier(effect) >= 3)
				return false;
			return true;
		}

		public static void AddEffect(PotionEffect effect)
		{
			Managers.Potion.ApplyEffectToPotion(effect, 1);
		}

		public static void SetMapBase(MapState based)
		{
			if (Managers.RecipeMap.potionBaseSubManager.IsBaseUnlocked(based.potionBase))
			{
				SelectMap(based);
				return;
			}
			Managers.RecipeMap.potionBaseSubManager.UnlockPotionBase(based.potionBase, false, false, false);
			Managers.Game.LoadScenes(new List<SceneIndexEnum> { based.potionBase.GetSceneIndexEnum() }, null, () => SelectMap(based));
		}

		private static void SelectMap(MapState based)
		{
			float zoomNow = Managers.RecipeMap.recipeMapObject.GetZoomObject().ZoomNow;
			MapStatesManager.SelectMap(based.index, true);
			Managers.RecipeMap.currentMap.ResetCamState(zoomNow);
			Managers.RecipeMap.potionBaseSubManager.ReadPotionBase(based.potionBase);
		}

	}
}
