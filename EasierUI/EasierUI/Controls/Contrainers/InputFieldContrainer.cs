using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace EasierUI.Controls.Contrainers
{
	public class InputFieldContrainer : ControlContainer
	{
		public readonly TMP_InputField InputField;
		public readonly TextMeshProUGUI Text;
		public readonly TextMeshProUGUI TextPlaceHolder;
		public readonly Image BackgroundImage;

		public InputFieldContrainer(GameObject GO, TMP_InputField inputField, TextMeshProUGUI text, TextMeshProUGUI textPlaceHolder, Image backgroundImage) : base(GO)
		{
			InputField = inputField;
			Text = text;
			TextPlaceHolder = textPlaceHolder;
			BackgroundImage = backgroundImage;
		}

		public void AddHandler(Action<string> handler)
		{
			InputField.onValueChanged.AddListener(new UnityAction<string>(handler));
		}
	}
}