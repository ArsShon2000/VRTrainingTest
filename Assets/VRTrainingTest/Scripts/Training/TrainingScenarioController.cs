using System.Collections.Generic;
using UnityEngine;

namespace VRTrainingTest.Training
{
    public sealed class TrainingScenarioController : MonoBehaviour
    {
        [SerializeField] private TrainingScenarioView scenarioView;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip successClip;
        [SerializeField] private AudioClip errorClip;

        private readonly List<TrainingStepGroup> groups = new();
        private int currentGroupIndex;
        private int currentStepIndex;

        private TrainingStep CurrentStep => groups[currentGroupIndex].Steps[currentStepIndex];

        private void Awake()
        {
            // Сценарий собирается в коде, чтобы все шаги были видны в одном месте  
            BuildScenario();
            CreateAudioClipsIfNeeded();
        }

        private void Start()
        {
            ActivateCurrentGroup();
        }

        public void RegisterAction(TrainingAction action)
        {
            if (groups.Count == 0)
                return;

            // Любой интерактивный объект приходит сюда одним общим сообщением
            var currentStep = CurrentStep;

            if (IsExpectedAction(currentStep, action))
            {
                CompleteCurrentStep();
                return;
            }

            if (IsFutureStepAction(action))
            {
                FailCurrentGroupBySequenceBreak(action);
                return;
            }

            FailCurrentStep(action);
        }

        private void BuildScenario()
        {
            groups.Clear();

            // Три группы по три шага
            groups.Add(new TrainingStepGroup(
                "Проверка документов",
                new List<TrainingStep>
                {
                    new(1, "Подойдите к зоне проверки документов.", TrainingActionType.ReachPoint, "DocumentZone"),
                    new(2, "Возьмите документ со стола.", TrainingActionType.GrabObject, "Passport"),
                    new(3, "Нажмите кнопку подтверждения документов.", TrainingActionType.PressUIButton, "ConfirmDocumentsButton")
                }));

            groups.Add(new TrainingStepGroup(
                "Проверка оборудования",
                new List<TrainingStep>
                {
                    new(1, "Подойдите к зоне оборудования.", TrainingActionType.ReachPoint, "EquipmentZone"),
                    new(2, "Кликните на неисправный объект.", TrainingActionType.ClickObject, "BrokenDevice"),
                    new(3, "Нажмите кнопку фиксации нарушения.", TrainingActionType.PressUIButton, "ConfirmViolationButton")
                }));

            groups.Add(new TrainingStepGroup(
                "Завершение смены",
                new List<TrainingStep>
                {
                    new(1, "Подойдите к зоне завершения.", TrainingActionType.ReachPoint, "FinishZone"),
                    new(2, "Возьмите финальный отчёт.", TrainingActionType.GrabObject, "FinalReport"),
                    new(3, "Нажмите кнопку завершения тренировки.", TrainingActionType.PressUIButton, "FinishTrainingButton")
                }));
        }

        private void ActivateCurrentGroup()
        {
            var group = groups[currentGroupIndex];
            var step = CurrentStep;

            scenarioView?.ShowStep(step, group.Name);

            Debug.Log($"[Training] Группа: {group.Name}");
            Debug.Log($"[Training] Текущий шаг: {step.Description}");
        }

        private bool IsExpectedAction(TrainingStep step, TrainingAction action)
        {
            return step.ExpectedActionType == action.Type && step.TargetId == action.TargetId;
        }

        private bool IsFutureStepAction(TrainingAction action)
        {
            var steps = groups[currentGroupIndex].Steps;

            // Если игрок сделал будущий шаг раньше времени, считаем это нарушением порядка 
            for (var index = currentStepIndex + 1; index < steps.Count; index++)
            {
                if (IsExpectedAction(steps[index], action))
                    return true;
            }

            return false;
        }

        private void CompleteCurrentStep()
        {
            PlaySuccessSound();

            CurrentStep.Status = TrainingStepStatus.Completed;
            Debug.Log($"[Training] Шаг выполнен: {CurrentStep.Description}");

            MoveNext();
        }

        private void FailCurrentStep(TrainingAction action)
        {
            PlayErrorSound();

            CurrentStep.Status = TrainingStepStatus.Failed;
            Debug.LogWarning(
                $"[Training] Ошибка шага. Ожидалось: {CurrentStep.ExpectedActionType}/{CurrentStep.TargetId}. " +
                $"Получено: {action.Type}/{action.TargetId}");

            MoveNext();
        }

        private void FailCurrentGroupBySequenceBreak(TrainingAction action)
        {
            PlayErrorSound();

            Debug.LogWarning($"[Training] Нарушен порядок выполнения: {action.Type}/{action.TargetId}");

            CurrentStep.Status = TrainingStepStatus.Failed;

            // Остаток группы уже нельзя пройти честно, поэтому помечаем его как пропущенный 
            var steps = groups[currentGroupIndex].Steps;
            for (var index = currentStepIndex + 1; index < steps.Count; index++)
            {
                steps[index].Status = TrainingStepStatus.Skipped;
            }

            MoveNextGroup();
        }

        private void MoveNext()
        {
            currentStepIndex++;

            if (currentStepIndex >= groups[currentGroupIndex].Steps.Count)
            {
                MoveNextGroup();
                return;
            }

            scenarioView?.ShowStep(CurrentStep, groups[currentGroupIndex].Name);

            Debug.Log($"[Training] Текущий шаг: {CurrentStep.Description}");
        }

        private void MoveNextGroup()
        {
            currentGroupIndex++;
            currentStepIndex = 0;

            if (currentGroupIndex >= groups.Count)
            {
                CompleteScenario();
                return;
            }

            ActivateCurrentGroup();
        }

        private void CompleteScenario()
        {
            scenarioView?.ShowCompleted(groups);

            Debug.Log("[Training] Сценарий завершён.");

            // Дублируем итог в Console, чтобы было удобно проверять без UI
            foreach (var group in groups)
            {
                Debug.Log($"[Training] Итог группы: {group.Name}");

                foreach (var step in group.Steps)
                {
                    Debug.Log($"[Training] Шаг {step.Id}: {step.Status} - {step.Description}");
                }
            }
        }



        private void PlaySuccessSound()
        {
            PlaySound(successClip);
        }

        private void PlayErrorSound()
        {
            PlaySound(errorClip);
        }

        private void PlaySound(AudioClip clip)
        {
            if (audioSource == null || clip == null)
                return;

            audioSource.PlayOneShot(clip);
        }

        private void CreateAudioClipsIfNeeded()
        {
            // Если аудиоклипы не заданы в инспекторе, создаем простые короткие сигналы кодом  
            if (successClip == null)
            {
                successClip = CreateToneClip("Training Success", 880f, 0.12f, 0.35f);
            }

            if (errorClip == null)
            {
                errorClip = CreateToneClip("Training Error", 220f, 0.18f, 0.45f);
            }
        }

        private static AudioClip CreateToneClip(string clipName, float frequency, float duration, float volume)
        {
            const int sampleRate = 44100;
            int sampleCount = Mathf.CeilToInt(sampleRate * duration);
            var samples = new float[sampleCount];

            for (int i = 0; i < sampleCount; i++)
            {
                float time = (float)i / sampleRate;
                samples[i] = Mathf.Sin(2f * Mathf.PI * frequency * time) * volume;
            }

            var clip = AudioClip.Create(clipName, sampleCount, 1, sampleRate, false);
            clip.SetData(samples, 0);
            return clip;
        }
    }
}
