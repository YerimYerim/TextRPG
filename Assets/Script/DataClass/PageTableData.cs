using System;
using System.Collections.Generic;
[Serializable]
public class PageTableData
{
    public int? page_id { get; set; }			//페이지 ID
    public PAGE_TYPE? type { get; set; }			//항목의 기능 타입
    public string output_txt { get; set; }			//출력할 텍스트
    public string relate_value { get; set; }			//타입에 따라 받는 값
    public OCCUR_CONDITION? occur_condition { get; set; }			//해당 항목 발생 조건
    public int[] occur_value { get; set; }			//해당 항목 발생 조건에 따른 값
    public int? occur_prob { get; set; }			//해당 항목 발생 확률
    public int[] result_value { get; set; }			//타입에 따라 발생할 결과값
    public int? result_count { get; set; }			//결과에서 선택할 개수 (1개만 선택할 경우 1 입력)
    public int[] result_prob { get; set; }			//타입에 따라 결과가 발생할 확률
    public bool? is_renew_page { get; set; }			//페이지 갱신 여부
}
