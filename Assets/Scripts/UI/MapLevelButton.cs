using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BasketLegend.Data;
using BasketLegend.Managers;

namespace BasketLegend.UI
{
    public class MapLevelButton : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Button button;
        [SerializeField] private Image levelIcon;
        [SerializeField] private Image backgroundRing;
        [SerializeField] private TextMeshProUGUI levelNumberText;
        [SerializeField] private GameObject lockedOverlay;
        [SerializeField] private Transform starsContainer;
        [SerializeField] private Image[] starImages;

        [Header("Level Type Icons")]
        [SerializeField] private Sprite normalLevelIcon;
        [SerializeField] private Sprite bossLevelIcon;
        [SerializeField] private Sprite bonusLevelIcon;
        [SerializeField] private Sprite challengeLevelIcon;

        [Header("Visual States")]
        [SerializeField] private Color unlockedColor = Color.white;
        [SerializeField] private Color lockedColor = Color.gray;
        [SerializeField] private Color completedColor = Color.green;
        [SerializeField] private Color currentLevelColor = Color.yellow;

        [Header("Animation")]
        [SerializeField] private float pulseScale = 1.1f;
        [SerializeField] private float pulseDuration = 1f;

        private LevelData levelData;
        private bool isCurrentLevel;

        public void Initialize(LevelData data)
        {
            levelData = data;
            UpdateVisuals();
            SetupButton();
            CheckIfCurrentLevel();
        }

        public LevelData GetLevelData()
        {
            return levelData;
        }

        private void UpdateVisuals()
        {
            if (levelData == null) return;

            bool isUnlocked = LevelManager.Instance.IsLevelUnlocked(levelData.levelNumber);
            bool isCompleted = levelData.starsEarned > 0;

            UpdateLevelNumber();
            UpdateLevelIcon();
            UpdateLockState(isUnlocked);
            UpdateStars();
            UpdateColors(isUnlocked, isCompleted);
        }

        private void UpdateLevelNumber()
        {
            if (levelNumberText != null)
            {
                levelNumberText.text = levelData.levelNumber.ToString();
            }
        }

        private void UpdateLevelIcon()
        {
            if (levelIcon == null) return;

            Sprite iconToUse = levelData.levelType switch
            {
                LevelType.Boss => bossLevelIcon,
                LevelType.Bonus => bonusLevelIcon,
                LevelType.Challenge => challengeLevelIcon,
                _ => normalLevelIcon
            };

            if (iconToUse != null)
            {
                levelIcon.sprite = iconToUse;
            }
        }

        private void UpdateLockState(bool isUnlocked)
        {
            if (lockedOverlay != null)
            {
                lockedOverlay.SetActive(!isUnlocked);
            }

            if (levelNumberText != null)
            {
                levelNumberText.gameObject.SetActive(isUnlocked);
            }

            if (starsContainer != null)
            {
                starsContainer.gameObject.SetActive(isUnlocked);
            }
        }

        private void UpdateStars()
        {
            if (starImages == null || starImages.Length == 0) return;

            for (int i = 0; i < starImages.Length; i++)
            {
                if (starImages[i] != null)
                {
                    bool shouldShowStar = i < levelData.starsEarned;
                    starImages[i].gameObject.SetActive(shouldShowStar);
                    
                    if (shouldShowStar)
                    {
                        starImages[i].color = Color.yellow;
                    }
                }
            }
        }

        private void UpdateColors(bool isUnlocked, bool isCompleted)
        {
            Color targetColor;

            if (!isUnlocked)
            {
                targetColor = lockedColor;
            }
            else if (isCurrentLevel)
            {
                targetColor = currentLevelColor;
            }
            else if (isCompleted)
            {
                targetColor = completedColor;
            }
            else
            {
                targetColor = unlockedColor;
            }

            if (backgroundRing != null)
            {
                backgroundRing.color = targetColor;
            }

            if (levelIcon != null)
            {
                levelIcon.color = isUnlocked ? Color.white : lockedColor;
            }
        }

        private void CheckIfCurrentLevel()
        {
            var nextLevel = SeasonManager.Instance?.GetNextUnlockedLevel();
            isCurrentLevel = nextLevel != null && nextLevel.levelNumber == levelData.levelNumber;

            if (isCurrentLevel)
            {
                StartPulseAnimation();
            }
        }

        private void StartPulseAnimation()
        {
            if (!isCurrentLevel) return;

            // Simple pulse animation using LeanTween or similar
            StartCoroutine(PulseCoroutine());
        }

        private System.Collections.IEnumerator PulseCoroutine()
        {
            Vector3 originalScale = transform.localScale;
            Vector3 targetScale = originalScale * pulseScale;

            while (isCurrentLevel && gameObject.activeInHierarchy)
            {
                // Scale up
                float elapsedTime = 0f;
                while (elapsedTime < pulseDuration * 0.5f)
                {
                    float t = elapsedTime / (pulseDuration * 0.5f);
                    transform.localScale = Vector3.Lerp(originalScale, targetScale, t);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                // Scale down
                elapsedTime = 0f;
                while (elapsedTime < pulseDuration * 0.5f)
                {
                    float t = elapsedTime / (pulseDuration * 0.5f);
                    transform.localScale = Vector3.Lerp(targetScale, originalScale, t);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
            }

            transform.localScale = originalScale;
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
                // Play button click sound
                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.PlayButtonClick();
                }

                // Load the level
                LevelManager.Instance.LoadLevel(levelData.levelNumber);
            }
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
}