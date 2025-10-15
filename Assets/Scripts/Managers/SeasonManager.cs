using UnityEngine;
using System.Collections.Generic;
using BasketLegend.Data;

namespace BasketLegend.Managers
{
    public class SeasonManager : MonoBehaviour
    {
        public static SeasonManager Instance { get; private set; }

        [Header("Season Configuration")]
        [SerializeField] private List<SeasonData> allSeasons = new List<SeasonData>();
        [SerializeField] private int levelsPerSeason = 20;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeSeasons();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void InitializeSeasons()
        {
            foreach (var season in allSeasons)
            {
                if (season != null)
                {
                    UpdateSeasonUnlockStatus(season);
                }
            }
        }

        public List<SeasonData> GetAllSeasons()
        {
            return allSeasons;
        }

        public SeasonData GetSeasonData(int seasonNumber)
        {
            return allSeasons.Find(season => season.seasonNumber == seasonNumber);
        }

        public SeasonData GetSeasonForLevel(int levelNumber)
        {
            int seasonNumber = Mathf.CeilToInt((float)levelNumber / levelsPerSeason);
            return GetSeasonData(seasonNumber);
        }

        public List<SeasonData> GetUnlockedSeasons()
        {
            var unlockedSeasons = new List<SeasonData>();
            
            foreach (var season in allSeasons)
            {
                if (IsSeasonUnlocked(season.seasonNumber))
                {
                    unlockedSeasons.Add(season);
                }
            }

            return unlockedSeasons;
        }

        public bool IsSeasonUnlocked(int seasonNumber)
        {
            if (seasonNumber == 1) return true; // First season always unlocked

            // Check if previous season is completed
            var previousSeason = GetSeasonData(seasonNumber - 1);
            if (previousSeason == null) return false;

            return IsSeasonCompleted(seasonNumber - 1);
        }

        public bool IsSeasonCompleted(int seasonNumber)
        {
            var season = GetSeasonData(seasonNumber);
            if (season == null) return false;

            foreach (var level in season.seasonLevels)
            {
                if (level != null && level.starsEarned == 0)
                {
                    return false; // Season not completed if any level has no stars
                }
            }

            return true;
        }

        public int GetSeasonProgress(int seasonNumber)
        {
            var season = GetSeasonData(seasonNumber);
            if (season == null) return 0;

            int completedLevels = 0;
            foreach (var level in season.seasonLevels)
            {
                if (level != null && level.starsEarned > 0)
                {
                    completedLevels++;
                }
            }

            return completedLevels;
        }

        public float GetSeasonProgressPercentage(int seasonNumber)
        {
            var season = GetSeasonData(seasonNumber);
            if (season == null) return 0f;

            int progress = GetSeasonProgress(seasonNumber);
            return (float)progress / season.seasonLevels.Count;
        }

        public LevelData GetNextUnlockedLevel()
        {
            int playerUnlockedLevels = PlayerDataManager.Instance.UnlockedLevels;
            
            foreach (var season in allSeasons)
            {
                if (!IsSeasonUnlocked(season.seasonNumber)) continue;

                foreach (var level in season.seasonLevels)
                {
                    if (level != null && level.levelNumber <= playerUnlockedLevels && level.starsEarned == 0)
                    {
                        return level;
                    }
                }
            }

            return null;
        }

        private void UpdateSeasonUnlockStatus(SeasonData season)
        {
            season.isUnlocked = IsSeasonUnlocked(season.seasonNumber);
        }

        public void UnlockNextSeason(int currentSeasonNumber)
        {
            var nextSeason = GetSeasonData(currentSeasonNumber + 1);
            if (nextSeason != null && IsSeasonCompleted(currentSeasonNumber))
            {
                nextSeason.isUnlocked = true;
            }
        }
    }
}