using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BasketLegend.Managers;

namespace BasketLegend.UI
{
    public class SettingsPanel : MonoBehaviour
    {
        [Header("Audio Settings")]
        [SerializeField] private Slider masterVolumeSlider;
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider sfxVolumeSlider;
        [SerializeField] private Toggle muteToggle;

        [Header("Graphics Settings")]
        [SerializeField] private TMP_Dropdown qualityDropdown;
        [SerializeField] private Toggle fullscreenToggle;

        [Header("Player Settings")]
        [SerializeField] private TMP_InputField playerNameInput;
        [SerializeField] private Button saveButton;
        [SerializeField] private Button backButton;

        private void OnEnable()
        {
            LoadSettings();
        }

        private void Start()
        {
            SetupEventListeners();
        }

        private void SetupEventListeners()
        {
            if (masterVolumeSlider != null)
                masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);

            if (musicVolumeSlider != null)
                musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);

            if (sfxVolumeSlider != null)
                sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);

            if (muteToggle != null)
                muteToggle.onValueChanged.AddListener(OnMuteToggleChanged);

            if (qualityDropdown != null)
                qualityDropdown.onValueChanged.AddListener(OnQualityChanged);

            if (fullscreenToggle != null)
                fullscreenToggle.onValueChanged.AddListener(OnFullscreenToggleChanged);

            if (saveButton != null)
                saveButton.onClick.AddListener(SavePlayerName);
        }

        private void LoadSettings()
        {
            if (SettingsManager.Instance != null)
            {
                if (masterVolumeSlider != null)
                    masterVolumeSlider.value = SettingsManager.Instance.MasterVolume;

                if (musicVolumeSlider != null)
                    musicVolumeSlider.value = SettingsManager.Instance.MusicVolume;

                if (sfxVolumeSlider != null)
                    sfxVolumeSlider.value = SettingsManager.Instance.SFXVolume;

                if (muteToggle != null)
                    muteToggle.isOn = SettingsManager.Instance.IsMuted;

                if (qualityDropdown != null)
                    qualityDropdown.value = SettingsManager.Instance.QualityLevel;

                if (fullscreenToggle != null)
                    fullscreenToggle.isOn = SettingsManager.Instance.IsFullscreen;
            }

            if (playerNameInput != null && BasketLegend.Data.PlayerDataManager.Instance != null)
            {
                playerNameInput.text = BasketLegend.Data.PlayerDataManager.Instance.PlayerName;
            }
        }

        private void OnMasterVolumeChanged(float value)
        {
            if (SettingsManager.Instance != null)
            {
                SettingsManager.Instance.SetMasterVolume(value);
            }
        }

        private void OnMusicVolumeChanged(float value)
        {
            if (SettingsManager.Instance != null)
            {
                SettingsManager.Instance.SetMusicVolume(value);
            }
        }

        private void OnSFXVolumeChanged(float value)
        {
            if (SettingsManager.Instance != null)
            {
                SettingsManager.Instance.SetSFXVolume(value);
            }
        }

        private void OnMuteToggleChanged(bool isMuted)
        {
            if (SettingsManager.Instance != null)
            {
                SettingsManager.Instance.SetMute(isMuted);
            }
        }

        private void OnQualityChanged(int qualityIndex)
        {
            if (SettingsManager.Instance != null)
            {
                SettingsManager.Instance.SetQuality(qualityIndex);
            }
        }

        private void OnFullscreenToggleChanged(bool isFullscreen)
        {
            if (SettingsManager.Instance != null)
            {
                SettingsManager.Instance.SetFullscreen(isFullscreen);
            }
        }

        private void SavePlayerName()
        {
            if (playerNameInput != null && BasketLegend.Data.PlayerDataManager.Instance != null)
            {
                string newName = playerNameInput.text.Trim();
                if (!string.IsNullOrEmpty(newName))
                {
                    BasketLegend.Data.PlayerDataManager.Instance.PlayerName = newName;
                    BasketLegend.Data.PlayerDataManager.Instance.SaveData();
                }
            }
        }
    }
}