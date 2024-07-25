using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using UnityEngine;

namespace EasierUI
{
	public static class SpritesHelper
	{
		public static Sprite GetFromBitmap(Bitmap image)
		{
			MemoryStream tempStream = new MemoryStream();
			image.Save(tempStream, ImageFormat.Png);
			byte[] bytes = tempStream.ToArray();
			Texture2D texture = new Texture2D(1, 1);
			ImageConversion.LoadImage(texture, bytes);

			return Sprite.Create(texture, new Rect(0, 0, image.Width, image.Height), new Vector2(0, 0));
		}
	}
}
