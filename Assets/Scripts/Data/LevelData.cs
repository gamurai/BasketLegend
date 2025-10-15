using UnityEngine;

namespace BasketLegend.Data
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "BasketLegend/Level Data")]
    public class LevelData : ScriptableObject
    {
        [Header("Level Information")]
        public int levelNumber;
        public int seasonNumber;
        public string levelName;
        public string sceneName;
        public Sprite levelThumbnail;
        public bool isUnlocked;
        
        [Header("Map Position")]
        public Vector2 mapPosition;
        public LevelType levelType = LevelType.Normal;
        
        [Header("Requirements")]
        public int requiredPlayerLevel;
        public int previousLevelToUnlock;
        
        [Header("Rewards")]
        public int experienceReward;
        public int starsToEarn;
        public int starsEarned;
        
        [TextArea(3, 5)]
        public string levelDescription;
    }

    public enum LevelType
    {
        Normal,
        Boss,
        Bonus,
        Challenge
    }
}