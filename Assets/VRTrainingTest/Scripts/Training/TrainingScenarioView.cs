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

        [SerializeField] private TrainingHighlight documentZoneHighlight;
        [SerializeField] private TrainingHighlight passportHighlight;
        [SerializeField] private TrainingHighlight confirmDocumentsButtonHighlight;

        [SerializeField] private TrainingHighlight equipmentZoneHighlight;
        [SerializeField] private TrainingHighlight brokenDeviceHighlight;
        [SerializeField] private TrainingHighlight confirmViolationButtonHighlight;

        [SerializeField] private TrainingHighlight finishZoneHighlight;
        [SerializeField] private TrainingHighlight finalReportHighlight;
        [SerializeField] private TrainingHighlight finishTrainingButtonHighlight;

        public void ShowStep(TrainingStep step, string groupName)
        {
            // Каждый новый шаг заново выставляет текст, кнопки и подсветку
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
            UpdateHighlights(step);
        }

        public void ShowCompleted(IReadOnlyList<TrainingStepGroup> groups)
        {
            // На финальном экране шагов уже нет, показываем только итог и кнопки действий
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
            ClearHighlights();
        }

        private static string BuildResultsText(IReadOnlyList<TrainingStepGroup> groups)
        {
            var builder = new StringBuilder();

            // Собираем обычный текст, так его проще вывести в один TMP label
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
            // Показываем только ту кнопку, которая реально нужна на текущем шаге
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

        private void UpdateHighlights(TrainingStep step)
        {
            ClearHighlights();

            // TargetId связывает шаг сценария с конкретным объектом на сцене
            SetHighlight(documentZoneHighlight, step.TargetId == "DocumentZone");
            SetHighlight(passportHighlight, step.TargetId == "Passport");
            SetHighlight(confirmDocumentsButtonHighlight, step.TargetId == "ConfirmDocumentsButton");

            SetHighlight(equipmentZoneHighlight, step.TargetId == "EquipmentZone");
            SetHighlight(brokenDeviceHighlight, step.TargetId == "BrokenDevice");
            SetHighlight(confirmViolationButtonHighlight, step.TargetId == "ConfirmViolationButton");

            SetHighlight(finishZoneHighlight, step.TargetId == "FinishZone");
            SetHighlight(finalReportHighlight, step.TargetId == "FinalReport");
            SetHighlight(finishTrainingButtonHighlight, step.TargetId == "FinishTrainingButton");
        }

        private void ClearHighlights()
        {
            SetHighlight(documentZoneHighlight, false);
            SetHighlight(passportHighlight, false);
            SetHighlight(confirmDocumentsButtonHighlight, false);

            SetHighlight(equipmentZoneHighlight, false);
            SetHighlight(brokenDeviceHighlight, false);
            SetHighlight(confirmViolationButtonHighlight, false);

            SetHighlight(finishZoneHighlight, false);
            SetHighlight(finalReportHighlight, false);
            SetHighlight(finishTrainingButtonHighlight, false);
        }

        private static void SetHighlight(TrainingHighlight highlight, bool isHighlighted)
        {
            if (highlight != null)
            {
                highlight.SetHighlighted(isHighlighted);
            }
        }
    }
}
