using UnityExplorer.UI;

namespace TooManyPotions.Helpers
{
	public static class UnityExplorerHelper
	{
		public static bool IsExplorerLoaded { get; internal set; }

		public static bool IsFocused => IsExplorerLoaded && GetExplorerState();

		private static bool GetExplorerState()
		{
			var panelTypes = System.Enum.GetValues(typeof(UIManager.Panels));
			foreach (UIManager.Panels panelType in panelTypes)
			{
				if (UIManager.GetPanel(panelType).Enabled)
					return true;
			}
			return false;
		}

	}
}
