namespace TooManyPotions.Controls.Factories
{
	public abstract class ControlsFactory
	{
		public static EasierUI.Controls.ControlsFactory Instance => PotionCraftControlsFactory.Instance;
	}

}
