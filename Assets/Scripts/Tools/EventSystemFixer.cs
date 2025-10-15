using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

namespace BasketLegend.Tools
{
    public class EventSystemFixer : MonoBehaviour
    {
        [ContextMenu("Fix EventSystem for New Input System")]
        public void FixEventSystem()
        {
            EventSystem eventSystem = FindFirstObjectByType<EventSystem>();
            
            if (eventSystem == null)
            {
                Debug.LogWarning("No EventSystem found in the scene!");
                return;
            }

            StandaloneInputModule oldModule = eventSystem.GetComponent<StandaloneInputModule>();
            if (oldModule != null)
            {
                DestroyImmediate(oldModule);
                Debug.Log("Removed old StandaloneInputModule");
            }

            InputSystemUIInputModule newModule = eventSystem.GetComponent<InputSystemUIInputModule>();
            if (newModule == null)
            {
                eventSystem.gameObject.AddComponent<InputSystemUIInputModule>();
                Debug.Log("Added InputSystemUIInputModule - EventSystem is now compatible with New Input System!");
            }
            else
            {
                Debug.Log("EventSystem already has InputSystemUIInputModule");
            }
        }

        [ContextMenu("Auto Fix All EventSystems in Scene")]
        public void FixAllEventSystems()
        {
            EventSystem[] allEventSystems = FindObjectsByType<EventSystem>(FindObjectsSortMode.None);
            
            foreach (var eventSystem in allEventSystems)
            {
                StandaloneInputModule oldModule = eventSystem.GetComponent<StandaloneInputModule>();
                if (oldModule != null)
                {
                    DestroyImmediate(oldModule);
                }

                InputSystemUIInputModule newModule = eventSystem.GetComponent<InputSystemUIInputModule>();
                if (newModule == null)
                {
                    eventSystem.gameObject.AddComponent<InputSystemUIInputModule>();
                }
            }

            Debug.Log($"Fixed {allEventSystems.Length} EventSystem(s) for New Input System!");
        }
    }
}
