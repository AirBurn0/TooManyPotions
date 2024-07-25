using UnityEngine;

namespace EasierUI.Controls.Contrainers
{
	public abstract class ControlContainer
	{
		public readonly GameObject GameObject;
		public ControlContainer(GameObject GO)
		{
			GameObject = GO;
		}

		public void SetParent(Transform parentTransform)
		{
			GameObject.transform.SetParent(parentTransform, false);
		}

		public void SetName(string name)
		{
			GameObject.name = name;
		}
	}
}
