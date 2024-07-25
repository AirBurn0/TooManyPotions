using HarmonyLib;
using PotionCraft.ObjectBased.Haggle;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TooManyPotions.Scripts.CheatModules
{
	public class HaggleAutoplayer : MonoBehaviour
	{

		public void FixedUpdate()
		{
			if (GlobalConfigs.IsHaggleAutoplayAllowed)
				HaggleAutoplay();
		}

		private static void HaggleAutoplay()
		{
			HaggleWindow haggle = HaggleWindow.Instance;
			if (haggle.isPaused || haggle.state == HaggleState.DifficultyNotSelected)
				return;
			List<BonusInfo> bonuses = haggle.currentBonuses;
			Pointer pointer = Traverse.Create(haggle).Field("pointer").GetValue() as Pointer;
			if (pointer == null)
				return;
			BonusInfo bonus = bonuses.FirstOrDefault((BonusInfo info) => Mathf.Abs(info.haggleBonus.Position - pointer.Position) <= info.size / 2f);
			if (bonus == null)
				return;
			int num = bonuses.IndexOf(bonus);
			if (num == 0 || num == bonuses.Count - 1)
				return;
			haggle.bargainButton.OnButtonClicked();
		}

	}

}
