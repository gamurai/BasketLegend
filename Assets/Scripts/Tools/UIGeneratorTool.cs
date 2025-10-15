using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BasketLegend.UI;
using BasketLegend.Core;

namespace BasketLegend.Tools
{
    public class UIGeneratorTool : MonoBehaviour
    {
        [Header("UI Generation Settings")]
        [SerializeField] private bool useGradients = true;
        [SerializeField] private Color primaryColor = new Color(0.2f, 0.4f, 0.8f, 1f);
        [SerializeField] private Color secondaryColor = new Color(0.8f, 0.5f, 0.2f, 1f);
        [SerializeField] private Color backgroundColor = new Color(0.1f, 0.1f, 0.15f, 0.95f);

        [Header("Font Settings")]
        [SerializeField] private TMP_FontAsset customFont;
        [SerializeField] private float titleFontSize = 72f;
        [SerializeField] private float buttonFontSize = 36f;
        [SerializeField] private float labelFontSize = 24f;

        [ContextMenu("Generate Main Menu UI")]
        public void GenerateMainMenuUI()
        {
            GameObject mainMenuPanel = GameObject.Find("Main Menu Panel");
            
            if (mainMenuPanel == null)
            {
                Debug.LogError("Main Menu Panel not found! Please create it first.");
                return;
            }

            ClearChildren(mainMenuPanel.transform);
            BuildMainMenuUI(mainMenuPanel.transform);
            
            Debug.Log("Main Menu UI generated successfully!");
        }

        [ContextMenu("Generate Settings Panel UI")]
        public void GenerateSettingsPanelUI()
        {
            GameObject settingsPanel = GameObject.Find("Settings Panel");
            
            if (settingsPanel == null)
            {
                Debug.LogError("Settings Panel not found! Please create it first.");
                return;
            }

            ClearChildren(settingsPanel.transform);
            BuildSettingsUI(settingsPanel.transform);
            
            Debug.Log("Settings Panel UI generated successfully!");
        }

        [ContextMenu("Generate Both Main Menu and Settings UI")]
        public void GenerateBothUIs()
        {
            GenerateMainMenuUI();
            GenerateSettingsPanelUI();
        }

        private void BuildMainMenuUI(Transform parent)
        {
            // Background decoration
            CreateBackgroundDecoration(parent);

            // Top section - Player Info
            CreatePlayerInfoSection(parent);

            // Middle section - Title
            CreateGameTitle(parent);

            // Bottom section - Buttons
            CreateMainMenuButtons(parent);

            // Version text
            CreateVersionText(parent);
        }

        private void CreateBackgroundDecoration(Transform parent)
        {
            GameObject bgDecor = new GameObject("Background Decoration");
            bgDecor.transform.SetParent(parent, false);

            RectTransform rect = bgDecor.AddComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.sizeDelta = Vector2.zero;

            Image image = bgDecor.AddComponent<Image>();
            image.color = new Color(1f, 1f, 1f, 0.05f);
            image.raycastTarget = false;
        }

        private void CreatePlayerInfoSection(Transform parent)
        {
            GameObject playerInfoBG = new GameObject("Player Info Container");
            playerInfoBG.transform.SetParent(parent, false);

            RectTransform bgRect = playerInfoBG.AddComponent<RectTransform>();
            bgRect.anchorMin = new Vector2(0.05f, 0.85f);
            bgRect.anchorMax = new Vector2(0.95f, 0.98f);
            bgRect.sizeDelta = Vector2.zero;

            Image bgImage = playerInfoBG.AddComponent<Image>();
            bgImage.color = new Color(0.15f, 0.15f, 0.2f, 0.8f);

            HorizontalLayoutGroup layout = playerInfoBG.AddComponent<HorizontalLayoutGroup>();
            layout.padding = new RectOffset(20, 20, 10, 10);
            layout.spacing = 15f;
            layout.childAlignment = TextAnchor.MiddleLeft;
            layout.childControlWidth = false;
            layout.childControlHeight = true;

            // Player Avatar (placeholder)
            CreatePlayerAvatar(playerInfoBG.transform);

            // Player Name
            CreatePlayerNameText(playerInfoBG.transform);

            // Player Level
            CreatePlayerLevelText(playerInfoBG.transform);

            // Experience Bar
            CreateExperienceBar(playerInfoBG.transform);
        }

