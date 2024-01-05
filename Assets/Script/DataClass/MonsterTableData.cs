using System.Collections.Generic;
public class MonsterTableData
{
    public int? monster_id { get; set; }			//몬스터 ID
    public string monster_name { get; set; }			//인게임에서 표시할 몬스터 이름
    public int? monster_level { get; set; }			//인게임에서 표시할 몬스터 레벨
    public string monter_img { get; set; }			//몬스터 이미지
    public string occur_text { get; set; }			//몬스터 출현 시 출력할 지문
    public int? status_damage_factor { get; set; }			//스탯 - 공격력
    public int? status_reduce_defense_factor { get; set; }			//스탯 - 관통률
    public int? status_hp { get; set; }			//스탯 - 체력
    public int? status_defense_factor { get; set; }			//스탯 - 방어력
    public int? status_dodge_factor { get; set; }			//스탯 - 회피
    public int? status_hit_factor { get; set; }			//스탯 - 명중
    public int[] action_group { get; set; }			//몬스터 행동 그룹. action_group 참조
    public int[] phaze_hp_condition { get; set; }			//페이즈가 넘어가는 체력 기준
    public int[] status_buff_phaze_changed { get; set; }			//페이즈 바뀔 때 스탯 변경점_스탯id
    public int[] status_buff_phaze_changed_value { get; set; }			//페이즈 바뀔 때 스탯 변경점_수치
    public bool? is_collection_check { get; set; }			//도감 표시 구분자
    public string collection_ptr { get; set; }			//도감용 몬스터 이미지(정사이즈)
    public string monster_desc { get; set; }			//도감용 몬스터 설명
}
