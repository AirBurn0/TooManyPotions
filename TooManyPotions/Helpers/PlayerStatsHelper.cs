using PotionCraft.ManagersSystem;
using PotionCraft.ObjectBased.RecipeMap;
using PotionCraft.ScriptableObjects;
using PotionCraft.ScriptableObjects.Ingredient;
using PotionCraft.ScriptableObjects.Salts;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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
			items.AddRange(Ingredient.allIngredients);
			items.AddRange(Salt.allSalts);
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
				Managers.Player.inventory.AddItem(item, amount);
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
			Managers.RecipeMap.potionBaseSubManager.UnlockPotionBase(based.potionBase, false, false);
			ModInfo.Log(based.potionBase.name);
			MapStatesManager.SelectMap(based.index, true);
		}

	}
}