        private void CreatePlayerAvatar(Transform parent)
        {
            GameObject avatar = new GameObject("Player Avatar");
            avatar.transform.SetParent(parent, false);

            RectTransform rect = avatar.AddComponent<RectTransform>();
            rect.sizeDelta = new Vector2(80f, 80f);

            Image image = avatar.AddComponent<Image>();
            image.color = primaryColor;

            LayoutElement layoutElement = avatar.AddComponent<LayoutElement>();
            layoutElement.minWidth = 80f;
            layoutElement.minHeight = 80f;
        }

        private void CreatePlayerNameText(Transform parent)
        {
            GameObject nameObj = new GameObject("Player Name");
            nameObj.transform.SetParent(parent, false);

            RectTransform rect = nameObj.AddComponent<RectTransform>();
            rect.sizeDelta = new Vector2(200f, 40f);

            TextMeshProUGUI text = nameObj.AddComponent<TextMeshProUGUI>();
            text.text = "Player Name";
            text.fontSize = 28f;
            text.color = Color.white;
            text.font = customFont;
            text.alignment = TextAlignmentOptions.MidlineLeft;

            LayoutElement layoutElement = nameObj.AddComponent<LayoutElement>();
            layoutElement.minWidth = 150f;
        }

        private void CreatePlayerLevelText(Transform parent)
        {
            GameObject levelObj = new GameObject("Player Level");
            levelObj.transform.SetParent(parent, false);

            RectTransform rect = levelObj.AddComponent<RectTransform>();
            rect.sizeDelta = new Vector2(120f, 40f);

            TextMeshProUGUI text = levelObj.AddComponent<TextMeshProUGUI>();
            text.text = "Level 1";
            text.fontSize = 24f;
            text.color = secondaryColor;
            text.font = customFont;
            text.alignment = TextAlignmentOptions.MidlineLeft;

            LayoutElement layoutElement = levelObj.AddComponent<LayoutElement>();
            layoutElement.minWidth = 100f;
        }

        private void CreateExperienceBar(Transform parent)
        {
            GameObject expBarBG = new GameObject("Experience Bar");
            expBarBG.transform.SetParent(parent, false);

            RectTransform bgRect = expBarBG.AddComponent<RectTransform>();
            bgRect.sizeDelta = new Vector2(300f, 30f);

            Image bgImage = expBarBG.AddComponent<Image>();
            bgImage.color = new Color(0.2f, 0.2f, 0.25f, 1f);

            LayoutElement layoutElement = expBarBG.AddComponent<LayoutElement>();
            layoutElement.flexibleWidth = 1f;
            layoutElement.minHeight = 30f;

            // Fill
            GameObject fill = new GameObject("Fill");
            fill.transform.SetParent(expBarBG.transform, false);

            RectTransform fillRect = fill.AddComponent<RectTransform>();
            fillRect.anchorMin = Vector2.zero;
            fillRect.anchorMax = new Vector2(0.5f, 1f);
            fillRect.sizeDelta = Vector2.zero;

            Image fillImage = fill.AddComponent<Image>();
            fillImage.color = primaryColor;

            // XP Text
            GameObject xpText = new GameObject("XP Text");
            xpText.transform.SetParent(expBarBG.transform, false);

            RectTransform xpRect = xpText.AddComponent<RectTransform>();
            xpRect.anchorMin = Vector2.zero;
            xpRect.anchorMax = Vector2.one;
            xpRect.sizeDelta = Vector2.zero;

            TextMeshProUGUI xpTMP = xpText.AddComponent<TextMeshProUGUI>();
            xpTMP.text = "500 / 1000 XP";
            xpTMP.fontSize = 18f;
            xpTMP.color = Color.white;
            xpTMP.font = customFont;
            xpTMP.alignment = TextAlignmentOptions.Center;
        }

        private void CreateGameTitle(Transform parent)
        {
            GameObject titleObj = new GameObject("Game Title");
            titleObj.transform.SetParent(parent, false);

            RectTransform rect = titleObj.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.1f, 0.55f);
            rect.anchorMax = new Vector2(0.9f, 0.75f);
            rect.sizeDelta = Vector2.zero;

            TextMeshProUGUI title = titleObj.AddComponent<TextMeshProUGUI>();
            title.text = "BASKET LEGEND";
            title.fontSize = titleFontSize;
            title.color = Color.white;
            title.font = customFont;
            title.alignment = TextAlignmentOptions.Center;
            title.fontStyle = FontStyles.Bold;

