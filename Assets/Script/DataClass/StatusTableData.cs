using System.Collections.Generic;
public class StatusTableData
{
    public int? status_id { get; set; }			//스탯 ID
    public int? rarity_id { get; set; }			//희귀도 ID
    public string status_rsc { get; set; }			//이미지
    public string status_name { get; set; }			//이름
    public string status_desc { get; set; }			//설명
    public bool? is_reset_able { get; set; }			//초기화 여부 (TRUE = 초기화 / FALSE = 안함)
    public int? stack_amount { get; set; }			//최대 스택 개수
    public bool? is_ui_show { get; set; }			//UI 표시 여부 (TRUE = 표시 / FALSE = 숨김 처리)
    public STATUS_FUNCTION_TYPE? function_type { get; set; }			//스탯 기능 타입
    public int[] function_value { get; set; }			//기능 관련값
    public int[] function_value_1 { get; set; }			//기능 관련값
    public int[] function_value_2 { get; set; }			//기능 관련값2
    public bool? is_collection_check { get; set; }			//도감 표시 구분자
    public int? default_status { get; set; }			//기본 스탯
}
