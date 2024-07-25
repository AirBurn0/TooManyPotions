using EasierUI.Controls.Contrainers;
using PotionCraft.DebugObjects.DebugWindows;
using PotionCraft.LocalizationSystem;
using PotionCraft.ManagersSystem;
using PotionCraft.ManagersSystem.Debug;
using PotionCraft.Markers;
using PotionCraft.Settings;
using System.Collections.Generic;
using TMPro;
using TooManyPotions.Controls.Factories;
using UnityEngine;
using UnityEngine.UI;

namespace TooManyPotions.Displays
{
	abstract internal class AbstractDisplay : MonoBehaviour
	{
		public const string UISortingLayerName = "Debug";
		protected float scaleFactor = 0.02f;
		protected int defaultPadding = 4;
		protected int headPadding = 26;
		protected int sortingOrderIncrement = 80;
		private SpriteRenderer _sortingOrderController;
		protected List<TextMeshPro> _unsortedTextElements = new List<TextMeshPro>();
		protected Canvas _canvas;
		protected PanelContainer _panel;
		private BoxCollider2D _backgroundCollider;
		public DebugWindow Window { get; private set; }
		private int SortingOrder => _sortingOrderController?.sortingOrder + sortingOrderIncrement ?? 0;

		private void Update()
		{
			if (Window == null)
				return;
			TryReorder();
			// on window resize		
			Vector2 arenaVector = scaleFactor * (_panel.GameObject.GetComponent<RectTransform>().sizeDelta + new Vector2(2 * defaultPadding + 1, headPadding + 2 * defaultPadding));
			Vector2 boxVector = new Vector2(arenaVector.x, 0.5f);
			Window.colliderBackground.size = boxVector;
			Window.colliderBackground.offset = boxVector / 2f * (Vector2.right + Vector2.down);
			Window.spriteBackground.size = arenaVector;
			Window.spriteScratches.size = arenaVector;
			_backgroundCollider.size = arenaVector;
			_backgroundCollider.offset = arenaVector / 2f * (Vector2.right + Vector2.down);
			Vector3 localPosition = Window.headTransform.localPosition;
			Window.headTransform.localPosition = new Vector3(arenaVector.x - 0.06f, localPosition.y, localPosition.z);
			DisplayUpdate();
		}

		private void Awake()
		{
			SetupCanvas();
			SetupPanel();
			SetupElements();
			TryReorder();
			gameObject.GetComponent<RectTransform>().localScale = new Vector3(scaleFactor, scaleFactor, 1f);
		}

		protected void TryReorder()
		{
			int order = SortingOrder;
			if (_canvas.sortingOrder == order)
				return;
			_canvas.sortingOrder = order;
			foreach (var text in _unsortedTextElements)
				text.sortingOrder = _canvas.sortingOrder + 1;

		}

		private void SetupCanvas()
		{
			_canvas = gameObject.AddComponent<Canvas>();
			_canvas.renderMode = RenderMode.WorldSpace;
			_canvas.worldCamera = Managers.Game.Cam;
			_canvas.sortingLayerName = UISortingLayerName;
			_sortingOrderController = gameObject.transform.parent.gameObject.GetComponent<SpriteRenderer>();

			gameObject.AddComponent<GraphicRaycaster>();
			// ignore canvas size
			RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
			rectTransform.sizeDelta = new Vector2(0f, 0f);
			rectTransform.localPosition = new Vector3(0f, 0f, 0f);
		}

		private void SetupPanel()
		{
			_panel = ControlsFactory.Instance.CreatePanel(transform);

			RectTransform rectTransform = _panel.RectTransform;
			rectTransform.pivot = new Vector2(0f, 1f);
			rectTransform.localPosition = new Vector3(defaultPadding, -defaultPadding - headPadding, 0f);
			VerticalLayoutGroup layoutGroup = _panel.GameObject.AddComponent<VerticalLayoutGroup>();
			layoutGroup.padding = new RectOffset(defaultPadding, defaultPadding, defaultPadding, defaultPadding);
			layoutGroup.childControlHeight = false;
			layoutGroup.spacing = 4f;

			ContentSizeFitter fitter = _panel.GameObject.AddComponent<ContentSizeFitter>();
			fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
			Destroy(_panel.GameObject.GetComponent<Image>()); // no thanks
		}

		protected abstract void SetupElements();

		protected static GameObject CreateLayout(Transform parent, string name, float width, float height)
		{
			GameObject layout = new GameObject(name);
			layout.transform.SetParent(parent);

			LayoutElement layoutElementHolder = layout.AddComponent<LayoutElement>();
			layoutElementHolder.preferredWidth = width;
			layoutElementHolder.preferredHeight = height;

			HorizontalLayoutGroup layoutGroup = layout.AddComponent<HorizontalLayoutGroup>();
			layoutGroup.childForceExpandWidth = false;
			layoutGroup.spacing = 4f;

			ContentSizeFitter fitter = layout.AddComponent<ContentSizeFitter>();
			fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
			return layout;
		}

		protected static ImageContrainer CreateIconOnLayout(Transform parent, string objectName, Sprite sprite, int width = 32, int height = 32)
		{
			ImageContrainer icon = ControlsFactory.Instance.CreateImage(parent, objectName, sprite);

			LayoutElement layoutElement = icon.GameObject.AddComponent<LayoutElement>();
			layoutElement.preferredHeight = height;
			layoutElement.preferredWidth = width;
			return icon;
		}

		protected void DisplayUpdate()
		{

		}

		protected static T Init<T>(string title, string objectName) where T : AbstractDisplay
		{
			DebugWindow window = Instantiate(Settings<DebugManagerSettings>.Asset.debugWindowPrefab).GetComponent<DebugWindow>();
			window.captionText.text = title;
			window.captionText.gameObject.AddComponent<LocalizedText>();
			window.Visible = true;
			window.resizeAfter = float.MaxValue;

			GameObject windowObject = window.gameObject;
			windowObject.name = objectName + " GUI";
			windowObject.SetActive(true);
			windowObject.transform.SetParent(Managers.Game.Cam.transform);
			windowObject.transform.localPosition = new Vector3(-10f, 6f, 0f);

			GameObject menuObject = new GameObject(objectName + " Menu Display");
			Transform Root = windowObject?.transform.Find("Maximized/Background");
			menuObject.transform.SetParent(Root);
			T menu = menuObject.AddComponent<T>();
			menu.Window = window;

			GameObject background = new GameObject("BackgroundRaycastCollider");
			menu._backgroundCollider = background.AddComponent<BoxCollider2D>();
			background.AddComponent<RightPanelColliderForItemsRaycasting>();
			background.transform.SetParent(Root);
			background.layer = Root.gameObject.layer;
			background.transform.localPosition = Vector3.zero;

			return menu;
		}

	}

}
