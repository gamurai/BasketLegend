using UnityEngine;
using BasketLegend.Core;
using BasketLegend.Managers;
using BasketLegend.Data;

namespace BasketLegend.Core
{
    public class MainGameController : MonoBehaviour
    {
        [Header("Managers")]
        [SerializeField] private GameStateManager gameStateManager;
        [SerializeField] private UIManager uiManager;
        [SerializeField] private PlayerDataManager playerDataManager;
        [SerializeField] private LevelManager levelManager;
        [SerializeField] private SeasonManager seasonManager;

        private void Start()
        {
            InitializeGame();
        }

        private void InitializeGame()
        {
            if (gameStateManager != null)
            {
                gameStateManager.StartGame();
            }
        }

        public void PlayButtonClicked()
        {
            if (gameStateManager != null)
            {
                gameStateManager.ChangeState("LevelSelect");
            }
        }

        public void SettingsButtonClicked()
        {
            if (gameStateManager != null)
            {
                gameStateManager.ChangeState("Settings");
            }
        }

        public void BackToMainMenuClicked()
        {
            if (gameStateManager != null)
            {
                gameStateManager.ChangeState("MainMenu");
            }
        }

        public void LevelSelected(int levelNumber)
        {
            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.LoadLevel(levelNumber);
            }
        }

        public void ContinueButtonClicked()
        {
            if (SeasonManager.Instance != null)
            {
                var nextLevel = SeasonManager.Instance.GetNextUnlockedLevel();
                if (nextLevel != null)
                {
                    LevelManager.Instance.LoadLevel(nextLevel.levelNumber);
                }
                else
                {
                    // No more levels, go to level select
                    PlayButtonClicked();
                }
            }
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}