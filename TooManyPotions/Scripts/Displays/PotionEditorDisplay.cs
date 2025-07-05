using EasierUI.Controls.Contrainers;
using PotionCraft.DebugObjects.DebugWindows;
using PotionCraft.LocalizationSystem;
using PotionCraft.ManagersSystem;
using PotionCraft.ObjectBased.RecipeMap;
using PotionCraft.ScriptableObjects;
using TooManyPotions.Controls.Factories;
using TooManyPotions.Helpers;
using TooManyPotions.Scripts.Controls;
using UnityEngine;
using UnityEngine.UI;

namespace TooManyPotions.Displays
{
	internal class PotionEditorDisplay : AbstractDisplay
	{
		private const int baseScrollWidth = 60 + 16;
		private const int effectScrollWidth = 720 + 16;
		private const int width = effectScrollWidth + baseScrollWidth;
		private const int height = 120;
		private DebugWindow? _warningWindow = null;
		private Button? currentState = null;
		private ScrollContainer? _effectsScroll;

		public static PotionEditorDisplay Init()
		{
			return Init<PotionEditorDisplay>("#potion_editor_title", "Potion Editor");
		}

		protected override void SetupElements()
		{
			GameObject layout = CreateLayout(_panel.RectTransform, "Potion Editor Scrolls", width, height);
			SetupBaseScroll(layout.transform);
			SetupEffectsScroll(layout.transform);
			CreateToggle(_panel.RectTransform, "Overflow Toggle",
				(bool value) => GlobalConfigs.IsEffectOverflowAllowed = value, "#overflow_toggle_text"
			);
		}

		private ToggleContrainer CreateToggle(RectTransform parent, string name, System.Action<bool> handler, string label)
		{
			ToggleContrainer toggle = ControlsFactory.Instance.CreateToggle(parent, name, handler, label);
			toggle.GameObject.AddComponent<LayoutElement>().preferredWidth = width;
			toggle.Text.gameObject.AddComponent<LocalizedText>();
			return toggle;
		}

		public ScrollContainer CreateVerticalScroll(Transform parent, string objectName)
		{
			ScrollContainer scroll = ControlsFactory.Instance.CreateVerticalScroll(parent, objectName);
			Destroy(scroll.Background);

			return scroll;
		}

		private void SetupEffectsScroll(Transform parent)
		{
			_effectsScroll = CreateVerticalScroll(parent, "Effects Scroll Selector");
			_effectsScroll.GameObject.GetComponent<ScrollRectCellResize>().iconsInRow = 12;
			LayoutElement layoutElement = _effectsScroll.GameObject.AddComponent<LayoutElement>();
			layoutElement.preferredWidth = effectScrollWidth;
			int slot = 0;
			foreach (PotionEffect effect in PlayerStatsHelper.Effects)
			{
				ButtonContrainer newButton = ControlsFactory.Instance.CreateButton(
					_effectsScroll.ContentHolder.transform, effect.name,
					() =>
					{
						if (!GlobalConfigs.IsEffectOverflowAllowed && !PlayerStatsHelper.CanAddEffect(effect))
						{
							ShowWarning();
							return;
						}
						else
						{
							if (_warningWindow?.Visible ?? false)
								_warningWindow.Visible = false;
						}
						PlayerStatsHelper.AddEffect(effect);
					},
					SpritesHelper.RequestSpriteByName($"Inventory Item Slot {1 + (slot++ % 18)} Normal"),
					true
				);
				Destroy(newButton.Text.gameObject);
				newButton.Image.color = new Color(1f, 1f, 1f, 0.3f);
				ImageContrainer icon = CreateIconOnLayout(newButton.GameObject.transform, effect.name + " Icon", effect.icon.GetSprite());
				RectTransform transform = icon.GameObject.GetComponent<RectTransform>();
				transform.anchoredPosition = new(0f, 0f);
				transform.anchorMax = new(0.9f, 0.9f);
				transform.anchorMin = new(0.1f, 0.1f);
				transform.sizeDelta = Vector2.zero;
			}
		}

		private void SetupBaseScroll(Transform parent)
		{
			_effectsScroll = CreateVerticalScroll(parent, "Base Scroll Selector");
			_effectsScroll.GameObject.GetComponent<ScrollRectCellResize>().iconsInRow = 1;
			LayoutElement layoutElement = _effectsScroll.GameObject.AddComponent<LayoutElement>();
			layoutElement.preferredWidth = baseScrollWidth;
			layoutElement.flexibleWidth = 1;
			int slot = 0;
			foreach (MapState map in PlayerStatsHelper.MapBases)
			{
				ButtonContrainer newButton = ControlsFactory.Instance.CreateButton(
					_effectsScroll.ContentHolder.transform, map.potionBase.name, () => { },
					SpritesHelper.RequestSpriteByName($"Inventory Item Slot {1 + (slot++ % 18)} Normal"),
					true
				);
				Destroy(newButton.Text.gameObject);
				if (currentState == null)
				{
					currentState = newButton.Button;
					currentState.interactable = false;
				}
				// late onClick init
				newButton.AddHandler(() =>
				{
					PlayerStatsHelper.SetMapBase(map);
					NewState(newButton.Button);
				});
				Managers.RecipeMap.onPotionBaseSelect.AddListener(() =>
				{
					if (Managers.RecipeMap.currentMap == map)
						NewState(newButton.Button);
				});
				newButton.Image.color = new Color(1f, 1f, 1f, 0.3f);
				ImageContrainer icon = CreateIconOnLayout(newButton.GameObject.transform, map.potionBase.name + " Icon", map.potionBase.markerIconSelectedSprite);
				RectTransform transform = icon.GameObject.GetComponent<RectTransform>();
				transform.anchoredPosition = new(0f, 0f);
				transform.anchorMax = new(0.9f, 0.9f);
				transform.anchorMin = new(0.1f, 0.1f);
				transform.sizeDelta = Vector2.zero;
			}
		}

		private void NewState(Button n)
		{
			if (currentState != null)
				currentState.interactable = true;
			currentState = n;
			currentState.interactable = false;
		}

		private void ShowWarning()
		{
			if (_warningWindow == null)
			{
				_warningWindow = DebugWindow.Init("#warning_window_title", true);
				_warningWindow.ShowText("#warning_window_text");
				_warningWindow.captionText.gameObject.AddComponent<LocalizedText>();
				_warningWindow.bodyText.gameObject.AddComponent<LocalizedText>();
				_warningWindow.resizeAfter = 0;
				_warningWindow.gameObject.transform.localPosition = new(-4.5f, 1f, 0f);
			}
			_warningWindow.Visible = true;
		}

	}

}
