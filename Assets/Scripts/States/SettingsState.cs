using UnityEngine;
using BasketLegend.Core;

namespace BasketLegend.Core
{
    public class SettingsState : IGameState
    {
        private IUIManager uiManager;

        public void Enter()
        {
            uiManager = GameObject.FindObjectOfType<UIManager>();
            
            if (uiManager != null)
            {
                uiManager.ShowPanel("SettingsPanel");
            }
        }

        public void Update()
        {
            // Handle settings input if needed
        }

        public void Exit()
        {
            if (uiManager != null)
            {
                uiManager.HidePanel("SettingsPanel");
            }
        }
    }
}