using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TooManyPotions.Patches
{

	[HarmonyPatch(typeof(ScrollRect))]
	internal class ScrollRectPatch
	{
		private static Dictionary<GameObject, Vector2> slidersDictionary = new();

		public static void SetSliderSize(GameObject scrollRectObject, Vector2 size)
		{
			slidersDictionary.Add(scrollRectObject, size);
		}

		[HarmonyPatch("UpdateScrollbars")]
		[HarmonyPostfix]
		public static void Postfix(ScrollRect __instance, ref Scrollbar ___m_HorizontalScrollbar, ref Scrollbar ___m_VerticalScrollbar)
		{
			if (!slidersDictionary.TryGetValue(__instance.gameObject, out Vector2 size))
				return;

			if (___m_HorizontalScrollbar)
			{
				___m_HorizontalScrollbar.size = size.x;
				___m_HorizontalScrollbar.value = __instance.horizontalNormalizedPosition;
			}

			if (___m_VerticalScrollbar)
			{
				___m_VerticalScrollbar.size = size.y;
				___m_VerticalScrollbar.value = __instance.verticalNormalizedPosition;
			}

		}
	}

}
