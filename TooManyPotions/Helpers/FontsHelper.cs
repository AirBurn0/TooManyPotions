using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace TooManyPotions.Helpers
{
	public static class FontsHelper
	{
		private readonly static Dictionary<string, TMP_FontAsset> fontsDictionary = new Dictionary<string, TMP_FontAsset>();

		static FontsHelper()
		{
			ModInfo.Log("Loading Fonts");
			RequestFonts(
				Enumerable.Empty<string>()
				.Append("Vollkorn-PC-Numbers Bold SDF")
				.Append("Vollkorn-PC SemiBold SDF")
			);
			ModInfo.Log($"Loaded {fontsDictionary.Count} Fonts");
		}

		public static TMP_FontAsset RequestFont(string name)
		{
			TMP_FontAsset font;
			if (fontsDictionary.TryGetValue(name, out font))
				return font;

			font = System.Array.Find<TMP_FontAsset>(Resources.FindObjectsOfTypeAll<TMP_FontAsset>(), s => s.name?.Equals(name) ?? false);
			fontsDictionary.Add(name, font);

			return font;
		}

		public static void RequestFonts(IEnumerable<string> names)
		{
			foreach (TMP_FontAsset font in Resources.FindObjectsOfTypeAll<TMP_FontAsset>().Where(s => names.Contains(s.name) && !fontsDictionary.Keys.Contains(s.name)))
				fontsDictionary.Add(font.name, font);
		}

	}

}
