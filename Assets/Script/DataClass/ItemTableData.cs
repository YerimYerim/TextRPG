using System.Collections.Generic;
public class ItemTableData
{
    public int? item_id { get; set; }			//아이템 ID
    public ITEM_TYPE? item_type { get; set; }			//아이템 타입
    public int? rarity_id { get; set; }			//희귀도 ID
    public string item_rsc { get; set; }			//이미지
    public string item_name { get; set; }			//이름
    public string item_desc { get; set; }			//설명
    public bool? is_reset_able { get; set; }			//초기화 여부 (TRUE = 초기화 / FALSE = 안함)
    public int? stack_amount { get; set; }			//최대 스택 개수
    public bool? is_ui_show { get; set; }			//UI 표시 여부 (TRUE = 표시 / FALSE = 숨김 처리)
    public ITEM_FUNCTION_TYPE? function_type { get; set; }			//아이템 기능 타입
    public int[] function_value_1 { get; set; }			//기능 관련값
    public int[] function_value_2 { get; set; }			//기능 관련값2
    public bool? is_collection_check { get; set; }			//도감 표시 구분자
}
