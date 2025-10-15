using UnityEngine;
using BasketLegend.Core;

namespace BasketLegend.Core
{
    public class LevelSelectState : IGameState
    {
        private IUIManager uiManager;

        public void Enter()
        {
            uiManager = GameObject.FindObjectOfType<UIManager>();
            
            if (uiManager != null)
            {
                uiManager.ShowPanel("LevelSelectPanel");
            }
        }

        public void Update()
        {
            // Handle level select input if needed
        }

        public void Exit()
        {
            if (uiManager != null)
            {
                uiManager.HidePanel("LevelSelectPanel");
            }
        }
    }
}