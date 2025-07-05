using UnityEngine;
using UnityEngine.UI;

namespace TooManyPotions.Scripts.Controls
{
	public class ScrollRectCellResize : MonoBehaviour
	{
		public RectTransform? scrollRectTransform;
		public RectTransform? scrollbarVTransform;
		public GridLayoutGroup? group;
		public int iconsInRow = 4;
		private float _gridCellSize;

		public void Update()
		{
			if (group == null || scrollRectTransform == null || scrollbarVTransform == null)
				return;
			float panelWidth = scrollRectTransform.rect.width;
			float barWidth = scrollbarVTransform.rect.width;
			float cellSize = (panelWidth - barWidth) / iconsInRow;
			if (cellSize < 0)
				cellSize = 0;
			if (cellSize == _gridCellSize)
				return;
			group.cellSize = new Vector2(cellSize, cellSize);
			_gridCellSize = cellSize;
		}

	}

}