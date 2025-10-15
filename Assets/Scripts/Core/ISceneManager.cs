using System;

namespace BasketLegend.Core
{
    public interface ISceneManager
    {
        void LoadScene(string sceneName, Action onSceneLoaded = null);
        void LoadSceneAsync(string sceneName, Action<float> onProgress = null, Action onComplete = null);
        string GetCurrentSceneName();
    }
}