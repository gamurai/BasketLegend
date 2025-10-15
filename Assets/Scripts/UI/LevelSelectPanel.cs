using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using BasketLegend.Data;
using BasketLegend.Managers;

namespace BasketLegend.UI
{
    public class LevelSelectPanel : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Transform levelButtonsParent;
        [SerializeField] private LevelButton levelButtonPrefab;
        [SerializeField] private Button backButton;
        [SerializeField] private ScrollRect scrollRect;

        private List<LevelButton> levelButtons = new List<LevelButton>();

        private void OnEnable()
        {
            RefreshLevelButtons();
        }

        private void RefreshLevelButtons()
        {
            ClearLevelButtons();
            CreateLevelButtons();
        }

        private void ClearLevelButtons()
        {
            foreach (var button in levelButtons)
            {
                if (button != null)
                {
                    DestroyImmediate(button.gameObject);
                }
            }
            levelButtons.Clear();
        }

        private void CreateLevelButtons()
        {
            if (LevelManager.Instance == null || levelButtonPrefab == null || levelButtonsParent == null)
                return;

            var allLevels = LevelManager.Instance.GetAllLevels();

            foreach (var levelData in allLevels)
            {
                var buttonInstance = Instantiate(levelButtonPrefab, levelButtonsParent);
                buttonInstance.Initialize(levelData);
                levelButtons.Add(buttonInstance);
            }
        }
    }
}