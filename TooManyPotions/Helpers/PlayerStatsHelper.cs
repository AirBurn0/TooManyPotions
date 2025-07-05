using PotionCraft;
using PotionCraft.ManagersSystem;
using PotionCraft.ManagersSystem.Player;
using PotionCraft.ObjectBased.RecipeMap;
using PotionCraft.SceneLoader;
using PotionCraft.ScriptableObjects;
using PotionCraft.ScriptableObjects.AlchemyMachineProducts;
using PotionCraft.ScriptableObjects.BuildableInventoryItem;
using PotionCraft.ScriptableObjects.Ingredient;
using PotionCraft.ScriptableObjects.Salts;
using PotionCraft.ScriptableObjects.TradableUpgrades;
using PotionCraft.ScriptableObjects.WateringPot;
using PotionCraft.Settings;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

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
		public static float Experience
		{
			get => Managers.Player.experience.CurrentExp;
			set
			{
				if (value < 0)
					return;
				if (value > 1e20f) // 1e20 is theoretical xp limit of level with that kind of progression
					value = 1e20f;
				var xp = Managers.Player.experience;
				float step = Settings<PlayerManagerSettings>.Asset.levelStep; // not float but it's better to be float
				int newLvl = Mathf.FloorToInt(0.5f * (Mathf.Sqrt(8f * value / step + 1f) + 1f)); // inverse Arithmetic progression Sum goes brrrr
				int diffLvl = newLvl - xp.currentLvl;
				bool skipTweens = diffLvl > 50 || diffLvl < 0; // getting more than 50 lvls at once can get extremely laggy due to tweens pool
				if (skipTweens || value < xp.CurrentExp)
				{
					xp.currentExp = value;
					if (skipTweens)
					{
						xp.currentLvl = newLvl;
						xp.currentLvlAt = (newLvl - 1) * newLvl / 2 * step;
						xp.nextLvlAt = xp.currentLvlAt + newLvl * step;
						if (diffLvl > 0) // give ppl their talent points
						{
							Managers.Player.talents.currentPoints += diffLvl - 1;
							xp.OnLvlUp?.Invoke();
							xp.CheckLvlGoal();
						}
					}
					xp.OnCurrentExpChanged?.Invoke();
					return;
				}
				xp.AddExperience(value - xp.CurrentExp, ExperienceCategory.Debug);
			}
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
			List<InventoryItem> items =
			[
				.. Ingredient.allIngredients.OrderBy(x => x.GetItemType()),
				.. AlchemyMachineProduct.allProducts.Where(x => !x.name.Contains("Useless") && !x.name.Contains("Salt Pile")), // exclude useless poop and salts
                .. Salt.allSalts,
			];

			var buildable = BuildableInventoryItem.allBuildableItems;
			// ensures order - seeds first, pots and decor in middle, furniture last
			// can be also achieved via BuildableInventoryItem.ForEach call but it's gross
			items.AddRange(buildable[BuildableInventoryItemType.Seed]);
			items.AddRange(WateringPot.AllPots);
			items.AddRange(DecorDynamic.allDecorItems);
			items.AddRange(buildable[BuildableInventoryItemType.Furniture]);
			items.AddRange(TradableUpgrade.allTradableUpgrades);
			// last ones?
			Items = items.AsReadOnly();

			// potion effects
			Effects = PotionEffect.allPotionEffects.AsReadOnly();

			// potion bases
			MapBases = new(MapStatesManager.MapStates);
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