            // Add outline for better readability
            title.outlineWidth = 0.2f;
            title.outlineColor = new Color(0f, 0f, 0f, 0.5f);
        }

        private void CreateMainMenuButtons(Transform parent)
        {
            GameObject buttonContainer = new GameObject("Button Container");
            buttonContainer.transform.SetParent(parent, false);

            RectTransform containerRect = buttonContainer.AddComponent<RectTransform>();
            containerRect.anchorMin = new Vector2(0.25f, 0.15f);
            containerRect.anchorMax = new Vector2(0.75f, 0.5f);
            containerRect.sizeDelta = Vector2.zero;

            VerticalLayoutGroup layout = buttonContainer.AddComponent<VerticalLayoutGroup>();
            layout.spacing = 20f;
            layout.childAlignment = TextAnchor.MiddleCenter;
            layout.childControlWidth = true;
            layout.childControlHeight = false;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;

            // Continue Button
            CreateMenuButton(buttonContainer.transform, "CONTINUE", primaryColor, "ContinueButton");

            // Play Button
            CreateMenuButton(buttonContainer.transform, "LEVEL SELECT", secondaryColor, "PlayButton");

            // Settings Button
            CreateMenuButton(buttonContainer.transform, "SETTINGS", new Color(0.5f, 0.5f, 0.5f, 1f), "SettingsButton");

            // Quit Button
            CreateMenuButton(buttonContainer.transform, "QUIT", new Color(0.8f, 0.3f, 0.3f, 1f), "QuitButton");
        }

        private GameObject CreateMenuButton(Transform parent, string buttonText, Color color, string objectName)
        {
            GameObject buttonObj = new GameObject(objectName);
            buttonObj.transform.SetParent(parent, false);

            RectTransform rect = buttonObj.AddComponent<RectTransform>();
            rect.sizeDelta = new Vector2(400f, 80f);

            Image buttonImage = buttonObj.AddComponent<Image>();
            buttonImage.color = color;

            Button button = buttonObj.AddComponent<Button>();
            button.targetGraphic = buttonImage;

            // Button color transitions
            ColorBlock colors = button.colors;
            colors.normalColor = Color.white;
            colors.highlightedColor = new Color(1.2f, 1.2f, 1.2f, 1f);
            colors.pressedColor = new Color(0.8f, 0.8f, 0.8f, 1f);
            colors.selectedColor = new Color(1.1f, 1.1f, 1.1f, 1f);
            colors.disabledColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            button.colors = colors;

            // Button Text
            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(buttonObj.transform, false);

            RectTransform textRect = textObj.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = Vector2.zero;

            TextMeshProUGUI text = textObj.AddComponent<TextMeshProUGUI>();
            text.text = buttonText;
            text.fontSize = buttonFontSize;
            text.color = Color.white;
            text.font = customFont;
            text.alignment = TextAlignmentOptions.Center;
            text.fontStyle = FontStyles.Bold;

            LayoutElement layoutElement = buttonObj.AddComponent<LayoutElement>();
            layoutElement.minHeight = 80f;
            layoutElement.preferredHeight = 80f;

            return buttonObj;
        }

        private void CreateVersionText(Transform parent)
        {
            GameObject versionObj = new GameObject("Version Text");
            versionObj.transform.SetParent(parent, false);

            RectTransform rect = versionObj.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.8f, 0.02f);
            rect.anchorMax = new Vector2(0.98f, 0.08f);
            rect.sizeDelta = Vector2.zero;

