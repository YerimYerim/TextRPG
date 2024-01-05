using System.Collections.Generic;
public class EnumTableData
{
    public string group_name { get; set; }			//Enum 그룹명
    public string enum_name { get; set; }			//Enum 명
    public string source_enum_name { get; set; }			//코드용 enum 취합
    public int? enum_value { get; set; }			//인덱스
}
