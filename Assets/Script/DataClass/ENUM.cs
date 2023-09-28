using System;

namespace Script.DataClass
{

    public enum TemplateType
    {
        Text,
        Image,
        Choice,
        ItemGet,
    }

    public enum OccurCondition
    {
        PowerOver,
        ClearID,
        ItemOwn,
    }

    public enum SuccessCondition
    {
        Power,
        MpMax,
    }
    
    public static class EnumExtensions
    {
        public static string to_String(this TemplateType templateType)
        {
            return templateType switch
            {
                TemplateType.Text => "텍스트",
                TemplateType.Image => "이미지",
                TemplateType.Choice => "선택지",
                TemplateType.ItemGet => "아이템획득",
                _ => String.Empty,
            };
        }

        public static TemplateType to_TemplateType_enum(this string str)
        {
            return str switch
            {
                "텍스트"    => TemplateType.Choice,
                "이미지"    => TemplateType.Image,
                "선택지"    => TemplateType.Choice,
                "아이템획득" => TemplateType.ItemGet,
                _ => throw new ArgumentOutOfRangeException(nameof(str), str, null)
            };
        }
    }
}
