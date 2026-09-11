using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace VRTrainingTest.Training
{
    [RequireComponent(typeof(XRSimpleInteractable))]
    public sealed class TrainingClickableObject : MonoBehaviour
    {
        [SerializeField] private TrainingScenarioController scenarioController;
        [SerializeField] private string targetId;

        private XRSimpleInteractable interactable;
        private bool isTriggered;

        private void Awake()
        {
            // XRSimpleInteractable дает объекту обычный клик через XR-луч
            interactable = GetComponent<XRSimpleInteractable>();
            interactable.selectEntered.AddListener(OnSelected);
        }

        private void OnDestroy()
        {
            if (interactable != null)
            {
                interactable.selectEntered.RemoveListener(OnSelected);
            }
        }

        private void OnSelected(SelectEnterEventArgs args)
        {
            if (isTriggered || scenarioController == null)
                return;

            // Считаем действие один раз, чтобы повторный клик не ломал шаги
            isTriggered = true;
            scenarioController.RegisterAction(new TrainingAction(TrainingActionType.ClickObject, targetId));
        }
    }
}
