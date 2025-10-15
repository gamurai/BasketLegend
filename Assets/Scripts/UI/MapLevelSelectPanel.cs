using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using BasketLegend.Data;
using BasketLegend.Managers;

namespace BasketLegend.UI
{
    public class MapLevelSelectPanel : MonoBehaviour
    {
        [Header("Map UI References")]
        [SerializeField] private ScrollRect mapScrollView;
        [SerializeField] private RectTransform mapContent;
        [SerializeField] private Button backButton;

        [Header("Prefab References")]
        [SerializeField] private MapLevelButton levelButtonPrefab;
        [SerializeField] private SeasonHeader seasonHeaderPrefab;
        [SerializeField] private PathRenderer pathRendererPrefab;

        [Header("Map Layout Settings")]
        [SerializeField] private float levelSpacing = 150f;
        [SerializeField] private float seasonSpacing = 300f;
        [SerializeField] private float mapWidth = 600f;
        [SerializeField] private int levelsPerRow = 3;

        private List<MapLevelButton> levelButtons = new List<MapLevelButton>();
        private List<SeasonHeader> seasonHeaders = new List<SeasonHeader>();
        private List<PathRenderer> pathRenderers = new List<PathRenderer>();

        private void OnEnable()
        {
            RefreshMap();
            ScrollToCurrentLevel();
        }

        private void RefreshMap()
        {
            ClearMap();
            CreateSeasonMap();
        }

        private void ClearMap()
        {
            foreach (var button in levelButtons)
            {
                if (button != null) DestroyImmediate(button.gameObject);
            }
            levelButtons.Clear();

            foreach (var header in seasonHeaders)
            {
                if (header != null) DestroyImmediate(header.gameObject);
            }
            seasonHeaders.Clear();

            foreach (var path in pathRenderers)
            {
                if (path != null) DestroyImmediate(path.gameObject);
            }
            pathRenderers.Clear();
        }

        private void CreateSeasonMap()
        {
            if (SeasonManager.Instance == null || mapContent == null) return;

            var allSeasons = SeasonManager.Instance.GetAllSeasons();
            float currentY = 0f;

            foreach (var season in allSeasons)
            {
                if (season == null) continue;

                // Create season header
                CreateSeasonHeader(season, currentY);
                currentY -= seasonSpacing * 0.3f;

                // Create levels for this season
                CreateSeasonLevels(season, ref currentY);
                currentY -= seasonSpacing * 0.7f;
            }

            // Update content size
            UpdateContentSize(Mathf.Abs(currentY));
            
            // Create paths between levels
            CreateLevelPaths();
        }

        private void CreateSeasonHeader(SeasonData season, float yPosition)
        {
            if (seasonHeaderPrefab == null) return;

            var headerInstance = Instantiate(seasonHeaderPrefab, mapContent);
            headerInstance.Initialize(season);

            RectTransform headerRect = headerInstance.GetComponent<RectTransform>();
            headerRect.anchoredPosition = new Vector2(0, yPosition);

            seasonHeaders.Add(headerInstance);
        }

        private void CreateSeasonLevels(SeasonData season, ref float currentY)
        {
            if (levelButtonPrefab == null || season.seasonLevels == null) return;

            int levelIndex = 0;
            int row = 0;

            foreach (var level in season.seasonLevels)
            {
                if (level == null) continue;

                // Calculate position in zigzag pattern
                Vector2 position = CalculateLevelPosition(levelIndex, ref currentY, row);
                
                // Create level button
                var buttonInstance = Instantiate(levelButtonPrefab, mapContent);
                buttonInstance.Initialize(level);
                
                RectTransform buttonRect = buttonInstance.GetComponent<RectTransform>();
                buttonRect.anchoredPosition = position;

                // Store position in level data for path creation
                level.mapPosition = position;

                levelButtons.Add(buttonInstance);
                levelIndex++;

                // Move to next row every few levels
                if (levelIndex % levelsPerRow == 0)
                {
                    row++;
                }
            }
        }

        private Vector2 CalculateLevelPosition(int levelIndex, ref float currentY, int row)
        {
            // Calculate column position with zigzag pattern
            int column;
            bool isEvenRow = row % 2 == 0;
            
            if (isEvenRow)
            {
                column = levelIndex % levelsPerRow;
            }
            else
            {
                column = (levelsPerRow - 1) - (levelIndex % levelsPerRow);
            }

            // Calculate X position
            float startX = -(mapWidth * 0.5f) + (mapWidth / (levelsPerRow + 1));
            float x = startX + column * (mapWidth / (levelsPerRow + 1));

            // Calculate Y position
            if (levelIndex % levelsPerRow == 0 && levelIndex > 0)
            {
                currentY -= levelSpacing;
            }

            return new Vector2(x, currentY);
        }

        private void CreateLevelPaths()
        {
            if (pathRendererPrefab == null) return;

            var allSeasons = SeasonManager.Instance.GetAllSeasons();

            foreach (var season in allSeasons)
            {
                if (season?.seasonLevels == null) continue;

                for (int i = 0; i < season.seasonLevels.Count - 1; i++)
                {
                    var currentLevel = season.seasonLevels[i];
                    var nextLevel = season.seasonLevels[i + 1];

                    if (currentLevel != null && nextLevel != null)
                    {
                        CreatePathBetweenLevels(currentLevel, nextLevel);
                    }
                }
            }
        }

        private void CreatePathBetweenLevels(LevelData fromLevel, LevelData toLevel)
        {
            var pathInstance = Instantiate(pathRendererPrefab, mapContent);
            pathInstance.SetPath(fromLevel.mapPosition, toLevel.mapPosition);
            
            // Set path state based on level unlock status
            bool isPathUnlocked = toLevel.isUnlocked || toLevel.starsEarned > 0;
            pathInstance.SetPathState(isPathUnlocked);

            pathRenderers.Add(pathInstance);
        }

        private void UpdateContentSize(float totalHeight)
        {
            if (mapContent != null)
            {
                mapContent.sizeDelta = new Vector2(mapContent.sizeDelta.x, totalHeight + 200f);
            }
        }

        private void ScrollToCurrentLevel()
        {
            if (mapScrollView == null) return;

            var nextLevel = SeasonManager.Instance?.GetNextUnlockedLevel();
            if (nextLevel == null) return;

            // Find the current level button
            var currentLevelButton = levelButtons.Find(btn => btn.GetLevelData()?.levelNumber == nextLevel.levelNumber);
            if (currentLevelButton == null) return;

            // Calculate scroll position
            RectTransform buttonRect = currentLevelButton.GetComponent<RectTransform>();
            float normalizedPosition = Mathf.Clamp01(Mathf.Abs(buttonRect.anchoredPosition.y) / mapContent.sizeDelta.y);
            
            // Scroll to position with smooth animation
            StartCoroutine(SmoothScrollToPosition(1f - normalizedPosition));
        }

        private System.Collections.IEnumerator SmoothScrollToPosition(float targetPosition)
        {
            float duration = 1f;
            float startPosition = mapScrollView.verticalNormalizedPosition;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                t = Mathf.SmoothStep(0f, 1f, t); // Smooth interpolation
                
                mapScrollView.verticalNormalizedPosition = Mathf.Lerp(startPosition, targetPosition, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            mapScrollView.verticalNormalizedPosition = targetPosition;
        }
    }
}