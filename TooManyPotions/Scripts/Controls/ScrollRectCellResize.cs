using UnityEngine;
using UnityEngine.UI;

namespace TooManyPotions.Scripts.Controls
{
	public class ScrollRectCellResize : MonoBehaviour
	{
		public RectTransform? panel, content, scroll;
		public GridLayoutGroup? group;
		public int iconsInRow = 4;
		private float _gridCellSize;

		public void Update()
		{
			if (group == null || panel == null || scroll == null)
				return;
			if (content == null)
				content = (RectTransform?)panel.Find("Viewport")?.Find("Content");
			float panelWidth = panel.rect.width * (content?.anchorMax.x - content?.anchorMin.x ?? 1);
			float barWidth = scroll.rect.width;
			float cellSize = (panelWidth - barWidth) / iconsInRow;
			if (cellSize < 0)
				cellSize = 0;
			if (cellSize == _gridCellSize)
				return;
			group.cellSize = new(cellSize, cellSize);
			_gridCellSize = cellSize;
		}

	}

}