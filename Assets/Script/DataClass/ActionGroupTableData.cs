using System.Collections.Generic;
public class ActionGroupTableData
{
    public int? action_group_id { get; set; }			//액션 그룹 ID
    public int? action_id { get; set; }			//각 행동 개별 ID
    public ACTION_TYPE? action_type { get; set; }			//행동 타입
    public OCCUR_CONDITION? occur_condition { get; set; }			//몬스터 행동 출현 조건
    public int[] occur_value { get; set; }			//출현 조건 관련 값
    public int? occur_prob { get; set; }			//해당 항목 발생 확률
    public string warning_text { get; set; }			//행동 전, 전조 텍스트
    public int[] action_value { get; set; }			//action_type에 따른 관련 값
    public string additional_selection { get; set; }			//몬스터 행동에 따른 플레이어 추가 선택지
    public OCCUR_CONDITION? additional_selection_condition { get; set; }			//추가 선택지 등장 조건
    public int[] additional_selection_value { get; set; }			//추가 선택지 조건 관련 값
    public  ACTION_RESULT_TYPE? additional_selection_result { get; set; }			//결과 처리 방식
    public string result_text { get; set; }			//결과에 따른 출력 텍스트
    public bool? text_always { get; set; }			//텍스트 항상 출력 여부
    public string result_text_additional { get; set; }			//결과에 따른 출력 텍스트_추가 선택지
    public bool? text_always_additional { get; set; }			//텍스트 항상 출력 여부
    public bool? is_unique_action { get; set; }			//전투 중, 1회성 출력 처리 여부
}
