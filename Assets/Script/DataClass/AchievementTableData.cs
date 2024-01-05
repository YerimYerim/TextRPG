using System.Collections.Generic;
public class AchievementTableData
{
    public int? ach_id { get; set; }			//업적 id
    public ACH_TYPE? ach_type { get; set; }			//업적 종류
    public int[] ach_value { get; set; }			//달성 관련 값
    public string[] ach_value_item_type { get; set; }			//달성 관련 값(아이템 타입)
    public int? ach_count { get; set; }			//달성 목표 수량
    public string ach_title { get; set; }			//업적 제목
    public string ach_desc { get; set; }			//업적 설명
    public int? reward_item { get; set; }			//보상 지급할 item_id
    public int? reward_amount { get; set; }			//보상 수량
}
