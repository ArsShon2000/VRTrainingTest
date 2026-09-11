using System;
using System.Collections.Generic;

namespace VRTrainingTest.Training
{
    [Serializable]
    public sealed class TrainingStepGroup
    {
        public string Name;
        public List<TrainingStep> Steps;

        public TrainingStepGroup(string name, List<TrainingStep> steps)
        {
            Name = name;
            Steps = steps;
        }
    }
}