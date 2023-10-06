using System.Collections.Generic;

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
public partial class RarityTableData
{
    public int rarity_id { get; set; }
    public string rarity_string { get; set; }
    public string rarity_rsc { get; set; }
    public string rarity_color { get; set; }
}
public partial class ItemTableData
{
    public int item_id { get; set; }
    public string item_type { get; set; }
    public int rarity_id { get; set; }
    public string item_rsc { get; set; }
    public string item_name { get; set; }
    public string item_desc { get; set; }
    public bool is_reset_able { get; set; }
    public int stack_amount { get; set; }
    public bool is_ui_show { get; set; }
    public string function_type { get; set; }
    public List<int> function_value_1 { get; set; }
    public List<int> function_value_2 { get; set; }
}

public partial class StatusTableData
{
    public int status_id { get; set; }
    public int rarity_id { get; set; }
    public string status_rsc { get; set; }
    public string status_name { get; set; }
    public string status_desc { get; set; }
    public bool is_ui_show { get; set; }
    public int stack_amount { get; set; }
    public bool ui_show { get; set; }
    public string function_type { get; set; }
    public List<int> function_value { get; set; }
}