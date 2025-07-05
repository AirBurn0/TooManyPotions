using EasierUI.Controls.Contrainers;
using PotionCraft.LocalizationSystem;
using PotionCraft.ManagersSystem;
using PotionCraft.ManagersSystem.Debug;
using PotionCraft.ScriptableObjects;
using PotionCraft.ScriptableObjects.Salts;
using System;
using System.Globalization;
using TMPro;
using TooManyPotions.Controls.Factories;
using TooManyPotions.Extensions;
using TooManyPotions.Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace TooManyPotions.Displays
{
	internal class CheatMenuDisplay : AbstractDisplay
	{
		const int preferedWidth = 200;
		public Action? potionEffectMenuOpenHandler;

		#region Controls
		private InputFieldContrainer? _goldInput, _popularityInput, _experienceInput;
		private SliderContrainer? _karmaSlider;
		private ScrollContainer? _itemsScroll;
		private ToggleContrainer? _devmodeToggle, _teleportToggle, _nodamageToggle, _dupingToggle, _autohaggleToggle;
		private ButtonContrainer? _button;
		#endregion

		public static CheatMenuDisplay Init()
		{
			return Init<CheatMenuDisplay>("#cheat_menu_title", "Cheat");
		}

		protected override void SetupElements()
		{
			SetupGoldInput();
			SetupPopularityInput();
			SetupExperienceInput();
			SetupKarmaSlider();
			SetupItemsScroll();
			
			SetupDevmodeToggle();
			SetupTeleportationToggle();
			SetupNodamageToggle();
			SetupDupingToggle();
			SetupAutoHaggleToggle();
			
			SetupPotionEditWindowButton();
		}

		#region SetupControls

		private GameObject CreateLayout(string name)
		{
			GameObject layout = new(name);
			layout.transform.SetParent(_panel.RectTransform);

			LayoutElement layoutElementHolder = layout.AddComponent<LayoutElement>();
			layoutElementHolder.preferredWidth = preferedWidth;

			HorizontalLayoutGroup layoutGroup = layout.AddComponent<HorizontalLayoutGroup>();
			layoutGroup.childForceExpandWidth = false;
			layoutGroup.spacing = 4f;

			ContentSizeFitter fitter = layout.AddComponent<ContentSizeFitter>();
			fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
			return layout;
		}

		private void CreateIconOnLayout(Transform parent, string objectName, int size = 32)
		{
			CreateIconOnLayout(parent, objectName, SpritesHelper.GetByName(objectName), size, size);
		}

		private InputFieldContrainer CreateInputField(Transform parent, Sprite sprite, Action<string> action, string objectName, string text, TMP_InputField.ContentType type = TMP_InputField.ContentType.IntegerNumber)
		{
			InputFieldContrainer field = ControlsFactory.Instance.CreateInputField(parent, objectName, action, type, text);
			field.GameObject.AddComponent<LayoutElement>().flexibleWidth = 1;
			field.BackgroundImage.sprite = sprite;
			return field;
		}

		public ScrollContainer CreateVerticalScroll(Transform parent, string objectName)
		{
			ScrollContainer scroll = ControlsFactory.Instance.CreateVerticalScroll(parent, objectName);

			CreateCorner(scroll, "Left", new(0.03f, 0.985f), new(0f, 0.035f));
			CreateCorner(scroll, "Top", new(0.99f, 1.01f), new(0f, 0.98f));
			CreateCorner(scroll, "Right", new(1f, 1f), new(0.97f, 0.035f));
			CreateCorner(scroll, "Bottom", new(1f, 0.04f), new(0f, -0.01f));

			return scroll;
		}

		private static void CreateCorner(ScrollContainer scroll, string sideName, Vector2 max, Vector2 min)
		{
			CreateCorner(scroll.GameObject.transform, $"{sideName} Corner", SpritesHelper.RequestSpriteByName($"InventoryWindow Foreground Var2 {sideName}"), max, min);
		}

		private static void CreateCorner(Transform parent, string name, Sprite sprite, Vector2 max, Vector2 min)
		{
			GameObject corner = new(name, typeof(Image));
			corner.transform.SetParent(parent);
			corner.GetComponent<Image>().sprite = sprite;
			RectTransform transform = (RectTransform)corner.transform;
			transform.sizeDelta = Vector2.zero;
			transform.anchoredPosition = Vector2.zero;
			transform.anchorMax = max;
			transform.anchorMin = min;
		}

		private void SetupGoldInput()
		{
			GameObject goldHolder = CreateLayout("Gold Holder");
			CreateIconOnLayout(goldHolder.transform, "Gold Icon");
			void handler(string value)
			{
				if (int.TryParse(value, out int result))
					PlayerStatsHelper.Gold = result;
			}
			_goldInput = CreateInputField(goldHolder.transform, SpritesHelper.RequestSpriteByName("Gold Plate Idle"), handler, "Gold Input", "0");
			Managers.Player.onGoldChanged.AddListener(() => _goldInput.InputField.text = PlayerStatsHelper.Gold.ToString());
		}

		private void SetupPopularityInput()
		{
			GameObject popularityHolder = CreateLayout("Popularity Holder");
			CreateIconOnLayout(popularityHolder.transform, "PopularityIcon Tier 5");

			_popularityInput = CreateInputField(popularityHolder.transform, SpritesHelper.RequestSpriteByName("Popularity Plate Idle"),value => { if (int.TryParse(value, out int result)) PlayerStatsHelper.Popularity = result; }, "Popularity Input", "0");
			Managers.Player.popularity.onPopularityChanged.AddListener(() => _popularityInput.InputField.text = PlayerStatsHelper.Popularity.ToString());
		}

		private void SetupExperienceInput()
		{
			GameObject experienceHolder = CreateLayout("Experience Holder");
			CreateIconOnLayout(experienceHolder.transform, "XP Icon");

			_experienceInput = CreateInputField(experienceHolder.transform, SpritesHelper.RequestSpriteByName("Karma Plate Idle"), value => { if (float.TryParse(value, out float result)) PlayerStatsHelper.Experience = result; }, "Experience Input", "0", TMP_InputField.ContentType.DecimalNumber);
			_experienceInput.InputField.characterValidation = TMP_InputField.CharacterValidation.Decimal;
			Managers.Player.experience.OnCurrentExpChanged.AddListener(() => _experienceInput.InputField.text = PlayerStatsHelper.Experience.ToString("#0.#"));
		}

		private void SetupKarmaSlider()
		{
			GameObject karmaHolder = CreateLayout("Karma Holder");
			CreateIconOnLayout(karmaHolder.transform, "KarmaIcon Good 3");
			_karmaSlider = ControlsFactory.Instance.CreateSlider(karmaHolder.transform, "Karma Slider", value => PlayerStatsHelper.Karma = (int)value, true, -100, 100);
			LayoutElement layoutElement = _karmaSlider.GameObject.AddComponent<LayoutElement>();
			layoutElement.flexibleWidth = 1;
			Managers.Player.karma.onKarmaChanged.AddListener(() => _karmaSlider.Slider.value = PlayerStatsHelper.Karma);
		}

		private void SetupItemsScroll()
		{
			_itemsScroll = CreateVerticalScroll(_panel.RectTransform, "Items Scroll Selector");
			LayoutElement layoutElement = _itemsScroll.GameObject.AddComponent<LayoutElement>();
			layoutElement.preferredWidth = preferedWidth;
			layoutElement.flexibleWidth = 1;

			int slot = 0;
			foreach (InventoryItem item in PlayerStatsHelper.Items)
			{
				ButtonContrainer newButton = ControlsFactory.Instance.CreateButton(
					_itemsScroll.ContentHolder.transform, item.name, () => PlayerStatsHelper.AddItem(item, item is Salt ? 1000 : 1),
					SpritesHelper.RequestSpriteByName($"Inventory Item Slot {1 + (slot++ % 18)} Normal"),
					true
				);
				Destroy(newButton.Text.gameObject);
				newButton.Image.color = new Color(1f, 1f, 1f, 0.3f);
				var iconData = item.GetIconData();
				ImageContrainer icon = CreateIconOnLayout(newButton.GameObject.transform, item.name + " Icon", iconData.Item1);
				RectTransform transform = icon.GameObject.GetComponent<RectTransform>();
				transform.anchoredPosition = iconData.Item2;
				transform.anchorMax = new(0.9f, 0.9f);
				transform.anchorMin = new(0.1f, 0.1f);
				transform.sizeDelta = Vector2.zero;
			}
		}

		private ToggleContrainer CreateToggle(RectTransform parent, string name, Action<bool> handler, string label)
		{
			ToggleContrainer toggle = ControlsFactory.Instance.CreateToggle(parent, name, handler, label);
			toggle.GameObject.AddComponent<LayoutElement>().preferredWidth = preferedWidth;
			toggle.Text.gameObject.AddComponent<LocalizedText>();
			return toggle;
		}

		private void SetupDevmodeToggle()
		{
			// can be force-set-always true;
			_devmodeToggle = CreateToggle(_panel.RectTransform, "DevMode Toggle", value => DebugManager.IsDeveloperMode = value || GlobalConfigs.IsForceDevMode, "#devmode_toggle_text");
			_devmodeToggle.Toggle.isOn = GlobalConfigs.IsForceDevMode;
		}

		private void SetupTeleportationToggle()
		{
			_teleportToggle = CreateToggle(_panel.RectTransform, "Teleport Toggle", value => GlobalConfigs.IsPositionModifyingAllowed = value, "#position_modifying_toggle_text");
		}

		private void SetupNodamageToggle()
		{
			_nodamageToggle = CreateToggle(_panel.RectTransform, "Godmode Toggle", value => GlobalConfigs.IsDangerZonesDisabled = value, "#disable_damage_toggle_text");
		}

		private void SetupDupingToggle()
		{
			_dupingToggle = CreateToggle(_panel.RectTransform, "Duping Toggle", value => GlobalConfigs.IsDuplicateOnClickAllowed = value, "#inventory_items_dupe_toggle_text");
		}

		private void SetupAutoHaggleToggle()
		{
			// that's just funny
			_autohaggleToggle = CreateToggle(_panel.RectTransform, "Autohaggle Toggle", value => GlobalConfigs.IsHaggleAutoplayAllowed = value, "#haggle_autoplay_toggle_text");
		}

		private void SetupPotionEditWindowButton()
		{
			_button = ControlsFactory.Instance.CreateButton(
				_panel.RectTransform, "Open Potion Editor Button", PotionEditWindowAction,
				SpritesHelper.RequestSpriteByName("SaveRecipe Active Slot"), true, "#effects_editor_button_text"
			);
			_button.Text.gameObject.AddComponent<LocalizedText>();
			_button.Image.color = new Color(1f, 1f, 1f, 0.3f);
			LayoutElement element = _button.GameObject.AddComponent<LayoutElement>();
			element.preferredWidth = preferedWidth;
		}

		#endregion

		private void PotionEditWindowAction()
		{
			potionEffectMenuOpenHandler?.Invoke();
		}

	}

}
