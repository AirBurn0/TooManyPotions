using HarmonyLib;
using PotionCraft.ObjectBased.RecipeMap.RecipeMapItem.Zones;

namespace TooManyPotions.Patches
{

	[HarmonyPatch(typeof(DangerZonePart), "GetHpChange")]
	internal class DangerZonePatch
	{
		public static bool Prefix()
		{
			return !GlobalConfigs.IsDangerZonesDisabled;
		}
	}

}
