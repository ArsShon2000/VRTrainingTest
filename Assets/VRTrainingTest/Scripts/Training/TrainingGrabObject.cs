using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace VRTrainingTest.Training
{
    public sealed class TrainingGrabObject : MonoBehaviour
    {
        [SerializeField] private TrainingScenarioController scenarioController;
        [SerializeField] private string targetId;

        private XRBaseInteractable interactable;
        private bool isTriggered;

        private void Awake()
        {
            // Берем базовый interactable, так скрипт работает с разными grab-компонентами XR Toolkit
            interactable = GetComponent<XRBaseInteractable>();

            if (interactable != null)
            {
                interactable.selectEntered.AddListener(OnSelected);
            }
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

            // Для сценария важно именно первое взятие предмета
            isTriggered = true;
            scenarioController.RegisterAction(new TrainingAction(TrainingActionType.GrabObject, targetId));
        }
    }
}
