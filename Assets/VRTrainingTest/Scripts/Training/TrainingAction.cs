namespace VRTrainingTest.Training
{
    // Короткое сообщение от объекта сценария: что игрок сделал и с чем
    public readonly struct TrainingAction
    {
        public readonly TrainingActionType Type;
        public readonly string TargetId;

        public TrainingAction(TrainingActionType type, string targetId)
        {
            Type = type;
            TargetId = targetId;
        }
    }
}
