using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BasketLegend.Data;

namespace BasketLegend.UI
{
    public class MainMenuPanel : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Button playButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button quitButton;
        
        [Header("Player Info")]
        [SerializeField] private TextMeshProUGUI playerNameText;
        [SerializeField] private TextMeshProUGUI playerLevelText;
        [SerializeField] private Slider experienceSlider;

        private void OnEnable()
        {
            UpdatePlayerInfo();
        }

        private void UpdatePlayerInfo()
        {
            if (PlayerDataManager.Instance != null)
            {
                if (playerNameText != null)
                {
                    playerNameText.text = PlayerDataManager.Instance.PlayerName;
                }

                if (playerLevelText != null)
                {
                    playerLevelText.text = $"Level {PlayerDataManager.Instance.PlayerLevel}";
                }

                if (experienceSlider != null)
                {
                    int currentExp = PlayerDataManager.Instance.Experience;
                    int requiredExp = PlayerDataManager.Instance.PlayerLevel * 100;
                    experienceSlider.value = (float)currentExp / requiredExp;
                }
            }
        }
    }
}