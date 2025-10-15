using UnityEngine;

namespace BasketLegend.Core
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "BasketLegend/Game Config")]
    public class GameConfig : ScriptableObject
    {
        [Header("Scene Configuration")]
        public string splashSceneName = "SplashScene";
        public string gameSceneName = "GameScene";
        public string demoSceneName = "demo";

        [Header("Loading Configuration")]
        public float minimumSplashTime = 2f;
        public float fadeTransitionDuration = 0.5f;

        [Header("Player Progression")]
        public int baseExperienceRequirement = 100;
        public float experienceMultiplier = 1.5f;
        public int maxPlayerLevel = 99;

        [Header("Level Configuration")]
        public int maxLevels = 50;
        public int initialUnlockedLevels = 1;

        [Header("Audio Configuration")]
        public float defaultMasterVolume = 0.8f;
        public float defaultMusicVolume = 0.7f;
        public float defaultSFXVolume = 0.8f;

        [Header("UI Configuration")]
        public float buttonAnimationDuration = 0.2f;
        public float panelTransitionDuration = 0.3f;
    }
}