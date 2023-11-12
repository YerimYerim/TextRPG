using System.Collections.Generic;using UnityEngine;

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
    public int? result_count { get; set; }
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
    public bool is_reset_able { get; set; }
    public int stack_amount { get; set; }
    public bool is_ui_show { get; set; }
    public string function_type { get; set; }
    public List<int> function_value { get; set; }
    public List<int> function_value_1 { get; set; }
    public List<int> function_value_2 { get; set; }
}

public partial class ConfigTableData
{
    public string config_id { get; set; }
    public string data_type { get; set; }
    public object value { get; set; }
}

public class ActionGroupTableData
{
    public int? action_group_id { get; set; }
    public int? action_id { get; set; }
    public string action_type { get; set; }
    public string occur_condition { get; set; }
    public List<int> occur_value { get; set; }
    public int? occur_prob { get; set; }
    public string warning_text { get; set; }
    public List<int> action_value { get; set; }
    public string additional_selection { get; set; }
    public string additional_selection_condition { get; set; }
    public List<int> additional_selection_value { get; set; }
    public string additional_selection_result { get; set; }
    public string result_text_success { get; set; }
    public string result_text_fail { get; set; }
    public List<string> result_text_additional { get; set; }
    public bool? is_unique_action { get; set; }
}

public class MonsterTableData
{
    public int? monster_id { get; set; }
    public string monster_name { get; set; }
    public int? monster_level { get; set; }
    public string monter_img { get; set; }
    public string occur_text { get; set; }
    public int? status_damage_factor { get; set; }
    public int? status_reduce_defense_factor { get; set; }
    public int? status_hp { get; set; }
    public int? status_defense_factor { get; set; }
    public int? status_dodge_factor { get; set; }
    public int? status_hit_factor { get; set; }
    public List<int> action_group { get; set; }
    public List<int> phaze_hp_condition { get; set; }
    public List<int> status_buff_phaze_changed { get; set; }
    public List<int> status_buff_phaze_changed_value { get; set; }
}

public class MonsterActionGroup
{
    public int? action_group_id { get; set; }
    public int? action_id { get; set; }
    public string action_type { get; set; }
    public string occur_condition { get; set; }
    public List<int> occur_value { get; set; }
    public int? occur_prob { get; set; }
    public string warning_text { get; set; }
    public List<int> action_value { get; set; }
    public string additional_selection { get; set; }
    public string additional_selection_condition { get; set; }
    public List<int> additional_selection_value { get; set; }
    public string additional_selection_result { get; set; }
    public string result_text { get; set; }
    public bool? text_always { get; set; }
    public string result_text_additional { get; set; }
    public bool? text_always_additional { get; set; }
    public bool? is_unique_action { get; set; }
}

public class PlayerActionGroup
{
    public string action_type { get; set; }
    public string result_text { get; set; }
    public bool text_priority { get; set; }
}