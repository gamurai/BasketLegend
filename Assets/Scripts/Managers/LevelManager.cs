using UnityEngine;
using System.Collections.Generic;
using BasketLegend.Data;

namespace BasketLegend.Managers
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { get; private set; }

        [Header("Level Configuration")]
        [SerializeField] private List<LevelData> allLevels = new List<LevelData>();
        [SerializeField] private string demoSceneName = "demo";

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public List<LevelData> GetAllLevels()
        {
            return allLevels;
        }

        public LevelData GetLevelData(int levelNumber)
        {
            return allLevels.Find(level => level.levelNumber == levelNumber);
        }

        public List<LevelData> GetUnlockedLevels()
        {
            var unlockedLevels = new List<LevelData>();
            int playerUnlockedLevels = PlayerDataManager.Instance.UnlockedLevels;

            foreach (var level in allLevels)
            {
                if (level.levelNumber <= playerUnlockedLevels)
                {
                    unlockedLevels.Add(level);
                }
            }

            return unlockedLevels;
        }

        public void LoadLevel(int levelNumber)
        {
            LevelData levelData = GetLevelData(levelNumber);
            if (levelData != null)
            {
                string sceneToLoad = string.IsNullOrEmpty(levelData.sceneName) ? demoSceneName : levelData.sceneName;
                SceneController.Instance.LoadScene(sceneToLoad);
            }
            else
            {
                SceneController.Instance.LoadScene(demoSceneName);
            }
        }

        public bool IsLevelUnlocked(int levelNumber)
        {
            return levelNumber <= PlayerDataManager.Instance.UnlockedLevels;
        }

        public void CompleteLevel(int levelNumber, int starsEarned)
        {
            LevelData level = GetLevelData(levelNumber);
            if (level != null)
            {
                // Update stars earned (only if better than previous)
                if (starsEarned > level.starsEarned)
                {
                    level.starsEarned = starsEarned;
                }

                // Unlock next level
                int nextLevelNumber = levelNumber + 1;
                if (PlayerDataManager.Instance.UnlockedLevels < nextLevelNumber)
                {
                    PlayerDataManager.Instance.UnlockedLevels = nextLevelNumber;
                }

                // Add experience
                PlayerDataManager.Instance.AddExperience(level.experienceReward * starsEarned);

                // Check if season is completed
                if (SeasonManager.Instance != null)
                {
                    var season = SeasonManager.Instance.GetSeasonForLevel(levelNumber);
                    if (season != null && SeasonManager.Instance.IsSeasonCompleted(season.seasonNumber))
                    {
                        SeasonManager.Instance.UnlockNextSeason(season.seasonNumber);
                    }
                }
            }
        }
    }
}