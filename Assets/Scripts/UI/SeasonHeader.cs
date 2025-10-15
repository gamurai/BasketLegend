using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BasketLegend.Data;
using BasketLegend.Managers;

namespace BasketLegend.UI
{
    public class SeasonHeader : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Image seasonIcon;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private TextMeshProUGUI seasonNameText;
        [SerializeField] private TextMeshProUGUI progressText;
        [SerializeField] private Slider progressBar;
        [SerializeField] private GameObject completedBadge;
        [SerializeField] private GameObject lockedOverlay;

        [Header("Visual Effects")]
        [SerializeField] private ParticleSystem completionEffect;

        private SeasonData seasonData;

        public void Initialize(SeasonData data)
        {
            seasonData = data;
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            if (seasonData == null) return;

            UpdateSeasonInfo();
            UpdateProgress();
            UpdateVisualState();
        }

        private void UpdateSeasonInfo()
        {
            if (seasonNameText != null)
            {
                seasonNameText.text = seasonData.seasonName;
            }

            if (seasonIcon != null && seasonData.seasonIcon != null)
            {
                seasonIcon.sprite = seasonData.seasonIcon;
            }

            if (backgroundImage != null)
            {
                backgroundImage.color = seasonData.seasonThemeColor;
                
                if (seasonData.seasonBackground != null)
                {
                    backgroundImage.sprite = seasonData.seasonBackground;
                }
            }
        }

        private void UpdateProgress()
        {
            if (SeasonManager.Instance == null) return;

            bool isUnlocked = SeasonManager.Instance.IsSeasonUnlocked(seasonData.seasonNumber);
            bool isCompleted = SeasonManager.Instance.IsSeasonCompleted(seasonData.seasonNumber);
            
            if (!isUnlocked)
            {
                UpdateLockedState();
                return;
            }

            int progress = SeasonManager.Instance.GetSeasonProgress(seasonData.seasonNumber);
            float progressPercentage = SeasonManager.Instance.GetSeasonProgressPercentage(seasonData.seasonNumber);

            if (progressText != null)
            {
                progressText.text = $"{progress}/{seasonData.seasonLevels.Count}";
            }

            if (progressBar != null)
            {
                progressBar.value = progressPercentage;
            }

            if (completedBadge != null)
            {
                completedBadge.SetActive(isCompleted);
            }

            if (isCompleted && completionEffect != null && !completionEffect.isPlaying)
            {
                completionEffect.Play();
            }
        }

        private void UpdateLockedState()
        {
            if (progressText != null)
            {
                progressText.text = "LOCKED";
            }

            if (progressBar != null)
            {
                progressBar.value = 0f;
            }

            if (completedBadge != null)
            {
                completedBadge.SetActive(false);
            }
        }

        private void UpdateVisualState()
        {
            bool isUnlocked = SeasonManager.Instance.IsSeasonUnlocked(seasonData.seasonNumber);

            if (lockedOverlay != null)
            {
                lockedOverlay.SetActive(!isUnlocked);
            }

            // Adjust alpha for locked seasons
            CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }

            canvasGroup.alpha = isUnlocked ? 1f : 0.5f;
        }

        private void OnEnable()
        {
            if (seasonData != null)
            {
                UpdateDisplay();
            }
        }
    }
}