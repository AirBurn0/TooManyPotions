using PotionCraft.InputSystem;
using TooManyPotions.Helpers;
using UnityEngine;

namespace TooManyPotions
{
	public static class GlobalConfigs
	{
		public static bool IsForceDevMode // once true - always true
		{
			get => _isForceDevMode;
			set => _isForceDevMode |= value;
		}

		private static bool _isForceDevMode;

		public static bool IsPositionModifyingAllowed
		{
			get;
			set;
		}

		public static bool IsDangerZonesDisabled
		{
			get;
			set;
		}

		public static bool IsDuplicateOnClickAllowed
		{
			get;
			set;
		}

		public static bool IsHaggleAutoplayAllowed
		{
			get;
			set;
		}

		public static bool IsEffectOverflowAllowed
		{
			get;
			set;
		}

		public static Command toggleMenu = new Command("Toggle Menu Panel", new HotKey[]
		{
			new HotKey(new Button[]
			{
				GamepadTrigger.Get(GamepadTrigger.Side.Left, 0.02f),
				GamepadTrigger.Get(GamepadTrigger.Side.Right, 0.02f)
			}),
			new HotKey(new Button[]
			{
				KeyboardKey.Get(KeyCode.O)
			})
		}, false);


		public static Command rotatePotion = new Command("Rotate potion to cursor", new HotKey[]
		{
			new HotKey(new Button[]
			{
				GamepadButton.Get(GamepadButton.ButtonCode.X)
			}),
			new HotKey(new Button[]
			{
				KeyboardKey.Get(KeyCode.LeftControl)
			})
		}, false);

		public static Command teleportPotion = new Command("Teleport potion to cursor", new HotKey[]
		{
			new HotKey(new Button[]
			{
				GamepadButton.Get(GamepadButton.ButtonCode.Y)
			}),
			new HotKey(new Button[]
			{
				MouseButton.Get(2)
			})
		}, false);

		public static Command duplicateInInventory = new Command("Duplicate item in inventory", new HotKey[]
		{
			new HotKey(new Button[]
			{
				GamepadButton.Get(GamepadButton.ButtonCode.LeftStick)
			}),
			new HotKey(new Button[]
			{
				MouseButton.Get(0),
				KeyboardKey.Get(KeyCode.LeftControl)
			})
		}, false);

		public static Command duplicateInInventoryMultiple = new Command("Duplicate item in inventory multiple times", new HotKey[]
		{
			new HotKey(new Button[]
			{
				GamepadButton.Get(GamepadButton.ButtonCode.RightStick)
			}),
			new HotKey(new Button[]
			{
				MouseButton.Get(1),
				KeyboardKey.Get(KeyCode.LeftControl)
			})
		}, false);

		public static bool IsInState(Command command, State state) => command.State == state;
		public static bool IsUnfocused => UnityExplorerHelper.IsFocused;
		public static bool IsRotating => !IsUnfocused && IsInState(rotatePotion, State.Downed);
		public static bool IsTeleporting => !IsUnfocused && IsInState(teleportPotion, State.Downed);
		public static bool IsDuplicating => !IsUnfocused && IsInState(duplicateInInventory, State.JustDowned);
		public static bool IsDuplicatingMultiple => !IsUnfocused && IsInState(duplicateInInventoryMultiple, State.JustDowned);
	}

}
