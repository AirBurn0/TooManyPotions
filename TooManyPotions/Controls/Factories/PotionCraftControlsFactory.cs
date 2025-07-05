using EasierUI;
using EasierUI.Controls.Contrainers;
using TMPro;
using TooManyPotions.Patches;
using TooManyPotions.Scripts.Controls;
using UnityEngine;
using UnityEngine.UI;

namespace TooManyPotions.Controls.Factories
{
	public class PotionCraftControlsFactory : EasierUI.Controls.Factories.DefaultControlsFactory
	{
		private ControlsResources.Resources _resourcesDefault = new()
		{
			font = Helpers.FontsHelper.RequestFont("Vollkorn-PC SemiBold SDF")
		};
		private ControlsResources.Resources _resourcesInputField = new()
		{
			font = Helpers.FontsHelper.RequestFont("Vollkorn-PC-Numbers Bold SDF")
		};
		private ControlsResources.Resources _resourcesToggle = new()
		{
			standard = Helpers.SpritesHelper.GetByName("Alchemist'sPathBook FollowIcon Default"),
			checkmark = Helpers.SpritesHelper.GetByName("Alchemist'sPathBook FollowIcon AlwaysFollow")
		};
		private ControlsResources.Resources _resourcesScroll = new()
		{
			// no standard cuz looks cringy
			knob = Helpers.SpritesHelper.RequestSpriteByName("InventoryScroller Pointer"),
			background = Helpers.SpritesHelper.RequestSpriteByName("InventoryScroller Axis Var1 Active")
		};
		private ControlsResources.Resources _resourcesScrollView = new()
		{
			standard = Helpers.SpritesHelper.RequestSpriteByName("InventoryScroller Pointer"),
			knob = Helpers.SpritesHelper.RequestSpriteByName("InventoryScroller Pointer"),
			background = Helpers.SpritesHelper.RequestSpriteByName("InventoryWindow Background Var2"),
			childBackground = Helpers.SpritesHelper.RequestSpriteByName("InventoryScroller Axis Var1 Active")
		};

		public static new PotionCraftControlsFactory Instance
		{
			get;
		} = new PotionCraftControlsFactory();

		protected override ButtonContrainer CreateButton(ControlsResources.Resources resources = new ControlsResources.Resources())
		{
			ButtonContrainer button = base.CreateButton(_resourcesDefault);
			button.Text.enableWordWrapping = false;

			return button;
		}

		protected override InputFieldContrainer CreateInputField(ControlsResources.Resources resources = new ControlsResources.Resources())
		{
			InputFieldContrainer inputField = base.CreateInputField(_resourcesInputField);

			return inputField;
		}

		protected override SliderContrainer CreateSlider(ControlsResources.Resources resources = new ControlsResources.Resources())
		{
			SliderContrainer slider = base.CreateSlider(_resourcesScroll);
			GameObject GO = slider.GameObject;
			UnityEngine.Object.Destroy(GO.transform.Find("Fill Area").gameObject);

			RectTransform background = (RectTransform)slider.BackgroundImage.gameObject.transform;
			// 2lazy 2rotate mesh uv
			background.localEulerAngles = new(0f, 0f, 90f);
			background.anchorMin = new (0.46f, -2f);
			background.anchorMax = new(0.54f, 3f);

			RectTransform sliderArea = (RectTransform)GO.transform.Find("Handle Slide Area");
			sliderArea.sizeDelta = new(sliderArea.sizeDelta.x, -10);

            return slider;
		}

		protected override ToggleContrainer CreateToggle(ControlsResources.Resources resources = new ControlsResources.Resources())
		{
			ToggleContrainer toggle = base.CreateToggle(_resourcesToggle);
			toggle.GameObject.GetComponent<RectTransform>().sizeDelta = new(0, 22);
			
			TMP_Text text = toggle.Text;
			text.fontSize = 16;
			text.color = new(0f, 0f, 0f, 1f);
			text.enableWordWrapping = false;
			RectTransform transform = text.gameObject.GetComponent<RectTransform>();
			transform.anchoredPosition = new(120, -25);
			transform.anchorMax = transform.anchorMin = Vector2.up;
			
			return toggle;
		}

		protected override ScrollContainer CreateVerticalScroll(ControlsResources.Resources resources = new ControlsResources.Resources())
		{
			ScrollContainer scroll = base.CreateVerticalScroll(_resourcesScrollView);
			scroll.Background.color = new(1f, 1f, 1f, 1f);
			scroll.Scroll.movementType = ScrollRect.MovementType.Clamped;
			GameObject GO = scroll.GameObject;
			RectTransform viewTransform = (RectTransform)GO.transform;
			viewTransform.pivot = new(1f, 0f);

			RectTransform content = scroll.ContentHolder.GetComponent<RectTransform>();
			content.anchorMax = new(0.98f, 1f);
			content.anchorMin = new(0.02f, 0f);

			RectTransform scrollTransform = scroll.VerticalScroll.GetComponent<RectTransform>();
			scrollTransform.sizeDelta = new(12f, 0f);
			scrollTransform.pivot = new(1f, 0f);
			scrollTransform.anchorMax = new(0.975f, 1f);;
			scrollTransform.anchorMin = new(0.975f, 0f);
			scrollTransform.anchoredPosition = Vector2.zero;
			// scrollbar slider/knob/handle size hack
			ScrollRectPatch.SetSliderSize(GO, Vector2.zero);
			RectTransform slidingArea = (RectTransform)scrollTransform.Find("Sliding Area");
			slidingArea.sizeDelta = new(-10f, -10f);
			RectTransform handle = (RectTransform)slidingArea.Find("Handle");
			handle.sizeDelta = new(15f, 15f);

			// grid + cell auto-resizing
			ScrollRectCellResize resizer = GO.AddComponent<ScrollRectCellResize>();
			resizer.panel = viewTransform;
			resizer.scroll = scrollTransform;
			resizer.group = scroll.ContentHolder.AddComponent<GridLayoutGroup>();
			ContentSizeFitter fitter = scroll.ContentHolder.AddComponent<ContentSizeFitter>();
			fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

			return scroll;
		}



	}

}
