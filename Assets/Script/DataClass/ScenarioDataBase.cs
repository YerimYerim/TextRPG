using System;
using System.Collections.Generic;

namespace Script.DataClass
{
    [Serializable]
    public class ScenarioDataBase
    {
        public int id;
        public TemplateType templateType;
        public string data;
        public int[] moveId;
        public Dictionary<ExposureCondition, float>[] ExposureProb;
        public float successProbFixed;
        public Dictionary<SuccessCondition, float>[] SuccessProb;
        public bool isDeleteBefore;
    }
    
}

