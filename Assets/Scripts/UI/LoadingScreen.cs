using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BasketLegend.UI
{
    public class LoadingScreen : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject loadingPanel;
        [SerializeField] private Slider progressBar;
        [SerializeField] private TextMeshProUGUI loadingText;
        [SerializeField] private TextMeshProUGUI progressText;

        public void ShowLoading()
        {
            if (loadingPanel != null)
            {
                loadingPanel.SetActive(true);
            }
            UpdateProgress(0f, "Initializing...");
        }

        public void HideLoading()
        {
            if (loadingPanel != null)
            {
                loadingPanel.SetActive(false);
            }
        }

        public void UpdateProgress(float progress, string message = "")
        {
            if (progressBar != null)
            {
                progressBar.value = progress;
            }

            if (loadingText != null && !string.IsNullOrEmpty(message))
            {
                loadingText.text = message;
            }

            if (progressText != null)
            {
                progressText.text = $"{Mathf.RoundToInt(progress * 100)}%";
            }
        }
    }
}