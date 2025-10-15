using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using BasketLegend.Core;

namespace BasketLegend.Managers
{
    public class SceneController : MonoBehaviour, ISceneManager
    {
        public static SceneController Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void LoadScene(string sceneName, Action onSceneLoaded = null)
        {
            StartCoroutine(LoadSceneCoroutine(sceneName, onSceneLoaded));
        }

        public void LoadSceneAsync(string sceneName, Action<float> onProgress = null, Action onComplete = null)
        {
            StartCoroutine(LoadSceneAsyncCoroutine(sceneName, onProgress, onComplete));
        }

        public string GetCurrentSceneName()
        {
            return SceneManager.GetActiveScene().name;
        }

        private IEnumerator LoadSceneCoroutine(string sceneName, Action onSceneLoaded)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            onSceneLoaded?.Invoke();
        }

        private IEnumerator LoadSceneAsyncCoroutine(string sceneName, Action<float> onProgress, Action onComplete)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

            while (!asyncLoad.isDone)
            {
                float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
                onProgress?.Invoke(progress);
                yield return null;
            }

            onComplete?.Invoke();
        }
    }
}