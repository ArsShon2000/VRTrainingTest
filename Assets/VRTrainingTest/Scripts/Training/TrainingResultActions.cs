using UnityEngine;
using UnityEngine.SceneManagement;

namespace VRTrainingTest.Training
{
    public sealed class TrainingResultActions : MonoBehaviour
    {
        public void RestartTraining()
        {
            SceneManager.LoadScene("Training");
        }

        public void ReturnToLobby()
        {
            SceneManager.LoadScene("Lobby");
        }
    }
}