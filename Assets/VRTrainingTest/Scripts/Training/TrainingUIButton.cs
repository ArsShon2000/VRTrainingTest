using UnityEngine;

namespace VRTrainingTest.Training
{
    public sealed class TrainingUIButton : MonoBehaviour
    {
        [SerializeField] private TrainingScenarioController scenarioController;
        [SerializeField] private string targetId;

        // Этот метод вешается в OnClick у Unity Button.
        public void Press()
        {
            if (scenarioController == null)
                return;

            scenarioController.RegisterAction(new TrainingAction(TrainingActionType.PressUIButton, targetId));
        }
    }
}
