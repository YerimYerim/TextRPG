using System;

namespace Script.DataClass
{

    public enum TemplateType
    {
        Text,
        Image,
        Choice,
        ItemGet,
        Status,
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
                TemplateType.Text => "text",
                TemplateType.Image => "img",
                TemplateType.Choice => "button",
                TemplateType.ItemGet => "get_item",
                TemplateType.Status => "status",
                _ => String.Empty,
            };
        }

        public static TemplateType to_TemplateType_enum(this string str)
        {
            return str switch
            {
                "text"    => TemplateType.Text,
                "img"    => TemplateType.Image,
                "button"    => TemplateType.Choice,
                "get_item" => TemplateType.ItemGet,
                "status" => TemplateType.ItemGet,
                _ => TemplateType.Choice,
            };
        }
    }
}
