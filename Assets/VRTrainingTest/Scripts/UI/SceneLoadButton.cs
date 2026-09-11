using UnityEngine;
using UnityEngine.SceneManagement;

namespace VRTrainingTest.UI
{
    public sealed class SceneLoadButton : MonoBehaviour
    {
        [SerializeField] private string sceneName;

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