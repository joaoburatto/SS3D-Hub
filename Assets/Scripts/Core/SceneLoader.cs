using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public sealed class SceneLoader : MonoBehaviour
    {
        private float _sceneLoadingProgress;
        
        public void LoadScene(Scenes scene)
        {
            switch (scene)
            {
                case Scenes.HUB:
                    StartCoroutine(LoadSceneCoroutine(Scenes.HUB));
                    break;
                case Scenes.MANAGER:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(scene), scene, null);
            }
        }

        private static void TryUnloadScene(Scenes scene)
        {
            if (SceneManager.GetSceneByBuildIndex((int)scene).isLoaded)
            {
                SceneManager.UnloadSceneAsync((int)scene);
            }
        }
        
        private IEnumerator LoadSceneCoroutine(Scenes scene)
        {
            _sceneLoadingProgress = 0;

            if (SceneManager.GetSceneByBuildIndex((int)scene).isLoaded)
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex((int)Scenes.MANAGER));
                SceneManager.UnloadSceneAsync((int)scene);
            }
           
            AsyncOperation operation = (SceneManager.LoadSceneAsync((int)scene, LoadSceneMode.Additive));

            yield return new WaitUntil( () => operation.isDone);
            _sceneLoadingProgress = operation.progress;
            
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex((int)scene));
        }
    }
}