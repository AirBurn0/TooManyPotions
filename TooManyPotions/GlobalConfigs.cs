using PotionCraft.InputSystem;
using TooManyPotions.Helpers;
using TooManyPotions.Scripts.CheatModules;
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
			get
			{
				return PotionPositionModifier.Instance?.enabled ?? false;
			}
			set
			{
				MonoBehaviour? field = PotionPositionModifier.Instance;
				if (field != null)
					field.enabled = value;
			}
		}

		public static bool IsDangerZonesDisabled
		{
			get;
			set;
		}
		public static bool IsDuplicateOnClickAllowed
		{
			get
			{
				return ItemDuplicator.Instance?.enabled ?? false;
			}
			set
			{
				MonoBehaviour? field = ItemDuplicator.Instance;
				if (field != null)
					field.enabled = value;
			}
		}
		public static bool IsHaggleAutoplayAllowed
		{
			get
			{
				return HaggleAutoplayer.Instance?.enabled ?? false;
			}
			set
			{
				MonoBehaviour? field = HaggleAutoplayer.Instance;
				if (field != null)
					field.enabled = value;
			}
		}
		public static bool IsEffectOverflowAllowed
		{
			get;
			set;
		}
		public static Command toggleMenu = new("Toggle Menu Panel",
		[
			new([GamepadTrigger.Get(GamepadTrigger.Side.Left, 0.02f), GamepadTrigger.Get(GamepadTrigger.Side.Right, 0.02f)]),
			new([KeyboardKey.Get(KeyCode.O)])
		], false);
		public static Command rotatePotion = new("Rotate potion to cursor",
		[
			new([GamepadButton.Get(GamepadButton.ButtonCode.X)]),
			new([KeyboardKey.Get(KeyCode.LeftControl)])
		], false);
		public static Command teleportPotion = new("Teleport potion to cursor",
		[
			new([GamepadButton.Get(GamepadButton.ButtonCode.Y)]),
			new([MouseButton.Get(2)])
		], false);
		public static Command duplicateInInventory = new("Duplicate item in inventory when clicked once, or multiple times if hold",
		[
			new([GamepadButton.Get(GamepadButton.ButtonCode.LeftStick)]),
			new([MouseButton.Get(0), KeyboardKey.Get(KeyCode.LeftShift)]),
			new([MouseButton.Get(0), KeyboardKey.Get(KeyCode.RightShift)]),
			new([MouseButton.Get(1), KeyboardKey.Get(KeyCode.LeftShift)]),
			new([MouseButton.Get(1), KeyboardKey.Get(KeyCode.RightShift)])
		], false);
        public static bool IsInState(Command command, State state) => command.State == state;
		public static bool IsUEUnfocused => !UnityExplorerHelper.IsFocused;
		public static bool IsRotating => IsUEUnfocused && IsInState(rotatePotion, State.Downed);
		public static bool IsTeleporting => IsUEUnfocused && IsInState(teleportPotion, State.Downed);
		public static bool IsDuplicating => IsUEUnfocused && IsInState(duplicateInInventory, State.JustDowned);
		public static bool IsDuplicatingMultiple => IsUEUnfocused && IsInState(duplicateInInventory, State.Downed);

	}

}
