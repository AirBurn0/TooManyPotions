using HarmonyLib;
using PotionCraft.ManagersSystem;
using PotionCraft.ManagersSystem.Trade;
using PotionCraft.ObjectBased.Haggle;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TooManyPotions.Scripts.CheatModules
{
	public class HaggleAutoplayer : MonoBehaviour
	{

		private static HaggleWindow window => HaggleWindow.Instance;
		private static TradeManager.HaggleSubManager manager => Managers.Trade.haggle;

		public void FixedUpdate()
		{
			if (GlobalConfigs.IsHaggleAutoplayAllowed)
				HaggleAutoplay();
		}

		private static void HaggleAutoplay()
		{
			if (window == null || window.IsPaused || manager == null || manager.HaggleState == HaggleState.DifficultyNotSelected) 
				return;
			List<BonusInfo> bonuses = manager.haggleCurrentBonuses;
			BonusInfo bonus = bonuses.FirstOrDefault((BonusInfo info) => Mathf.Abs(info.haggleBonus.Position - manager.pointerPosition) <= info.size / 2f);
			if (bonus == null)
				return;
			int num = bonuses.IndexOf(bonus);
			if (num == 0 || num == bonuses.Count - 1)
				return;
			window.bargainButton.OnButtonClicked();
		}

	}

}
