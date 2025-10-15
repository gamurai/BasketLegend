using UnityEngine;
using UnityEngine.Audio;

namespace BasketLegend.Managers
{
    public class SettingsManager : MonoBehaviour
    {
        public static SettingsManager Instance { get; private set; }

        [Header("Audio")]
        [SerializeField] private AudioMixer audioMixer;

        private const string MASTER_VOLUME_KEY = "MasterVolume";
        private const string MUSIC_VOLUME_KEY = "MusicVolume";
        private const string SFX_VOLUME_KEY = "SFXVolume";
        private const string MUTE_KEY = "IsMuted";
        private const string QUALITY_KEY = "QualityLevel";
        private const string FULLSCREEN_KEY = "IsFullscreen";

        private const string MASTER_VOLUME_MIXER = "MasterVolume";
        private const string MUSIC_VOLUME_MIXER = "MusicVolume";
        private const string SFX_VOLUME_MIXER = "SFXVolume";

        public float MasterVolume { get; private set; }
        public float MusicVolume { get; private set; }
        public float SFXVolume { get; private set; }
        public bool IsMuted { get; private set; }
        public int QualityLevel { get; private set; }
        public bool IsFullscreen { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                LoadSettings();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void LoadSettings()
        {
            MasterVolume = PlayerPrefs.GetFloat(MASTER_VOLUME_KEY, 0.8f);
            MusicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 0.7f);
            SFXVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 0.8f);
            IsMuted = PlayerPrefs.GetInt(MUTE_KEY, 0) == 1;
            QualityLevel = PlayerPrefs.GetInt(QUALITY_KEY, QualitySettings.GetQualityLevel());
            IsFullscreen = PlayerPrefs.GetInt(FULLSCREEN_KEY, Screen.fullScreen ? 1 : 0) == 1;

            ApplySettings();
        }

        private void ApplySettings()
        {
            ApplyAudioSettings();
            ApplyGraphicsSettings();
        }

        public void SetMasterVolume(float volume)
        {
            MasterVolume = Mathf.Clamp01(volume);
            ApplyMasterVolume();
            SaveSettings();
        }

        public void SetMusicVolume(float volume)
        {
            MusicVolume = Mathf.Clamp01(volume);
            ApplyMusicVolume();
            SaveSettings();
        }

        public void SetSFXVolume(float volume)
        {
            SFXVolume = Mathf.Clamp01(volume);
            ApplySFXVolume();
            SaveSettings();
        }

        public void SetMute(bool mute)
        {
            IsMuted = mute;
            ApplyAudioSettings();
            SaveSettings();
        }

        public void SetQuality(int qualityIndex)
        {
            QualityLevel = qualityIndex;
            QualitySettings.SetQualityLevel(qualityIndex);
            SaveSettings();
        }

        public void SetFullscreen(bool fullscreen)
        {
            IsFullscreen = fullscreen;
            Screen.fullScreen = fullscreen;
            SaveSettings();
        }

        private void ApplyAudioSettings()
        {
            if (audioMixer == null) return;

            if (IsMuted)
            {
                audioMixer.SetFloat(MASTER_VOLUME_MIXER, -80f);
            }
            else
            {
                ApplyMasterVolume();
                ApplyMusicVolume();
                ApplySFXVolume();
            }
        }

        private void ApplyMasterVolume()
        {
            if (audioMixer != null && !IsMuted)
            {
                float dbValue = MasterVolume > 0 ? Mathf.Log10(MasterVolume) * 20 : -80f;
                audioMixer.SetFloat(MASTER_VOLUME_MIXER, dbValue);
            }
        }

        private void ApplyMusicVolume()
        {
            if (audioMixer != null && !IsMuted)
            {
                float dbValue = MusicVolume > 0 ? Mathf.Log10(MusicVolume) * 20 : -80f;
                audioMixer.SetFloat(MUSIC_VOLUME_MIXER, dbValue);
            }
        }

        private void ApplySFXVolume()
        {
            if (audioMixer != null && !IsMuted)
            {
                float dbValue = SFXVolume > 0 ? Mathf.Log10(SFXVolume) * 20 : -80f;
                audioMixer.SetFloat(SFX_VOLUME_MIXER, dbValue);
            }
        }

        private void ApplyGraphicsSettings()
        {
            QualitySettings.SetQualityLevel(QualityLevel);
            Screen.fullScreen = IsFullscreen;
        }

        private void SaveSettings()
        {
            PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, MasterVolume);
            PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, MusicVolume);
            PlayerPrefs.SetFloat(SFX_VOLUME_KEY, SFXVolume);
            PlayerPrefs.SetInt(MUTE_KEY, IsMuted ? 1 : 0);
            PlayerPrefs.SetInt(QUALITY_KEY, QualityLevel);
            PlayerPrefs.SetInt(FULLSCREEN_KEY, IsFullscreen ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
}