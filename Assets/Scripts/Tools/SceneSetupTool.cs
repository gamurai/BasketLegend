using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem.UI;
using TMPro;
using BasketLegend.Core;
using BasketLegend.UI;
using BasketLegend.Managers;
using BasketLegend.Data;

namespace BasketLegend.Tools
{
    public class SceneSetupTool : MonoBehaviour
    {
        [Header("Scene Setup")]
        [SerializeField] private bool autoSetupOnStart = false;

        [Header("Canvas Settings")]
        [SerializeField] private CanvasScaler.ScaleMode scaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        [SerializeField] private Vector2 referenceResolution = new Vector2(1920, 1080);

        private void Start()
        {
            if (autoSetupOnStart)
            {
                SetupGameScene();
            }
        }

        [ContextMenu("Setup Game Scene")]
        public void SetupGameScene()
        {
            CreateMainCanvas();
            CreateEventSystem();
            CreateGameManagers();
            CreateUIPanels();
            
            Debug.Log("Game Scene setup completed!");
        }

        [ContextMenu("Setup Splash Scene")]
        public void SetupSplashScene()
        {
            CreateSplashCanvas();
            CreateEventSystem();
            CreateGameInitializer();
            
            Debug.Log("Splash Scene setup completed!");
        }

        private void CreateMainCanvas()
        {
            GameObject canvasGO = new GameObject("Main Canvas");
            Canvas canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 0;

            CanvasScaler scaler = canvasGO.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = scaleMode;
            scaler.referenceResolution = referenceResolution;
            scaler.matchWidthOrHeight = 0.5f;

            canvasGO.AddComponent<GraphicRaycaster>();

            Debug.Log("Main Canvas created successfully!");
        }

        private void CreateSplashCanvas()
        {
            GameObject canvasGO = new GameObject("Splash Canvas");
            Canvas canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 0;

            CanvasScaler scaler = canvasGO.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = scaleMode;
            scaler.referenceResolution = referenceResolution;
            scaler.matchWidthOrHeight = 0.5f;

            canvasGO.AddComponent<GraphicRaycaster>();

            CreateLoadingScreen(canvasGO.transform);

            Debug.Log("Splash Canvas created successfully!");
        }

        private void CreateLoadingScreen(Transform parent)
        {
            GameObject loadingPanel = new GameObject("Loading Panel");
            loadingPanel.transform.SetParent(parent);

            RectTransform rect = loadingPanel.AddComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.sizeDelta = Vector2.zero;
            rect.anchoredPosition = Vector2.zero;

            Image panelImage = loadingPanel.AddComponent<Image>();
            panelImage.color = new Color(0.1f, 0.1f, 0.1f, 1f);

            CreateProgressBar(loadingPanel.transform);
            CreateLoadingText(loadingPanel.transform);

            loadingPanel.AddComponent<LoadingScreen>();
        }

        private void CreateProgressBar(Transform parent)
        {
            GameObject progressBarGO = new GameObject("Progress Bar");
            progressBarGO.transform.SetParent(parent);

            RectTransform rect = progressBarGO.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.2f, 0.3f);
            rect.anchorMax = new Vector2(0.8f, 0.4f);
            rect.sizeDelta = Vector2.zero;
            rect.anchoredPosition = Vector2.zero;

            Slider slider = progressBarGO.AddComponent<Slider>();
            slider.transition = Selectable.Transition.None;

            GameObject background = new GameObject("Background");
            background.transform.SetParent(progressBarGO.transform);
            Image bgImage = background.AddComponent<Image>();
            bgImage.color = new Color(0.3f, 0.3f, 0.3f, 1f);

            RectTransform bgRect = background.GetComponent<RectTransform>();
            bgRect.anchorMin = Vector2.zero;
            bgRect.anchorMax = Vector2.one;
            bgRect.sizeDelta = Vector2.zero;
            bgRect.anchoredPosition = Vector2.zero;

            GameObject fillArea = new GameObject("Fill Area");
            fillArea.transform.SetParent(progressBarGO.transform);

            RectTransform fillAreaRect = fillArea.AddComponent<RectTransform>();
            fillAreaRect.anchorMin = Vector2.zero;
            fillAreaRect.anchorMax = Vector2.one;
            fillAreaRect.sizeDelta = Vector2.zero;
            fillAreaRect.anchoredPosition = Vector2.zero;

            GameObject fill = new GameObject("Fill");
            fill.transform.SetParent(fillArea.transform);
            Image fillImage = fill.AddComponent<Image>();
            fillImage.color = new Color(0.2f, 0.8f, 0.2f, 1f);

            RectTransform fillRect = fill.GetComponent<RectTransform>();
            fillRect.anchorMin = Vector2.zero;
            fillRect.anchorMax = Vector2.one;
            fillRect.sizeDelta = Vector2.zero;
            fillRect.anchoredPosition = Vector2.zero;

            slider.targetGraphic = fillImage;
            slider.fillRect = fillRect;
        }

