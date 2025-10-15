using UnityEngine;
using BasketLegend.Core;

namespace BasketLegend.Data
{
    public class PlayerDataManager : MonoBehaviour, IPlayerData
    {
        public static PlayerDataManager Instance { get; private set; }

        [Header("Player Settings")]
        [SerializeField] private string defaultPlayerName = "Player";
        [SerializeField] private int defaultPlayerLevel = 1;

        private const string PLAYER_NAME_KEY = "PlayerName";
        private const string PLAYER_LEVEL_KEY = "PlayerLevel";
        private const string EXPERIENCE_KEY = "Experience";
        private const string UNLOCKED_LEVELS_KEY = "UnlockedLevels";

        public string PlayerName { get; set; }
        public int PlayerLevel { get; set; }
        public int Experience { get; set; }
        public int UnlockedLevels { get; set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                LoadData();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void SaveData()
        {
            PlayerPrefs.SetString(PLAYER_NAME_KEY, PlayerName);
            PlayerPrefs.SetInt(PLAYER_LEVEL_KEY, PlayerLevel);
            PlayerPrefs.SetInt(EXPERIENCE_KEY, Experience);
            PlayerPrefs.SetInt(UNLOCKED_LEVELS_KEY, UnlockedLevels);
            PlayerPrefs.Save();
        }

        public void LoadData()
        {
            PlayerName = PlayerPrefs.GetString(PLAYER_NAME_KEY, defaultPlayerName);
            PlayerLevel = PlayerPrefs.GetInt(PLAYER_LEVEL_KEY, defaultPlayerLevel);
            Experience = PlayerPrefs.GetInt(EXPERIENCE_KEY, 0);
            UnlockedLevels = PlayerPrefs.GetInt(UNLOCKED_LEVELS_KEY, 1);
        }

        public void AddExperience(int amount)
        {
            Experience += amount;
            CheckLevelUp();
            SaveData();
        }

        public void UnlockLevel(int levelNumber)
        {
            if (levelNumber > UnlockedLevels)
            {
                UnlockedLevels = levelNumber;
                SaveData();
            }
        }

        private void CheckLevelUp()
        {
            int requiredExp = PlayerLevel * 100;
            if (Experience >= requiredExp)
            {
                PlayerLevel++;
                Experience -= requiredExp;
            }
        }
    }
}