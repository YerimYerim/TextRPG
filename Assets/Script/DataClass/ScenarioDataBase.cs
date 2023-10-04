using System;
using System.ComponentModel;

namespace Script.DataClass
{
    [Serializable]
    public class ScenarioData
    {
        public int? page_id { get; set; }
        public string type { get; set; }
        public string output_txt { get; set; }
        public string relate_value { get; set; }
        public string occur_condition { get; set; }
        public int[] occur_value { get; set; }
        public int? occur_prob { get; set; }
        public int[] result_value { get; set; }
        public int[] result_prob { get; set; }
        public bool? is_renew_page { get; set; }
    }
}

