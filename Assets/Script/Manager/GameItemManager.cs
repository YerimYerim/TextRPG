using System;
using System.Collections.Generic;
using System.Linq;
using Script.DataClass;
using Script.Manager;

public class GameItemManager : Singleton<GameItemManager>
{
    private Dictionary<int, int> ownItem = new();
    public void UseItem(int itemID, int useCount)
    {
        var itemData = GameDataManager.Instance._itemData.FirstOrDefault(_ => _.item_id == itemID);
        if(itemData != null)
        {
            if (ownItem.ContainsKey(itemID))
            {
                if (ownItem[itemID] < useCount)
                {
                    // ?? 예림 : 사용할 수 없습니다. - 토스트 메시지 대응?
                }
                else
                {
                    var functionType = itemData.function_type.to_Item_function_type_enum();
                    switch (functionType)
                    {
                        case ITEM_FUNCTION_TYPE.ITEM_FUNCTION_TYPE_MOVE_PAGE:
                            //MovePage();
                            break;
                        case ITEM_FUNCTION_TYPE.ITEM_FUNCTION_TYPE_CHAGE_STATUS:
                            ChangeStat(itemData.function_value_1[0], itemData.function_value_2[0]);
                            break;
                    }
                }
            }
        }
    }

    private void ChangeStat(int statID, int addStatValue)
    {
        GameStatManager.Instance.AddStat(statID, addStatValue);
    }
    public void GetItem(int itemID, int addCount)
    {
        var itemData = GameDataManager.Instance._itemData.FirstOrDefault(_ => _.item_id == itemID);
        if (itemData != null)
        {
            if (!ownItem.ContainsKey(itemID))
            {
                ownItem.Add(itemID, 0);
            }
            
            if (itemData.stack_amount != -1)
            {
                ownItem[itemID] += addCount;
            }
            else
            {
                ownItem[itemID] = Math.Min(ownItem[itemID] + addCount, itemData.stack_amount);
            }
        }
    }
    public int GetItemCount(int itemID)
    {
        if (ownItem.ContainsKey(itemID))
        {
            return ownItem[itemID];
        }
        return 0;
    }
}
