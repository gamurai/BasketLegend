using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BasketLegend.Data;
using BasketLegend.Managers;

namespace BasketLegend.UI
{
    public class LevelButton : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Button button;
        [SerializeField] private Image levelThumbnail;
        [SerializeField] private TextMeshProUGUI levelNumberText;
        [SerializeField] private TextMeshProUGUI levelNameText;
        [SerializeField] private GameObject lockedOverlay;
        [SerializeField] private Image[] starImages;

        [Header("Visual States")]
        [SerializeField] private Color unlockedColor = Color.white;
        [SerializeField] private Color lockedColor = Color.gray;

        private LevelData levelData;

        public void Initialize(LevelData data)
        {
            levelData = data;
            UpdateVisuals();
            SetupButton();
        }

        private void UpdateVisuals()
        {
            if (levelData == null) return;

            bool isUnlocked = LevelManager.Instance.IsLevelUnlocked(levelData.levelNumber);

            if (levelNumberText != null)
            {
                levelNumberText.text = levelData.levelNumber.ToString();
            }

            if (levelNameText != null)
            {
                levelNameText.text = levelData.levelName;
            }

            if (levelThumbnail != null && levelData.levelThumbnail != null)
            {
                levelThumbnail.sprite = levelData.levelThumbnail;
                levelThumbnail.color = isUnlocked ? unlockedColor : lockedColor;
            }

            if (lockedOverlay != null)
            {
                lockedOverlay.SetActive(!isUnlocked);
            }

            UpdateStars();
        }

        private void UpdateStars()
        {
            if (starImages == null || starImages.Length == 0) return;

            for (int i = 0; i < starImages.Length; i++)
            {
                if (starImages[i] != null)
                {
                    starImages[i].gameObject.SetActive(false);
                }
            }
        }

        private void SetupButton()
        {
            if (button != null)
            {
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(OnLevelButtonClicked);
                
                bool isUnlocked = LevelManager.Instance.IsLevelUnlocked(levelData.levelNumber);
                button.interactable = isUnlocked;
            }
        }

        private void OnLevelButtonClicked()
        {
            if (levelData != null && LevelManager.Instance != null)
            {
                LevelManager.Instance.LoadLevel(levelData.levelNumber);
            }
        }
    }
}