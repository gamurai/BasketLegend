using UnityEngine;
using System.Collections.Generic;

namespace BasketLegend.Data
{
    [CreateAssetMenu(fileName = "SeasonData", menuName = "BasketLegend/Season Data")]
    public class SeasonData : ScriptableObject
    {
        [Header("Season Information")]
        public int seasonNumber;
        public string seasonName;
        public Sprite seasonIcon;
        public Sprite seasonBackground;
        public Color seasonThemeColor = Color.white;
        
        [Header("Season Configuration")]
        public int levelsPerSeason = 20;
        public bool isUnlocked = false;
        
        [Header("Season Rewards")]
        public int seasonCompletionReward;
        public Sprite seasonRewardIcon;
        
        [TextArea(3, 5)]
        public string seasonDescription;
        
        [Header("Levels")]
        public List<LevelData> seasonLevels = new List<LevelData>();

        private void OnValidate()
        {
            // Auto-generate level numbers for the season
            for (int i = 0; i < seasonLevels.Count; i++)
            {
                if (seasonLevels[i] != null)
                {
                    seasonLevels[i].levelNumber = (seasonNumber - 1) * levelsPerSeason + i + 1;
                    seasonLevels[i].seasonNumber = seasonNumber;
                }
            }
        }
    }
}