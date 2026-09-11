using Unity.XR.CoreUtils;
using UnityEngine;

namespace VRTrainingTest.Training
{
    public sealed class TrainingReachPoint : MonoBehaviour
    {
        [SerializeField] private TrainingScenarioController scenarioController;
        [SerializeField] private string targetId;
        [SerializeField] private float activationDistance = 1.2f;

        private XROrigin xrOrigin;
        private bool isTriggered;

        private void Awake()
        {
            xrOrigin = FindAnyObjectByType<XROrigin>();
        }

        private void Update()
        {
            if (isTriggered || scenarioController == null || xrOrigin == null)
                return;

            var userPosition = xrOrigin.Camera.transform.position;
            var zonePosition = transform.position;

            userPosition.y = 0f;
            zonePosition.y = 0f;

            if (Vector3.Distance(userPosition, zonePosition) > activationDistance)
                return;

            isTriggered = true;
            scenarioController.RegisterAction(new TrainingAction(TrainingActionType.ReachPoint, targetId));
        }
    }
}