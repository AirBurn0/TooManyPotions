using PotionCraft.ManagersSystem;
using PotionCraft.ManagersSystem.Trade;
using PotionCraft.ObjectBased.Haggle;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TooManyPotions.Scripts.CheatModules
{
	public class HaggleAutoplayer : CheatBehaviour<HaggleAutoplayer>
	{
		private static HaggleWindow Window => HaggleWindow.Instance;
		private static TradeManager.HaggleSubManager Manager => Managers.Trade.haggle;

		public void FixedUpdate()
		{
			HaggleAutoplay();
		}

		private static void HaggleAutoplay()
		{
			if (Window == null || Window.IsPaused || Manager == null || Manager.HaggleState == HaggleState.DifficultyNotSelected) 
				return;
			List<BonusInfo> bonuses = Manager.haggleCurrentBonuses;
			BonusInfo bonus = bonuses.FirstOrDefault(info => Mathf.Abs(info.haggleBonus.Position - Manager.pointerPosition) <= info.size / 2f);
			if (bonus == null)
				return;
			int num = bonuses.IndexOf(bonus);
			if (num == 0 || num == bonuses.Count - 1)
				return;
			Window.bargainButton.OnButtonClicked();
		}

	}

}
