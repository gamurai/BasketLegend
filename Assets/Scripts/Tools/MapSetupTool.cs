using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BasketLegend.UI;

namespace BasketLegend.Tools
{
    public class MapSetupTool : MonoBehaviour
    {
        [Header("Map Setup")]
        [SerializeField] private bool autoSetupOnStart = false;

        [Header("Canvas Settings")]
        [SerializeField] private CanvasScaler.ScaleMode scaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        [SerializeField] private Vector2 referenceResolution = new Vector2(1920, 1080);

        private void Start()
        {
            if (autoSetupOnStart)
            {
                SetupMapLevelSelectPanel();
            }
        }

        [ContextMenu("Setup Map Level Select Panel")]
        public void SetupMapLevelSelectPanel()
        {
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas == null)
            {
                Debug.LogError("No Canvas found! Please create a Canvas first.");
                return;
            }

            CreateMapLevelSelectPanel(canvas.transform);
            CreateMapPrefabs();
            
            Debug.Log("Map Level Select Panel setup completed!");
        }

        private void CreateMapLevelSelectPanel(Transform parent)
        {
            // Main panel
            GameObject panelGO = new GameObject("Map Level Select Panel");
            panelGO.transform.SetParent(parent);

            RectTransform rect = panelGO.AddComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.sizeDelta = Vector2.zero;
            rect.anchoredPosition = Vector2.zero;

            Image panelImage = panelGO.AddComponent<Image>();
            panelImage.color = new Color(0.1f, 0.1f, 0.2f, 1f);

            // Create scroll view
            GameObject scrollViewGO = CreateScrollView(panelGO.transform);
            
            // Add MapLevelSelectPanel component
            MapLevelSelectPanel mapPanel = panelGO.AddComponent<MapLevelSelectPanel>();
            
            // Assign references via reflection
            var scrollRectField = typeof(MapLevelSelectPanel).GetField("mapScrollView", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            scrollRectField?.SetValue(mapPanel, scrollViewGO.GetComponent<ScrollRect>());

            var contentField = typeof(MapLevelSelectPanel).GetField("mapContent", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            contentField?.SetValue(mapPanel, scrollViewGO.transform.Find("Viewport/Content").GetComponent<RectTransform>());

            // Create back button
            CreateBackButton(panelGO.transform);

            panelGO.SetActive(false);
        }

        private GameObject CreateScrollView(Transform parent)
        {
            // Scroll View
            GameObject scrollViewGO = new GameObject("Scroll View");
            scrollViewGO.transform.SetParent(parent);

            RectTransform scrollRect = scrollViewGO.AddComponent<RectTransform>();
            scrollRect.anchorMin = new Vector2(0.1f, 0.1f);
            scrollRect.anchorMax = new Vector2(0.9f, 0.85f);
            scrollRect.sizeDelta = Vector2.zero;
            scrollRect.anchoredPosition = Vector2.zero;

            ScrollRect scrollRectComponent = scrollViewGO.AddComponent<ScrollRect>();
            scrollRectComponent.horizontal = false;
            scrollRectComponent.vertical = true;
            scrollRectComponent.movementType = ScrollRect.MovementType.Elastic;

            // Viewport
            GameObject viewportGO = new GameObject("Viewport");
            viewportGO.transform.SetParent(scrollViewGO.transform);

            RectTransform viewportRect = viewportGO.AddComponent<RectTransform>();
            viewportRect.anchorMin = Vector2.zero;
            viewportRect.anchorMax = Vector2.one;
            viewportRect.sizeDelta = Vector2.zero;
            viewportRect.anchoredPosition = Vector2.zero;

            Image viewportImage = viewportGO.AddComponent<Image>();
            viewportImage.color = Color.clear;

            Mask viewportMask = viewportGO.AddComponent<Mask>();
            viewportMask.showMaskGraphic = false;

            // Content
            GameObject contentGO = new GameObject("Content");
            contentGO.transform.SetParent(viewportGO.transform);

            RectTransform contentRect = contentGO.AddComponent<RectTransform>();
            contentRect.anchorMin = new Vector2(0.5f, 1f);
            contentRect.anchorMax = new Vector2(0.5f, 1f);
            contentRect.sizeDelta = new Vector2(800f, 2000f);
            contentRect.anchoredPosition = new Vector2(0, -1000f);

            ContentSizeFitter sizeFitter = contentGO.AddComponent<ContentSizeFitter>();
            sizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            // Connect ScrollRect
            scrollRectComponent.viewport = viewportRect;
            scrollRectComponent.content = contentRect;

            // Scrollbar (optional)
            CreateScrollbar(scrollViewGO.transform, scrollRectComponent);

            return scrollViewGO;
        }

        private void CreateScrollbar(Transform parent, ScrollRect scrollRect)
        {
            GameObject scrollbarGO = new GameObject("Scrollbar Vertical");
            scrollbarGO.transform.SetParent(parent);

            RectTransform scrollbarRect = scrollbarGO.AddComponent<RectTransform>();
            scrollbarRect.anchorMin = new Vector2(1f, 0f);
            scrollbarRect.anchorMax = new Vector2(1f, 1f);
            scrollbarRect.sizeDelta = new Vector2(20f, 0f);
            scrollbarRect.anchoredPosition = new Vector2(10f, 0f);

            Image scrollbarImage = scrollbarGO.AddComponent<Image>();
            scrollbarImage.color = new Color(0.3f, 0.3f, 0.3f, 0.8f);

            Scrollbar scrollbar = scrollbarGO.AddComponent<Scrollbar>();
            scrollbar.direction = Scrollbar.Direction.BottomToTop;

            // Scrollbar Handle
            GameObject handleAreaGO = new GameObject("Sliding Area");
            handleAreaGO.transform.SetParent(scrollbarGO.transform);

            RectTransform handleAreaRect = handleAreaGO.AddComponent<RectTransform>();
            handleAreaRect.anchorMin = Vector2.zero;
            handleAreaRect.anchorMax = Vector2.one;
            handleAreaRect.sizeDelta = new Vector2(-20f, -20f);
            handleAreaRect.anchoredPosition = Vector2.zero;

            GameObject handleGO = new GameObject("Handle");
            handleGO.transform.SetParent(handleAreaGO.transform);

            RectTransform handleRect = handleGO.AddComponent<RectTransform>();
            handleRect.anchorMin = Vector2.zero;
            handleRect.anchorMax = Vector2.one;
            handleRect.sizeDelta = Vector2.zero;
            handleRect.anchoredPosition = Vector2.zero;

            Image handleImage = handleGO.AddComponent<Image>();
            handleImage.color = new Color(0.6f, 0.6f, 0.6f, 1f);

            scrollbar.handleRect = handleRect;
            scrollbar.targetGraphic = handleImage;

            scrollRect.verticalScrollbar = scrollbar;
        }

        private void CreateBackButton(Transform parent)
        {
            GameObject buttonGO = new GameObject("Back Button");
            buttonGO.transform.SetParent(parent);

            RectTransform buttonRect = buttonGO.AddComponent<RectTransform>();
            buttonRect.anchorMin = new Vector2(0.05f, 0.9f);
            buttonRect.anchorMax = new Vector2(0.25f, 0.98f);
            buttonRect.sizeDelta = Vector2.zero;
            buttonRect.anchoredPosition = Vector2.zero;

            Image buttonImage = buttonGO.AddComponent<Image>();
            buttonImage.color = new Color(0.8f, 0.3f, 0.3f, 1f);

            Button button = buttonGO.AddComponent<Button>();

            // Button Text
            GameObject textGO = new GameObject("Text");
            textGO.transform.SetParent(buttonGO.transform);

            RectTransform textRect = textGO.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = Vector2.zero;
            textRect.anchoredPosition = Vector2.zero;

            TextMeshProUGUI text = textGO.AddComponent<TextMeshProUGUI>();
            text.text = "BACK";
            text.fontSize = 24;
            text.color = Color.white;
            text.alignment = TextAlignmentOptions.Center;

            button.targetGraphic = buttonImage;
        }

        private void CreateMapPrefabs()
        {
            // These would typically be created as prefabs in the project
            // For now, we'll create placeholder GameObjects that can be converted to prefabs
            
            CreateMapLevelButtonPrefab();
            CreateSeasonHeaderPrefab();
            CreatePathRendererPrefab();
            
            Debug.Log("Map prefabs created! Convert the created GameObjects to prefabs and assign them to MapLevelSelectPanel.");
        }

        private void CreateMapLevelButtonPrefab()
        {
            GameObject buttonGO = new GameObject("Map Level Button [CONVERT TO PREFAB]");
            
            RectTransform rect = buttonGO.AddComponent<RectTransform>();
            rect.sizeDelta = new Vector2(100f, 100f);

            Image buttonImage = buttonGO.AddComponent<Image>();
            buttonImage.color = Color.white;

            Button button = buttonGO.AddComponent<Button>();

            // Level number text
            GameObject textGO = new GameObject("Level Number");
            textGO.transform.SetParent(buttonGO.transform);

            RectTransform textRect = textGO.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = Vector2.zero;
            textRect.anchoredPosition = Vector2.zero;

            TextMeshProUGUI text = textGO.AddComponent<TextMeshProUGUI>();
            text.text = "1";
            text.fontSize = 32;
            text.color = Color.black;
            text.alignment = TextAlignmentOptions.Center;

            // Stars container
            GameObject starsGO = new GameObject("Stars");
            starsGO.transform.SetParent(buttonGO.transform);

            RectTransform starsRect = starsGO.AddComponent<RectTransform>();
            starsRect.anchorMin = new Vector2(0f, 0f);
            starsRect.anchorMax = new Vector2(1f, 0.3f);
            starsRect.sizeDelta = Vector2.zero;
            starsRect.anchoredPosition = Vector2.zero;

            // Add MapLevelButton component
            MapLevelButton mapLevelButton = buttonGO.AddComponent<MapLevelButton>();

            Debug.Log("Map Level Button prefab template created!");
        }

        private void CreateSeasonHeaderPrefab()
        {
            GameObject headerGO = new GameObject("Season Header [CONVERT TO PREFAB]");
            
            RectTransform rect = headerGO.AddComponent<RectTransform>();
            rect.sizeDelta = new Vector2(800f, 200f);

            Image backgroundImage = headerGO.AddComponent<Image>();
            backgroundImage.color = new Color(0.2f, 0.4f, 0.8f, 0.8f);

            // Season name
            GameObject nameGO = new GameObject("Season Name");
            nameGO.transform.SetParent(headerGO.transform);

            RectTransform nameRect = nameGO.AddComponent<RectTransform>();
            nameRect.anchorMin = new Vector2(0.1f, 0.6f);
            nameRect.anchorMax = new Vector2(0.9f, 0.9f);
            nameRect.sizeDelta = Vector2.zero;
            nameRect.anchoredPosition = Vector2.zero;

            TextMeshProUGUI nameText = nameGO.AddComponent<TextMeshProUGUI>();
            nameText.text = "Season 1";
            nameText.fontSize = 36;
            nameText.color = Color.white;
            nameText.alignment = TextAlignmentOptions.Center;

            // Progress bar
            GameObject progressGO = new GameObject("Progress Bar");
            progressGO.transform.SetParent(headerGO.transform);

            RectTransform progressRect = progressGO.AddComponent<RectTransform>();
            progressRect.anchorMin = new Vector2(0.2f, 0.2f);
            progressRect.anchorMax = new Vector2(0.8f, 0.4f);
            progressRect.sizeDelta = Vector2.zero;
            progressRect.anchoredPosition = Vector2.zero;

            Slider progressSlider = progressGO.AddComponent<Slider>();

            // Add SeasonHeader component
            SeasonHeader seasonHeader = headerGO.AddComponent<SeasonHeader>();

            Debug.Log("Season Header prefab template created!");
        }

        private void CreatePathRendererPrefab()
        {
            GameObject pathGO = new GameObject("Path Renderer [CONVERT TO PREFAB]");
            
            LineRenderer lineRenderer = pathGO.AddComponent<LineRenderer>();
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            if (lineRenderer.material != null)
            {
                lineRenderer.material.color = Color.white;
            }
            lineRenderer.startWidth = 10f;
            lineRenderer.endWidth = 10f;
            lineRenderer.positionCount = 2;

            // Add PathRenderer component
            PathRenderer pathRenderer = pathGO.AddComponent<PathRenderer>();

            Debug.Log("Path Renderer prefab template created!");
        }
    }
}