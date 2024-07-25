using EasierUI.Controls.Contrainers;
using System;
using UnityEngine;
using static TMPro.TMP_InputField;

namespace EasierUI.Controls
{
	public abstract class ControlsFactory
	{
		protected abstract PanelContainer CreatePanel(ControlsResources.Resources resources = new ControlsResources.Resources());
		protected abstract ButtonContrainer CreateButton(ControlsResources.Resources resources = new ControlsResources.Resources());
		protected abstract InputFieldContrainer CreateInputField(ControlsResources.Resources resources = new ControlsResources.Resources());
		protected abstract ToggleContrainer CreateToggle(ControlsResources.Resources resources = new ControlsResources.Resources());
		protected abstract SliderContrainer CreateSlider(ControlsResources.Resources resources = new ControlsResources.Resources());
		protected abstract ScrollContainer CreateVerticalScroll(ControlsResources.Resources resources = new ControlsResources.Resources());
		protected abstract ImageContrainer CreateImage(ControlsResources.Resources resources = new ControlsResources.Resources());

		public PanelContainer CreatePanel(Transform parent, string name = null)
		{
			PanelContainer panel = CreatePanel();
			panel.SetParent(parent);

			if (name != null)
				panel.SetName(name);

			return panel;
		}

		public ButtonContrainer CreateButton(Transform parent, string name = null, Action handler = null, Sprite sprite = null, bool? fillCenter = null, string text = null)
		{
			ButtonContrainer button = CreateButton();
			button.SetParent(parent);

			if (name != null)
				button.SetName(name);
			if (handler != null)
				button.AddHandler(handler);
			if (sprite != null)
				button.Image.sprite = sprite;
			if (fillCenter != null)
				button.Image.fillCenter = (bool)fillCenter;
			if (text != null)
				button.Text.text = text;

			return button;
		}

		public InputFieldContrainer CreateInputField(Transform parent, string name = null, Action<string> handler = null, ContentType? type = null, string placeholderText = null)
		{
			InputFieldContrainer field = CreateInputField();
			field.SetParent(parent);

			if (name != null)
				field.SetName(name);
			if (handler != null)
				field.AddHandler(handler);
			if (type != null)
				field.InputField.contentType = (ContentType)type;
			if (placeholderText != null)
				field.TextPlaceHolder.text = placeholderText;

			return field;
		}

		public ToggleContrainer CreateToggle(Transform parent, string name = null, Action<bool> handler = null, string text = null)
		{
			ToggleContrainer toggle = CreateToggle();
			toggle.SetParent(parent);

			if (name != null)
				toggle.SetName(name);
			if (handler != null)
				toggle.AddHandler(handler);
			if (text != null)
				toggle.Text.text = text;

			return toggle;
		}

		public SliderContrainer CreateSlider(Transform parent, string name = null, Action<float> handler = null, bool? wholeNumbers = null, float? minValue = null, float? maxValue = null)
		{
			SliderContrainer slider = CreateSlider();
			slider.SetParent(parent);

			if (name != null)
				slider.SetName(name);
			if (handler != null)
				slider.AddHandler(handler);
			if (wholeNumbers != null)
				slider.Slider.wholeNumbers = (bool)wholeNumbers;
			if (minValue != null)
				slider.Slider.minValue = (float)minValue;
			if (maxValue != null)
				slider.Slider.maxValue = (float)maxValue;

			return slider;
		}

		public ScrollContainer CreateVerticalScroll(Transform parent, string name = null)
		{
			ScrollContainer scroll = CreateVerticalScroll();
			scroll.SetParent(parent);

			if (name != null)
				scroll.SetName(name);

			return scroll;
		}

		public ImageContrainer CreateImage(Transform parent, string name = null, Sprite sprite = null)
		{
			ImageContrainer image = CreateImage();
			image.SetParent(parent);

			if (name != null)
				image.SetName(name);
			if (sprite != null)
				image.Image.sprite = sprite;

			return image;
		}
	}

}
