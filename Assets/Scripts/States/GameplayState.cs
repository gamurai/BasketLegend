using UnityEngine;
using BasketLegend.Core;

namespace BasketLegend.Core
{
    public class GameplayState : IGameState
    {
        private IUIManager uiManager;

        public void Enter()
        {
            uiManager = GameObject.FindObjectOfType<UIManager>();
            
            if (uiManager != null)
            {
                uiManager.HideAllPanels();
                uiManager.ShowPanel("GameplayPanel");
            }
        }

        public void Update()
        {
            // Handle gameplay input
        }

        public void Exit()
        {
            if (uiManager != null)
            {
                uiManager.HidePanel("GameplayPanel");
            }
        }
    }
}