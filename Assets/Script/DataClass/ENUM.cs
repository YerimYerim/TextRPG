using System;

namespace Script.DataClass
{

    public enum PAGE_TYPE
    {
        PAGE_TYPE_TEXT,
        PAGE_TYPE_IMG,
        PAGE_TYPE_BUTTON,
        PAGE_TYPE_GET_ITEM,
        PAGE_TYPE_STATUS,
        PAGE_TYPE_RECURSIVE_GROUP,
        PAGE_TYPE_BATTLE,
    }

    public enum OccurCondition
    {
        OCCUR_CONDITION_NONE,
        OCCUR_CONDITION_OWN_ITEM,           //특정 아이템 보유 시 발생
        OCCUR_CONDITION_NOT_ENOUGH_OWN_ITEM, //특정 아이템 미보유 시 발생
        OCCUR_CONDITION_STATUS_HIGH,        //특정 스탯이 특정값 이상인 경우 발생
        OCCUR_CONDITION_STATUS_LOW,         //특정 스탯이 특정값 이하인 경우 발생
        OCCUR_CONDITION_PAGE_VIEWED,        //특정 페이지를 열람했을 때 발생
    }
    public enum ITEM_FUNCTION_TYPE
    {
        ITEM_FUNCTION_TYPE_CHAGE_STATUS,
        ITEM_FUNCTION_TYPE_MOVE_PAGE,
    }
    public enum ITEM_TYPE
    {
        ITEM_TYPE_NORMAL,
        ITEM_TYPE_EQUIP_WEAPON,
        ITEM_TYPE_EQUIP_HEAD,
        ITEM_TYPE_EQUIP_ARMOR,
        ITEM_TYPE_EQUIP_SHOES,
        ITEM_TYPE_EQUIP_RING,
        ITEM_TYPE_EQUIP_NECKLACE,
    }

    public enum SuccessCondition
    {
        Power,
        MpMax,
    }

    public enum STATUS_FUNCTION_TYPE
    {
        STATUS_FUNCTION_TYPE_MAX_STAT,
        STATUS_FUNCTION_TYPE_GET_STAT,
    }
    
    
    public static class EnumExtensions
    {
        public static PAGE_TYPE to_TemplateType_enum(this string str)
        {
            return str switch
            {
                "text"            => PAGE_TYPE.PAGE_TYPE_TEXT,
                "img"             => PAGE_TYPE.PAGE_TYPE_IMG,
                "button"          => PAGE_TYPE.PAGE_TYPE_BUTTON,
                "get_item"        => PAGE_TYPE.PAGE_TYPE_GET_ITEM,
                "status"          => PAGE_TYPE.PAGE_TYPE_STATUS,
                "recursive_group" => PAGE_TYPE.PAGE_TYPE_RECURSIVE_GROUP,
                "battle"          => PAGE_TYPE.PAGE_TYPE_BATTLE,
                _                 => PAGE_TYPE.PAGE_TYPE_TEXT,
            };
        }
        
        public static OccurCondition to_OccurCondition_enum(this string str)
        {
            return str switch
            {
                "own_item" => OccurCondition.OCCUR_CONDITION_OWN_ITEM,
                "not_enough_own_item" => OccurCondition.OCCUR_CONDITION_NOT_ENOUGH_OWN_ITEM,
                "status_high" => OccurCondition.OCCUR_CONDITION_STATUS_HIGH,
                "status_low" => OccurCondition.OCCUR_CONDITION_STATUS_LOW,
                "page_viewed" => OccurCondition.OCCUR_CONDITION_PAGE_VIEWED,
                _ => OccurCondition.OCCUR_CONDITION_NONE
            };
        }
        
        public static STATUS_FUNCTION_TYPE to_Status_function_type_enum(this string str)
        {
            return str switch
            {
                "max_stat" => STATUS_FUNCTION_TYPE.STATUS_FUNCTION_TYPE_MAX_STAT,
                "get_stat" => STATUS_FUNCTION_TYPE.STATUS_FUNCTION_TYPE_GET_STAT,
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
        public static ITEM_TYPE to_Item_type_enum(this string str)
        {
            return str switch
            {
                "normal"        => ITEM_TYPE.ITEM_TYPE_NORMAL,
                "equip_weapon"  => ITEM_TYPE.ITEM_TYPE_EQUIP_WEAPON,
                "equip_head"    => ITEM_TYPE.ITEM_TYPE_EQUIP_HEAD,
                "equip_armor"   => ITEM_TYPE.ITEM_TYPE_EQUIP_ARMOR,
                "equip_shoes"   => ITEM_TYPE.ITEM_TYPE_EQUIP_SHOES,
                "equip_ring"    => ITEM_TYPE.ITEM_TYPE_EQUIP_RING,
                "equip_necklace"=> ITEM_TYPE.ITEM_TYPE_EQUIP_NECKLACE,
                _ => ITEM_TYPE.ITEM_TYPE_NORMAL
            };
        }
    }
}
