using UnityEngine;

namespace Core
{
    /// <summary>
    /// Manages loading scenes and the loading screen
    /// </summary>
    public sealed class GameManager : MonoBehaviour
    {
        [SerializeField] private SceneLoader sceneLoader;
        
        private void Awake()
        {
            Setup();
            SubscribeToEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }

        private void Setup()
        {
            UserDataSaveManager.UpdateSavePath();
            LoadHub();
        }

        private void SubscribeToEvents()
        {
        }

        private void UnsubscribeFromEvents()
        {
        }

        private void LoadHub()
        {
            sceneLoader.LoadScene(Scenes.HUB);
        }
    }
}
