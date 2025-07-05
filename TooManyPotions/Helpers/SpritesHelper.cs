using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TooManyPotions.Helpers
{
	public static class SpritesHelper
	{
		private readonly static Dictionary<string, List<Sprite>> SpritesDictionary;

		private readonly static Dictionary<string, Sprite> RequestedSpritesDictionary = [];

		static SpritesHelper()
		{
			ModInfo.Log("Loading Sprites");
			var allResources = Resources.LoadAll<Sprite>("Sprite Assets");
			SpritesDictionary = allResources.GroupBy(resource => resource.name).ToDictionary(pair => pair.Key, pair => pair.ToList());
			RequestSprites(
				Enumerable.Range(1, 19).Select(i => $"Inventory Item Slot {i} Normal")
				.Append("InventoryScroller Pointer")
				.Append("InventoryScroller Axis Var1 Active")
				.Append("GoalsTrackPanel Background")
				.Append("InventoryWindow Background Var2")
				.Append("Gold Plate Idle")
				.Append("Popularity Plate Idle")
				.Append("Karma Plate Idle")
				.Append("SaveRecipe Active Slot")
			);
			ModInfo.Log($"Loaded {SpritesDictionary.Count + RequestedSpritesDictionary.Count} Sprites");
		}

		public static List<Sprite>? GetByListName(string name)
		{
			if (SpritesDictionary.ContainsKey(name))
				return SpritesDictionary[name];

			return null;
		}

		public static Sprite? GetByName(string name)
		{
			return GetByListName(name)?.FirstOrDefault();
		}

		public static Sprite RequestSpriteByName(string name)
		{
			Sprite sprite;
			if (RequestedSpritesDictionary.TryGetValue(name, out sprite))
				return sprite;

			sprite = System.Array.Find(Resources.FindObjectsOfTypeAll<Sprite>(), s => s.name?.Equals(name) ?? false);
			RequestedSpritesDictionary.Add(name, sprite);

			return sprite;
		}

		public static void RequestSprites(IEnumerable<string> names)
		{
			foreach (Sprite sprite in Resources.FindObjectsOfTypeAll<Sprite>().Where(s => names.Contains(s.name) && !RequestedSpritesDictionary.Keys.Contains(s.name)))
				RequestedSpritesDictionary.Add(sprite.name, sprite);
		}

	}

}
