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
        OCCUR_CONDITION_OWN_ITEM,           //특정 아이템 보유 시 발생
        OCCUR_CONDITION_STATUS_HIGH,        //특정 스탯이 특정값 이상인 경우 발생
        OCCUR_CONDITION_STATUS_LOW,         //특정 스탯이 특정값 이하인 경우 발생
        OCCUR_CONDITION_PAGE_VIEWED,        //특정 페이지를 열람했을 때 발생
    }
    public enum ITEM_FUNCTION_TYPE
    {
        ITEM_FUNCTION_TYPE_CHAGE_STATUS,
        ITEM_FUNCTION_TYPE_MOVE_PAGE,
    }

    public enum SuccessCondition
    {
        Power,
        MpMax,
    }

    public enum STATUS_FUNCTION_TYPE
    {
        STATUS_FUNCTION_TYPE_MAX_STAT,

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
                "status" => TemplateType.Status,
                _ => TemplateType.Choice,
            };
        }
        
        public static OccurCondition to_OccurCondition_enum(this string str)
        {
            return str switch
            {
                "own_item" => OccurCondition.OCCUR_CONDITION_OWN_ITEM,
                "status_high" => OccurCondition.OCCUR_CONDITION_STATUS_HIGH,
                "status_low" => OccurCondition.OCCUR_CONDITION_STATUS_LOW,
                "page_viewed" => OccurCondition.OCCUR_CONDITION_PAGE_VIEWED,
                _ => OccurCondition.OCCUR_CONDITION_PAGE_VIEWED,
            };
        }
        
        public static string to_string(this STATUS_FUNCTION_TYPE statusFunction)
        {
            return statusFunction switch
            {
                STATUS_FUNCTION_TYPE.STATUS_FUNCTION_TYPE_MAX_STAT => "max_stat",
                _ => string.Empty
            };
        }
        public static STATUS_FUNCTION_TYPE to_Status_function_type_enum(this string str)
        {
            return str switch
            {
                "max_stat" => STATUS_FUNCTION_TYPE.STATUS_FUNCTION_TYPE_MAX_STAT,
                _ =>  STATUS_FUNCTION_TYPE.STATUS_FUNCTION_TYPE_MAX_STAT,
            };
        }
        
        public static ITEM_FUNCTION_TYPE to_Item_function_type_enum(this string str)
        {
            return str switch
            {
                "chage_status" => ITEM_FUNCTION_TYPE.ITEM_FUNCTION_TYPE_CHAGE_STATUS,
                "move_page" =>  ITEM_FUNCTION_TYPE.ITEM_FUNCTION_TYPE_MOVE_PAGE,
                _=> ITEM_FUNCTION_TYPE.ITEM_FUNCTION_TYPE_CHAGE_STATUS,
            };
        }
    }
}
