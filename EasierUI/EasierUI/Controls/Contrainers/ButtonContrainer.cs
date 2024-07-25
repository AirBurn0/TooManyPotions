using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace EasierUI.Controls.Contrainers
{
	public class ButtonContrainer : ControlContainer
	{
		public readonly Button Button;
		public readonly Image Image;
		public readonly TextMeshProUGUI Text;
		public ButtonContrainer(GameObject GO, Button button, Image image, TextMeshProUGUI text) : base(GO)
		{
			Button = button;
			Image = image;
			Text = text;
		}


		public void AddHandler(Action listener)
		{
			Button.onClick.AddListener(new UnityAction(listener));
		}
	}
}
