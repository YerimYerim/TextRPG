using System.Collections.Generic;
public class ActionGroupPlayerTableData
{
    public ACTION_TYPE? action_type { get; set; }			//행동 타입
    public string button_text { get; set; }			//버튼 텍스트
    public string result_text { get; set; }			//결과에 따른 출력 텍스트
    public bool? text_priority { get; set; }			//텍스트 우선순위 높음
    public bool? default_action { get; set; }			//기본적으로 호출할 행동
}
