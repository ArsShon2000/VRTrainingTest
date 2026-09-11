using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace VRTrainingTest.Training
{
    public sealed class TrainingScenarioView : MonoBehaviour
    {
        [SerializeField] private TMP_Text groupLabel;
        [SerializeField] private TMP_Text stepLabel;
        [SerializeField] private TMP_Text resultsLabel;
        [SerializeField] private GameObject confirmDocumentsButton;
        [SerializeField] private GameObject confirmViolationButton;
        [SerializeField] private GameObject finishTrainingButton;
        [SerializeField] private GameObject restartButton;
        [SerializeField] private GameObject returnLobbyButton;

        public void ShowStep(TrainingStep step, string groupName)
        {
            SetLabelActive(groupLabel, true);
            SetLabelActive(stepLabel, true);
            SetLabelActive(resultsLabel, false);

            if (groupLabel != null)
            {
                groupLabel.text = groupName;
            }

            if (stepLabel != null)
            {
                stepLabel.text = step.Description;
            }

            SetButtonActive(restartButton, false);
            SetButtonActive(returnLobbyButton, false);
            UpdateButtons(step);
        }

        public void ShowCompleted(IReadOnlyList<TrainingStepGroup> groups)
        {
            SetLabelActive(groupLabel, true);
            SetLabelActive(stepLabel, false);
            SetLabelActive(resultsLabel, true);

            if (groupLabel != null)
            {
                groupLabel.text = "Результаты";
            }

            if (resultsLabel != null)
            {
                resultsLabel.text = BuildResultsText(groups);
            }

            SetButtonActive(confirmDocumentsButton, false);
            SetButtonActive(confirmViolationButton, false);
            SetButtonActive(finishTrainingButton, false);
            SetButtonActive(restartButton, true);
            SetButtonActive(returnLobbyButton, true);
        }

        private static string BuildResultsText(IReadOnlyList<TrainingStepGroup> groups)
        {
            var builder = new StringBuilder();

            foreach (var group in groups)
            {
                builder.AppendLine(group.Name);

                foreach (var step in group.Steps)
                {
                    builder.AppendLine($"{step.Id}. {GetStatusText(step.Status)} - {step.Description}");
                }

                builder.AppendLine();
            }

            return builder.ToString();
        }

        private void UpdateButtons(TrainingStep step)
        {
            SetButtonActive(
                confirmDocumentsButton,
                step.ExpectedActionType == TrainingActionType.PressUIButton &&
                step.TargetId == "ConfirmDocumentsButton");

            SetButtonActive(
                confirmViolationButton,
                step.ExpectedActionType == TrainingActionType.PressUIButton &&
                step.TargetId == "ConfirmViolationButton");

            SetButtonActive(
                finishTrainingButton,
                step.ExpectedActionType == TrainingActionType.PressUIButton &&
                step.TargetId == "FinishTrainingButton");
        }

        private static string GetStatusText(TrainingStepStatus status)
        {
            return status switch
            {
                TrainingStepStatus.Completed => "Выполнен",
                TrainingStepStatus.Failed => "Ошибка",
                TrainingStepStatus.Skipped => "Пропущен",
                _ => "Ожидает"
            };
        }

        private static void SetButtonActive(GameObject button, bool isActive)
        {
            if (button != null)
            {
                button.SetActive(isActive);
            }
        }

        private static void SetLabelActive(TMP_Text label, bool isActive)
        {
            if (label != null)
            {
                label.gameObject.SetActive(isActive);
            }
        }
    }
}