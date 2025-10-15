using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

namespace BasketLegend.Managers
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [Header("Audio Sources")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private AudioSource uiSource;

        [Header("Audio Clips")]
        [SerializeField] private AudioClip backgroundMusic;
        [SerializeField] private AudioClip buttonClickSound;
        [SerializeField] private AudioClip basketScoredSound;
        [SerializeField] private AudioClip ballBounceSound;

        [Header("Audio Mixer")]
        [SerializeField] private AudioMixer audioMixer;

        private Dictionary<string, AudioClip> audioClips;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeAudioClips();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            PlayBackgroundMusic();
        }

        private void InitializeAudioClips()
        {
            audioClips = new Dictionary<string, AudioClip>();
            
            if (buttonClickSound != null)
                audioClips.Add("ButtonClick", buttonClickSound);
            
            if (basketScoredSound != null)
                audioClips.Add("BasketScored", basketScoredSound);
            
            if (ballBounceSound != null)
                audioClips.Add("BallBounce", ballBounceSound);
        }

        public void PlayBackgroundMusic()
        {
            if (musicSource != null && backgroundMusic != null)
            {
                musicSource.clip = backgroundMusic;
                musicSource.loop = true;
                musicSource.Play();
            }
        }

        public void StopBackgroundMusic()
        {
            if (musicSource != null)
            {
                musicSource.Stop();
            }
        }

        public void PlaySFX(string clipName)
        {
            if (audioClips.ContainsKey(clipName) && sfxSource != null)
            {
                sfxSource.PlayOneShot(audioClips[clipName]);
            }
        }

        public void PlaySFX(AudioClip clip)
        {
            if (sfxSource != null && clip != null)
            {
                sfxSource.PlayOneShot(clip);
            }
        }

        public void PlayUISound(string clipName)
        {
            if (audioClips.ContainsKey(clipName) && uiSource != null)
            {
                uiSource.PlayOneShot(audioClips[clipName]);
            }
        }

        public void PlayButtonClick()
        {
            PlayUISound("ButtonClick");
        }

        public void SetMusicVolume(float volume)
        {
            if (musicSource != null)
            {
                musicSource.volume = volume;
            }
        }

        public void SetSFXVolume(float volume)
        {
            if (sfxSource != null)
            {
                sfxSource.volume = volume;
            }
        }

        public void MuteAll(bool mute)
        {
            AudioListener.pause = mute;
        }
    }
}