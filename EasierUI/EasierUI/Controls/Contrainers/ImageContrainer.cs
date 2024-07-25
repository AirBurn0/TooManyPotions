using UnityEngine;
using UnityEngine.UI;

namespace EasierUI.Controls.Contrainers
{
	public class ImageContrainer : ControlContainer
	{
		public readonly Image Image;

		public ImageContrainer(GameObject GO, Image image) : base(GO)
		{
			Image = image;
		}
	}
}