        private void CreateLoadingText(Transform parent)
        {
            GameObject textGO = new GameObject("Loading Text");
            textGO.transform.SetParent(parent);

            RectTransform rect = textGO.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.2f, 0.5f);
            rect.anchorMax = new Vector2(0.8f, 0.6f);
            rect.sizeDelta = Vector2.zero;
            rect.anchoredPosition = Vector2.zero;

            TextMeshProUGUI text = textGO.AddComponent<TextMeshProUGUI>();
            text.text = "Loading...";
            text.fontSize = 24;
            text.color = Color.white;
            text.alignment = TextAlignmentOptions.Center;
        }

        private void CreateEventSystem()
        {
            if (FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
            {
                GameObject eventSystemGO = new GameObject("EventSystem");
                eventSystemGO.AddComponent<UnityEngine.EventSystems.EventSystem>();
                eventSystemGO.AddComponent<InputSystemUIInputModule>();

                Debug.Log("EventSystem created successfully with New Input System!");
            }
        }

        private void CreateGameManagers()
        {
            GameObject managersGO = new GameObject("Game Managers");

            GameObject gameController = new GameObject("Main Game Controller");
            gameController.transform.SetParent(managersGO.transform);
            gameController.AddComponent<MainGameController>();

            GameObject gameStateManager = new GameObject("Game State Manager");
            gameStateManager.transform.SetParent(managersGO.transform);
            gameStateManager.AddComponent<GameStateManager>();

            GameObject uiManager = new GameObject("UI Manager");
            uiManager.transform.SetParent(managersGO.transform);
            uiManager.AddComponent<UIManager>();

            GameObject playerDataManager = new GameObject("Player Data Manager");
            playerDataManager.transform.SetParent(managersGO.transform);
            playerDataManager.AddComponent<PlayerDataManager>();

            GameObject levelManager = new GameObject("Level Manager");
            levelManager.transform.SetParent(managersGO.transform);
            levelManager.AddComponent<LevelManager>();

            GameObject settingsManager = new GameObject("Settings Manager");
            settingsManager.transform.SetParent(managersGO.transform);
            settingsManager.AddComponent<SettingsManager>();

            GameObject audioManager = new GameObject("Audio Manager");
            audioManager.transform.SetParent(managersGO.transform);
            audioManager.AddComponent<AudioManager>();

            GameObject sceneController = new GameObject("Scene Controller");
            sceneController.transform.SetParent(managersGO.transform);
            sceneController.AddComponent<SceneController>();

            Debug.Log("Game Managers created successfully!");
        }

        private void CreateGameInitializer()
        {
            GameObject initializerGO = new GameObject("Game Initializer");
            GameInitializer initializer = initializerGO.AddComponent<GameInitializer>();

            LoadingScreen loadingScreen = FindObjectOfType<LoadingScreen>();
            if (loadingScreen != null)
            {
                var field = typeof(GameInitializer).GetField("loadingScreen", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                field?.SetValue(initializer, loadingScreen);
            }

            Debug.Log("Game Initializer created successfully!");
        }

        private void CreateUIPanels()
        {
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas == null) return;

            CreateMainMenuPanel(canvas.transform);
            CreateSettingsPanel(canvas.transform);
            CreateLevelSelectPanel(canvas.transform);

            Debug.Log("UI Panels created successfully!");
        }

        private void CreateMainMenuPanel(Transform parent)
        {
            GameObject panelGO = new GameObject("Main Menu Panel");
            panelGO.transform.SetParent(parent);

            RectTransform rect = panelGO.AddComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.sizeDelta = Vector2.zero;
            rect.anchoredPosition = Vector2.zero;

            Image panelImage = panelGO.AddComponent<Image>();
            panelImage.color = new Color(0.1f, 0.1f, 0.1f, 0.8f);

            panelGO.AddComponent<MainMenuPanel>();
            panelGO.SetActive(false);
        }

        private void CreateSettingsPanel(Transform parent)
        {
            GameObject panelGO = new GameObject("Settings Panel");
            panelGO.transform.SetParent(parent);

            RectTransform rect = panelGO.AddComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.sizeDelta = Vector2.zero;
            rect.anchoredPosition = Vector2.zero;

            Image panelImage = panelGO.AddComponent<Image>();
            panelImage.color = new Color(0.1f, 0.1f, 0.1f, 0.9f);

            panelGO.AddComponent<SettingsPanel>();
            panelGO.SetActive(false);
        }

        private void CreateLevelSelectPanel(Transform parent)
        {
            GameObject panelGO = new GameObject("Level Select Panel");
            panelGO.transform.SetParent(parent);

            RectTransform rect = panelGO.AddComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.sizeDelta = Vector2.zero;
            rect.anchoredPosition = Vector2.zero;

            Image panelImage = panelGO.AddComponent<Image>();
            panelImage.color = new Color(0.1f, 0.1f, 0.1f, 0.9f);

            panelGO.AddComponent<LevelSelectPanel>();
            panelGO.SetActive(false);
        }
    }
}