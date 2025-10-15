using UnityEngine;
using BasketLegend.Core;

namespace BasketLegend.Core
{
    public class MainMenuState : IGameState
    {
        private IUIManager uiManager;

        public void Enter()
        {
            uiManager = GameObject.FindObjectOfType<UIManager>();
            
            if (uiManager != null)
            {
                uiManager.HideAllPanels();
                uiManager.ShowPanel("MainMenuPanel");
            }
        }

        public void Update()
        {
            // Handle main menu input if needed
        }

        public void Exit()
        {
            if (uiManager != null)
            {
                uiManager.HidePanel("MainMenuPanel");
            }
        }
    }
}