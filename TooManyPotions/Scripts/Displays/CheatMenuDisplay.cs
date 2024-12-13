using EasierUI.Controls.Contrainers;
using PotionCraft.LocalizationSystem;
using PotionCraft.ManagersSystem;
using PotionCraft.ManagersSystem.Debug;
using PotionCraft.ScriptableObjects;
using PotionCraft.ScriptableObjects.Salts;
using System;
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
		public Action potionEffectMenuOpenHandler;

		#region Controls
		private InputFieldContrainer _goldInput;
		private InputFieldContrainer _popularityInput;
		private SliderContrainer _karmaSlider;
		private ScrollContainer _itemsScroll;
		private ToggleContrainer _devmodeToggle;
		private ToggleContrainer _teleportToggle;
		private ToggleContrainer _nodamageToggle;
		private ToggleContrainer _dupingToggle;
		private ToggleContrainer _autohaggleToggle;
		private ButtonContrainer _button;
		#endregion

		public static CheatMenuDisplay Init()
		{
			return Init<CheatMenuDisplay>("#cheat_menu_title", "Cheat");
		}

		protected override void SetupElements()
		{
			SetupGoldInput();
			SetupPopularityInput();
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
			GameObject layout = new GameObject(name);
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

		private InputFieldContrainer CreateInputField(Transform parent, Sprite sprite, Action<string> action, string objectName, string text)
		{
			InputFieldContrainer field = ControlsFactory.Instance.CreateInputField(parent, objectName, action, TMP_InputField.ContentType.IntegerNumber, text);
			field.GameObject.AddComponent<LayoutElement>().flexibleWidth = 1;
			field.BackgroundImage.sprite = sprite;
			return field;
		}

		public ScrollContainer CreateVerticalScroll(Transform parent, string objectName)
		{
			ScrollContainer scroll = ControlsFactory.Instance.CreateVerticalScroll(parent, objectName);

			CreateCorner(scroll, "Left", new Vector2(0.03f, 1f), Vector2.zero);
			CreateCorner(scroll, "Top", new Vector2(0.98f, 1.008f), new Vector2(0.02f, 0.984f));
			CreateCorner(scroll, "Right", new Vector2(1f, 0.998f), new Vector2(0.97f, -0.002f));
			CreateCorner(scroll, "Bottom", new Vector2(0.98f, 0.01f), new Vector2(0.02f, -0.006f));

			return scroll;
		}

		private static void CreateCorner(ScrollContainer scroll, string sideName, Vector2 max, Vector2 min)
		{
			CreateCorner(scroll.GameObject.transform, $"{sideName} Corner", SpritesHelper.RequestSpriteByName($"InventoryWindow Foreground Var2 {sideName}"), max, min);
		}

		private static void CreateCorner(Transform parent, string name, Sprite sprite, Vector2 max, Vector2 min)
		{
			GameObject corner = new GameObject(name, typeof(Image));
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
			_goldInput = CreateInputField(goldHolder.transform,
				SpritesHelper.RequestSpriteByName("Gold Plate Idle"), handler, "Gold Input", "0");
			Managers.Player.onGoldChanged.AddListener(() => _goldInput.InputField.text = PlayerStatsHelper.Gold.ToString());
		}

		private void SetupPopularityInput()
		{
			GameObject popularityHolder = CreateLayout("Popularity Holder");
			CreateIconOnLayout(popularityHolder.transform, "PopularityIcon Tier 5");

			_popularityInput = CreateInputField(popularityHolder.transform, SpritesHelper.RequestSpriteByName("Popularity Plate Idle"),
				(string value) => { if (int.TryParse(value, out int result)) PlayerStatsHelper.Popularity = result; }, "Popularity Input", "0");
			Managers.Player.popularity.onPopularityChanged
				.AddListener(() => _popularityInput.InputField.text = PlayerStatsHelper.Popularity.ToString());
		}

		private void SetupKarmaSlider()
		{
			GameObject karmaHolder = CreateLayout("Karma Holder");
			CreateIconOnLayout(karmaHolder.transform, "KarmaIcon Good 3");
			_karmaSlider = ControlsFactory.Instance.CreateSlider(karmaHolder.transform, "Karma Slider",
				(float value) => PlayerStatsHelper.Karma = (int)value, true, -100, 100);
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
				transform.anchorMax = new Vector2(0.9f, 0.9f);
				transform.anchorMin = new Vector2(0.1f, 0.1f);
				transform.sizeDelta = Vector2.zero;
			}
		}

		private ToggleContrainer CreateToggle(RectTransform parent, string name, Action<bool> handler, string label)
		{
			ToggleContrainer toggle = ControlsFactory.Instance.CreateToggle(parent, name, handler, label);
			toggle.GameObject.AddComponent<LayoutElement>().preferredWidth = preferedWidth;
			toggle.Text.sortingLayerID = SortingLayer.NameToID("Debug");
			toggle.Text.gameObject.AddComponent<LocalizedText>();
			_unsortedTextElements.Add(toggle.Text);
			return toggle;
		}

		private void SetupDevmodeToggle()
		{
			// can be force-set-always true;
			_devmodeToggle = CreateToggle(_panel.RectTransform, "DevMode Toggle",
				(bool value) => DebugManager.IsDeveloperMode = value || GlobalConfigs.IsForceDevMode, "#devmode_toggle_text");
			_devmodeToggle.Toggle.isOn = GlobalConfigs.IsForceDevMode;
		}

		private void SetupTeleportationToggle()
		{
			_teleportToggle = CreateToggle(_panel.RectTransform, "Teleport Toggle",
				(bool value) => GlobalConfigs.IsPositionModifyingAllowed = value, "#position_modifying_toggle_text");
		}

		private void SetupNodamageToggle()
		{
			_nodamageToggle = CreateToggle(_panel.RectTransform, "Godmode Toggle",
				(bool value) => GlobalConfigs.IsDangerZonesDisabled = value, "#disable_damage_toggle_text");
		}

		private void SetupDupingToggle()
		{
			_dupingToggle = CreateToggle(_panel.RectTransform, "Duping Toggle",
				(bool value) => GlobalConfigs.IsDuplicateOnClickAllowed = value, "#inventory_items_dupe_toggle_text");
		}

		private void SetupAutoHaggleToggle()
		{
			// that's just funny
			_autohaggleToggle = CreateToggle(_panel.RectTransform, "Autohaggle Toggle",
				(bool value) => GlobalConfigs.IsHaggleAutoplayAllowed = value, "#haggle_autoplay_toggle_text");
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
			potionEffectMenuOpenHandler.Invoke();
		}

	}

}
