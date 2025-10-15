using UnityEngine;
using System.Collections.Generic;
using BasketLegend.Core;

namespace BasketLegend.Core
{
    public class UIManager : MonoBehaviour, IUIManager
    {
        [Header("UI Panels")]
        [SerializeField] private List<UIPanel> uiPanels = new List<UIPanel>();

        private Dictionary<string, GameObject> panelsDictionary;

        private void Awake()
        {
            InitializePanels();
        }

        private void InitializePanels()
        {
            panelsDictionary = new Dictionary<string, GameObject>();
            
            foreach (var panel in uiPanels)
            {
                if (panel.panelObject != null)
                {
                    panelsDictionary.Add(panel.panelName, panel.panelObject);
                    panel.panelObject.SetActive(false);
                }
            }
        }

        public void ShowPanel(string panelName)
        {
            if (panelsDictionary.ContainsKey(panelName))
            {
                panelsDictionary[panelName].SetActive(true);
            }
        }

        public void HidePanel(string panelName)
        {
            if (panelsDictionary.ContainsKey(panelName))
            {
                panelsDictionary[panelName].SetActive(false);
            }
        }

        public void HideAllPanels()
        {
            foreach (var panel in panelsDictionary.Values)
            {
                panel.SetActive(false);
            }
        }

        public bool IsPanelVisible(string panelName)
        {
            if (panelsDictionary.ContainsKey(panelName))
            {
                return panelsDictionary[panelName].activeInHierarchy;
            }
            return false;
        }

        [System.Serializable]
        public class UIPanel
        {
            public string panelName;
            public GameObject panelObject;
        }
    }
}