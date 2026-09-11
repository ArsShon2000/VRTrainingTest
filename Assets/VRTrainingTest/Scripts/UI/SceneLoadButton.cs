using UnityEngine;
using UnityEngine.SceneManagement;

namespace VRTrainingTest.UI
{
    public sealed class SceneLoadButton : MonoBehaviour
    {
        [SerializeField] private string sceneName;

        // Вызывается с UI-кнопки в лобби и открывает нужную сцену 
        public void LoadConfiguredScene()
        {
            if (string.IsNullOrWhiteSpace(sceneName))
            {
                Debug.LogError("Сцена не сконфигурирована.", this);
                return;
            }

            SceneManager.LoadScene(sceneName);
        }
    }
}
