using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace EasierUI.Controls.Contrainers
{
	public class ToggleContrainer : ControlContainer
	{
		public readonly Toggle Toggle;
		public readonly Image CheckmarkImage;
		public readonly Image BackgroundImage;
		public readonly TextMeshPro Text;
		public ToggleContrainer(GameObject GO, Toggle toggle, Image backgroundImage, Image checkmarkImage, TextMeshPro text) : base(GO)
		{
			Toggle = toggle;
			BackgroundImage = backgroundImage;
			CheckmarkImage = checkmarkImage;
			Text = text;
		}

		public void AddHandler(Action<bool> action)
		{
			Toggle.onValueChanged.AddListener(new UnityAction<bool>(action));
		}

	}
}