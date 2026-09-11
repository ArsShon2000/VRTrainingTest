using System;
using System.Collections.Generic;

namespace VRTrainingTest.Training
{
    [Serializable]
    // Группа объединяет несколько шагов, которые идут строго по порядку 
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
