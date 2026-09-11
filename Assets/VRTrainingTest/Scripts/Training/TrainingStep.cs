using System;

namespace VRTrainingTest.Training
{
    [Serializable]
    // Один пункт сценария: что показать игроку и какое действие ждатб
    public sealed class TrainingStep
    {
        public int Id;
        public string Description;
        public TrainingActionType ExpectedActionType;
        public string TargetId;
        public TrainingStepStatus Status = TrainingStepStatus.Waiting;

        public TrainingStep(int id, string description, TrainingActionType expectedActionType, string targetId)
        {
            Id = id;
            Description = description;
            ExpectedActionType = expectedActionType;
            TargetId = targetId;
        }
    }
}
