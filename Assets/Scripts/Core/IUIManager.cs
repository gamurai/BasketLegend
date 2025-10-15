using UnityEngine;

namespace BasketLegend.Core
{
    public interface IUIManager
    {
        void ShowPanel(string panelName);
        void HidePanel(string panelName);
        void HideAllPanels();
        bool IsPanelVisible(string panelName);
    }
}