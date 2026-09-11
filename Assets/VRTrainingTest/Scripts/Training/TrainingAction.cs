namespace VRTrainingTest.Training
{
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