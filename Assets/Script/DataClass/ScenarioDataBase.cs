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
    
    [Serializable]
    public class ScenarioData
    {
        public int plot_id { get; set; }
        public string type { get; set; }
        public string output_txt { get; set; }
        public string relate_value { get; set; }
        public string relate_type { get; set; }
        public int occure_condition { get; set; }
        public string occur_value { get; set; }
        public int occur_prob { get; set; }
        public int result_value { get; set; }
        public List<int> result_prob { get; set; }
        public bool is_renew_page { get; set; }
    }
}

