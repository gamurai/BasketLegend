using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using BasketLegend.Core;
using BasketLegend.UI;

namespace BasketLegend.Core
{
    public class GameInitializer : MonoBehaviour
    {
        [Header("Loading Settings")]
        [SerializeField] private string gameSceneName = "GameScene";
        [SerializeField] private float minimumLoadTime = 2f;
        [SerializeField] private LoadingScreen loadingScreen;

        private void Start()
        {
            StartCoroutine(InitializeGame());
        }

        private IEnumerator InitializeGame()
        {
            float startTime = Time.time;

            if (loadingScreen != null)
            {
                loadingScreen.ShowLoading();
            }

            yield return StartCoroutine(LoadGameSystems());

            float elapsedTime = Time.time - startTime;
            if (elapsedTime < minimumLoadTime)
            {
                yield return new WaitForSeconds(minimumLoadTime - elapsedTime);
            }

            LoadGameScene();
        }

        private IEnumerator LoadGameSystems()
        {
            if (loadingScreen != null)
            {
                loadingScreen.UpdateProgress(0.2f, "Loading Player Data...");
            }
            yield return new WaitForSeconds(0.3f);

            if (loadingScreen != null)
            {
                loadingScreen.UpdateProgress(0.5f, "Loading Game Settings...");
            }
            yield return new WaitForSeconds(0.3f);

            if (loadingScreen != null)
            {
                loadingScreen.UpdateProgress(0.8f, "Preparing Game...");
            }
            yield return new WaitForSeconds(0.3f);

            if (loadingScreen != null)
            {
                loadingScreen.UpdateProgress(1f, "Ready!");
            }
            yield return new WaitForSeconds(0.2f);
        }

        private void LoadGameScene()
        {
            SceneManager.LoadScene(gameSceneName);
        }
    }
}