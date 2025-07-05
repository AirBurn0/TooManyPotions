using PotionCraft.DebugObjects.DebugWindows.Buttons;
using UnityEngine;

namespace TooManyPotions.Displays
{
	static class DisplayToggler
	{
		public static bool IsActive => _cheatMenuObject.activeSelf;
		private static CheatMenuDisplay _cheatMenu;
		private static PotionEditorDisplay? _effectsMenu;
		private static MaximizeButton _maximize;
		private static GameObject _cheatMenuObject;
		private static GameObject? _effectsMenuObject;

		static DisplayToggler()
		{
			_cheatMenu = CheatMenuDisplay.Init();
			_effectsMenu = PotionEditorDisplay.Init();

			_cheatMenuObject = _cheatMenu.Window.gameObject;
			_maximize = _cheatMenuObject.transform.Find("Minimized/Head/Maximize").GetComponent<MaximizeButton>();

			_effectsMenuObject = _effectsMenu?.Window.gameObject;
			_effectsMenuObject?.SetActive(false);
			_cheatMenu.potionEffectMenuOpenHandler = () => _effectsMenuObject?.SetActive(true);

			GlobalConfigs.toggleMenu.onJustUppedEvent.AddListener(ToggleMenu);
		}

		public static void Init() // https://stackoverflow.com/questions/11520829/explicitly-call-static-constructor
		{
		}


		private static void ToggleMenu()
		{
			if (_maximize?.isActiveAndEnabled ?? false)
			{
				_effectsMenuObject?.SetActive(false);
				_maximize.OnButtonReleasedPointerInside();
				return;
			}
			_cheatMenuObject.SetActive(!IsActive);
		}

	}

}