            TextMeshProUGUI text = versionObj.AddComponent<TextMeshProUGUI>();
            text.text = "v1.0.0";
            text.fontSize = 18f;
            text.color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
            text.font = customFont;
            text.alignment = TextAlignmentOptions.BottomRight;
        }

        private void BuildSettingsUI(Transform parent)
        {
            // Title
            CreateSettingsTitle(parent);

            // Settings Content
            CreateSettingsContent(parent);

            // Back Button
            CreateSettingsBackButton(parent);
        }

        private void CreateSettingsTitle(Transform parent)
        {
            GameObject titleObj = new GameObject("Settings Title");
            titleObj.transform.SetParent(parent, false);

            RectTransform rect = titleObj.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.1f, 0.88f);
            rect.anchorMax = new Vector2(0.9f, 0.98f);
            rect.sizeDelta = Vector2.zero;

            TextMeshProUGUI title = titleObj.AddComponent<TextMeshProUGUI>();
            title.text = "SETTINGS";
            title.fontSize = 56f;
            title.color = Color.white;
            title.font = customFont;
            title.alignment = TextAlignmentOptions.Center;
            title.fontStyle = FontStyles.Bold;
        }

        private void CreateSettingsContent(Transform parent)
        {
            GameObject contentBG = new GameObject("Settings Content");
            contentBG.transform.SetParent(parent, false);

            RectTransform bgRect = contentBG.AddComponent<RectTransform>();
            bgRect.anchorMin = new Vector2(0.1f, 0.2f);
            bgRect.anchorMax = new Vector2(0.9f, 0.85f);
            bgRect.sizeDelta = Vector2.zero;

            Image bgImage = contentBG.AddComponent<Image>();
            bgImage.color = new Color(0.15f, 0.15f, 0.2f, 0.8f);

            VerticalLayoutGroup layout = contentBG.AddComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(40, 40, 40, 40);
            layout.spacing = 30f;
            layout.childAlignment = TextAnchor.UpperCenter;
            layout.childControlWidth = true;
            layout.childControlHeight = false;

            // Audio Settings Section
            CreateSectionHeader(contentBG.transform, "AUDIO");
            CreateSliderSetting(contentBG.transform, "Master Volume", "MasterVolumeSlider");
            CreateSliderSetting(contentBG.transform, "Music Volume", "MusicVolumeSlider");
            CreateSliderSetting(contentBG.transform, "SFX Volume", "SFXVolumeSlider");

            // Graphics Settings Section
            CreateSectionHeader(contentBG.transform, "GRAPHICS");
            CreateDropdownSetting(contentBG.transform, "Quality", "QualityDropdown", new string[] { "Low", "Medium", "High", "Ultra" });
            CreateToggleSetting(contentBG.transform, "VSync", "VSyncToggle");
            CreateToggleSetting(contentBG.transform, "Fullscreen", "FullscreenToggle");

            // Gameplay Settings Section
            CreateSectionHeader(contentBG.transform, "GAMEPLAY");
            CreateToggleSetting(contentBG.transform, "Vibration", "VibrationToggle");
            CreateToggleSetting(contentBG.transform, "Show FPS", "ShowFPSToggle");
        }

        private void CreateSectionHeader(Transform parent, string headerText)
        {
            GameObject headerObj = new GameObject($"{headerText} Header");
            headerObj.transform.SetParent(parent, false);

            RectTransform rect = headerObj.AddComponent<RectTransform>();
            rect.sizeDelta = new Vector2(0f, 40f);

            TextMeshProUGUI text = headerObj.AddComponent<TextMeshProUGUI>();
            text.text = headerText;
            text.fontSize = 32f;
            text.color = secondaryColor;
            text.font = customFont;
            text.alignment = TextAlignmentOptions.MidlineLeft;
            text.fontStyle = FontStyles.Bold;

            LayoutElement layoutElement = headerObj.AddComponent<LayoutElement>();
            layoutElement.minHeight = 40f;
            layoutElement.preferredHeight = 40f;
        }

        private void CreateSliderSetting(Transform parent, string labelText, string sliderName)
        {
            GameObject settingObj = new GameObject($"{sliderName} Container");
            settingObj.transform.SetParent(parent, false);

            RectTransform rect = settingObj.AddComponent<RectTransform>();
            rect.sizeDelta = new Vector2(0f, 60f);

            HorizontalLayoutGroup layout = settingObj.AddComponent<HorizontalLayoutGroup>();
            layout.childAlignment = TextAnchor.MiddleLeft;
            layout.childControlWidth = false;
            layout.childControlHeight = true;
            layout.spacing = 20f;

            // Label
            GameObject labelObj = new GameObject("Label");
            labelObj.transform.SetParent(settingObj.transform, false);

            RectTransform labelRect = labelObj.AddComponent<RectTransform>();
            labelRect.sizeDelta = new Vector2(300f, 50f);

            TextMeshProUGUI label = labelObj.AddComponent<TextMeshProUGUI>();
            label.text = labelText;
            label.fontSize = labelFontSize;
            label.color = Color.white;
            label.font = customFont;
            label.alignment = TextAlignmentOptions.MidlineLeft;

            LayoutElement labelLayout = labelObj.AddComponent<LayoutElement>();
            labelLayout.minWidth = 250f;
            labelLayout.preferredWidth = 300f;

            // Slider
            CreateSlider(settingObj.transform, sliderName);

            // Value Text
            GameObject valueObj = new GameObject("Value");
            valueObj.transform.SetParent(settingObj.transform, false);

            RectTransform valueRect = valueObj.AddComponent<RectTransform>();
            valueRect.sizeDelta = new Vector2(100f, 50f);

            TextMeshProUGUI valueText = valueObj.AddComponent<TextMeshProUGUI>();
            valueText.text = "100%";
            valueText.fontSize = labelFontSize;
            valueText.color = primaryColor;
            valueText.font = customFont;
            valueText.alignment = TextAlignmentOptions.MidlineRight;

            LayoutElement valueLayout = valueObj.AddComponent<LayoutElement>();
            valueLayout.minWidth = 80f;
            valueLayout.preferredWidth = 100f;

            LayoutElement settingLayout = settingObj.AddComponent<LayoutElement>();
            settingLayout.minHeight = 60f;
            settingLayout.preferredHeight = 60f;
        }

        private void CreateSlider(Transform parent, string sliderName)
        {
            GameObject sliderObj = new GameObject(sliderName);
            sliderObj.transform.SetParent(parent, false);

            RectTransform sliderRect = sliderObj.AddComponent<RectTransform>();
            sliderRect.sizeDelta = new Vector2(400f, 30f);

            Slider slider = sliderObj.AddComponent<Slider>();
            slider.minValue = 0f;
            slider.maxValue = 1f;
            slider.value = 1f;

            LayoutElement sliderLayout = sliderObj.AddComponent<LayoutElement>();
            sliderLayout.flexibleWidth = 1f;
            sliderLayout.minWidth = 200f;

            // Background
            GameObject background = new GameObject("Background");
            background.transform.SetParent(sliderObj.transform, false);

            RectTransform bgRect = background.AddComponent<RectTransform>();
            bgRect.anchorMin = Vector2.zero;
            bgRect.anchorMax = Vector2.one;
            bgRect.sizeDelta = Vector2.zero;

            Image bgImage = background.AddComponent<Image>();
            bgImage.color = new Color(0.2f, 0.2f, 0.25f, 1f);

            // Fill Area
            GameObject fillArea = new GameObject("Fill Area");
            fillArea.transform.SetParent(sliderObj.transform, false);

            RectTransform fillAreaRect = fillArea.AddComponent<RectTransform>();
            fillAreaRect.anchorMin = Vector2.zero;
            fillAreaRect.anchorMax = Vector2.one;
            fillAreaRect.sizeDelta = new Vector2(-10f, -10f);

            // Fill
            GameObject fill = new GameObject("Fill");
            fill.transform.SetParent(fillArea.transform, false);

            RectTransform fillRect = fill.AddComponent<RectTransform>();
            fillRect.anchorMin = Vector2.zero;
            fillRect.anchorMax = Vector2.one;
            fillRect.sizeDelta = Vector2.zero;

            Image fillImage = fill.AddComponent<Image>();
            fillImage.color = primaryColor;

            slider.fillRect = fillRect;

            // Handle Slide Area
            GameObject handleArea = new GameObject("Handle Slide Area");
            handleArea.transform.SetParent(sliderObj.transform, false);

            RectTransform handleAreaRect = handleArea.AddComponent<RectTransform>();
            handleAreaRect.anchorMin = Vector2.zero;
            handleAreaRect.anchorMax = Vector2.one;
            handleAreaRect.sizeDelta = new Vector2(-10f, 0f);

            // Handle
            GameObject handle = new GameObject("Handle");
            handle.transform.SetParent(handleArea.transform, false);

            RectTransform handleRect = handle.AddComponent<RectTransform>();
            handleRect.sizeDelta = new Vector2(30f, 30f);

            Image handleImage = handle.AddComponent<Image>();
            handleImage.color = Color.white;

            slider.handleRect = handleRect;
            slider.targetGraphic = handleImage;
        }

        private void CreateToggleSetting(Transform parent, string labelText, string toggleName)
        {
            GameObject settingObj = new GameObject($"{toggleName} Container");
            settingObj.transform.SetParent(parent, false);

            RectTransform rect = settingObj.AddComponent<RectTransform>();
            rect.sizeDelta = new Vector2(0f, 60f);

            HorizontalLayoutGroup layout = settingObj.AddComponent<HorizontalLayoutGroup>();
            layout.childAlignment = TextAnchor.MiddleLeft;
            layout.childControlWidth = false;
            layout.childControlHeight = true;
            layout.spacing = 20f;

            // Label
            GameObject labelObj = new GameObject("Label");
            labelObj.transform.SetParent(settingObj.transform, false);

            RectTransform labelRect = labelObj.AddComponent<RectTransform>();
            labelRect.sizeDelta = new Vector2(300f, 50f);

            TextMeshProUGUI label = labelObj.AddComponent<TextMeshProUGUI>();
            label.text = labelText;
            label.fontSize = labelFontSize;
            label.color = Color.white;
            label.font = customFont;
            label.alignment = TextAlignmentOptions.MidlineLeft;

            LayoutElement labelLayout = labelObj.AddComponent<LayoutElement>();
            labelLayout.flexibleWidth = 1f;

            // Toggle
            CreateToggle(settingObj.transform, toggleName);

            LayoutElement settingLayout = settingObj.AddComponent<LayoutElement>();
            settingLayout.minHeight = 60f;
            settingLayout.preferredHeight = 60f;
        }

        private void CreateToggle(Transform parent, string toggleName)
        {
            GameObject toggleObj = new GameObject(toggleName);
            toggleObj.transform.SetParent(parent, false);

            RectTransform toggleRect = toggleObj.AddComponent<RectTransform>();
            toggleRect.sizeDelta = new Vector2(80f, 40f);

            Toggle toggle = toggleObj.AddComponent<Toggle>();
            toggle.isOn = true;

            LayoutElement toggleLayout = toggleObj.AddComponent<LayoutElement>();
            toggleLayout.minWidth = 80f;
            toggleLayout.preferredWidth = 80f;

            // Background
            GameObject background = new GameObject("Background");
            background.transform.SetParent(toggleObj.transform, false);

            RectTransform bgRect = background.AddComponent<RectTransform>();
            bgRect.anchorMin = Vector2.zero;
            bgRect.anchorMax = Vector2.one;
            bgRect.sizeDelta = Vector2.zero;

            Image bgImage = background.AddComponent<Image>();
            bgImage.color = new Color(0.2f, 0.2f, 0.25f, 1f);

            toggle.targetGraphic = bgImage;

            // Checkmark
            GameObject checkmark = new GameObject("Checkmark");
            checkmark.transform.SetParent(background.transform, false);

            RectTransform checkRect = checkmark.AddComponent<RectTransform>();
            checkRect.anchorMin = new Vector2(0.1f, 0.1f);
            checkRect.anchorMax = new Vector2(0.9f, 0.9f);
            checkRect.sizeDelta = Vector2.zero;

            Image checkImage = checkmark.AddComponent<Image>();
            checkImage.color = primaryColor;

            toggle.graphic = checkImage;
        }

        private void CreateDropdownSetting(Transform parent, string labelText, string dropdownName, string[] options)
        {
            GameObject settingObj = new GameObject($"{dropdownName} Container");
            settingObj.transform.SetParent(parent, false);

            RectTransform rect = settingObj.AddComponent<RectTransform>();
            rect.sizeDelta = new Vector2(0f, 60f);

            HorizontalLayoutGroup layout = settingObj.AddComponent<HorizontalLayoutGroup>();
            layout.childAlignment = TextAnchor.MiddleLeft;
            layout.childControlWidth = false;
            layout.childControlHeight = true;
            layout.spacing = 20f;

            // Label
            GameObject labelObj = new GameObject("Label");
            labelObj.transform.SetParent(settingObj.transform, false);

            RectTransform labelRect = labelObj.AddComponent<RectTransform>();
            labelRect.sizeDelta = new Vector2(300f, 50f);

            TextMeshProUGUI label = labelObj.AddComponent<TextMeshProUGUI>();
            label.text = labelText;
            label.fontSize = labelFontSize;
            label.color = Color.white;
            label.font = customFont;
            label.alignment = TextAlignmentOptions.MidlineLeft;

            LayoutElement labelLayout = labelObj.AddComponent<LayoutElement>();
            labelLayout.minWidth = 250f;
            labelLayout.preferredWidth = 300f;

            // Dropdown
            CreateDropdown(settingObj.transform, dropdownName, options);

            LayoutElement settingLayout = settingObj.AddComponent<LayoutElement>();
            settingLayout.minHeight = 60f;
            settingLayout.preferredHeight = 60f;
        }

        private void CreateDropdown(Transform parent, string dropdownName, string[] options)
        {
            GameObject dropdownObj = new GameObject(dropdownName);
            dropdownObj.transform.SetParent(parent, false);

            RectTransform dropdownRect = dropdownObj.AddComponent<RectTransform>();
            dropdownRect.sizeDelta = new Vector2(300f, 50f);

            Image dropdownImage = dropdownObj.AddComponent<Image>();
            dropdownImage.color = new Color(0.2f, 0.2f, 0.25f, 1f);

            TMP_Dropdown dropdown = dropdownObj.AddComponent<TMP_Dropdown>();
            dropdown.ClearOptions();
            dropdown.AddOptions(new System.Collections.Generic.List<string>(options));

            LayoutElement dropdownLayout = dropdownObj.AddComponent<LayoutElement>();
            dropdownLayout.flexibleWidth = 1f;
            dropdownLayout.minWidth = 200f;

            // Label (selected option text)
            GameObject labelObj = new GameObject("Label");
            labelObj.transform.SetParent(dropdownObj.transform, false);

            RectTransform labelRect = labelObj.AddComponent<RectTransform>();
            labelRect.anchorMin = Vector2.zero;
            labelRect.anchorMax = Vector2.one;
            labelRect.sizeDelta = new Vector2(-30f, 0f);
            labelRect.anchoredPosition = new Vector2(-5f, 0f);

            TextMeshProUGUI labelText = labelObj.AddComponent<TextMeshProUGUI>();
            labelText.text = options[0];
            labelText.fontSize = labelFontSize;
            labelText.color = Color.white;
            labelText.font = customFont;
            labelText.alignment = TextAlignmentOptions.MidlineLeft;

            dropdown.captionText = labelText;

            // Arrow
            GameObject arrow = new GameObject("Arrow");
            arrow.transform.SetParent(dropdownObj.transform, false);

            RectTransform arrowRect = arrow.AddComponent<RectTransform>();
            arrowRect.anchorMin = new Vector2(1f, 0.5f);
            arrowRect.anchorMax = new Vector2(1f, 0.5f);
            arrowRect.sizeDelta = new Vector2(20f, 20f);
            arrowRect.anchoredPosition = new Vector2(-15f, 0f);

            Image arrowImage = arrow.AddComponent<Image>();
            arrowImage.color = Color.white;

            // Template (dropdown list - simplified)
            GameObject template = new GameObject("Template");
            template.transform.SetParent(dropdownObj.transform, false);
            template.SetActive(false);

            RectTransform templateRect = template.AddComponent<RectTransform>();
            templateRect.anchorMin = new Vector2(0f, 0f);
            templateRect.anchorMax = new Vector2(1f, 0f);
            templateRect.pivot = new Vector2(0.5f, 1f);
            templateRect.sizeDelta = new Vector2(0f, 150f);
            templateRect.anchoredPosition = new Vector2(0f, 2f);

            dropdown.template = templateRect;
        }

        private void CreateSettingsBackButton(Transform parent)
        {
            GameObject buttonObj = new GameObject("Back Button");
            buttonObj.transform.SetParent(parent, false);

            RectTransform rect = buttonObj.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.35f, 0.05f);
            rect.anchorMax = new Vector2(0.65f, 0.15f);
            rect.sizeDelta = Vector2.zero;

            Image buttonImage = buttonObj.AddComponent<Image>();
            buttonImage.color = new Color(0.8f, 0.3f, 0.3f, 1f);

            Button button = buttonObj.AddComponent<Button>();
            button.targetGraphic = buttonImage;

            // Button Text
            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(buttonObj.transform, false);

            RectTransform textRect = textObj.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = Vector2.zero;

            TextMeshProUGUI text = textObj.AddComponent<TextMeshProUGUI>();
            text.text = "BACK";
            text.fontSize = buttonFontSize;
            text.color = Color.white;
            text.font = customFont;
            text.alignment = TextAlignmentOptions.Center;
            text.fontStyle = FontStyles.Bold;
        }

        private void ClearChildren(Transform parent)
        {
            int childCount = parent.childCount;
            for (int i = childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(parent.GetChild(i).gameObject);
            }
        }
    }
}
