using System;
using System.Collections.Generic;
using System.Linq;
using Script.DataClass;
using Script.Manager;
using UnityEngine;

public class GameItemManager : Singleton<GameItemManager>
{
    private Dictionary<int, int> ownItem = new();
    private Dictionary<string, int> equippedItem = new();
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
        GamePlayerManager.Instance.myActor.playerStat.AddStat(statID, addStatValue);
    }
    public void AddItem(int itemID, int addCount)
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
    public int GetItem(int itemID)
    {
        if (ownItem.ContainsKey(itemID))
        {
            return ownItem[itemID];
        }
        return 0;
    }

    public int GetItemCountAll()
    {
        return ownItem.Count;
    }

    public (int itemKey, int itemCount) GetItemByIndex(int index)
    {
        return (ownItem.ElementAt(index).Key, ownItem.ElementAt(index).Value);
    }

    public void EquipItem(int itemKey)
    {
        if (ownItem.TryGetValue(itemKey, out var item))
        {
            var itemTableData = GameDataManager.Instance._itemData.Find(_ => _.item_id == itemKey);

            if ( equippedItem.ContainsKey(itemTableData.item_type) == true)
            {
                UnEquipItem(itemTableData.item_type);
                EquipItem(itemKey);
            }
            else
            {
                equippedItem.Add(itemTableData.item_type, itemKey);
            }
        }
        else
        {
            Debug.Log($"보유한 아이템이 없는데 {itemKey.ToString()} 을 장착하려고함");
        }
    }

    public void UnEquipItem(string equipType)
    {
        if (equippedItem.TryGetValue(equipType, out var itemKey))
        {
            equippedItem.Remove(equipType);
        }
        else
        {
            Debug.Log($"item 이 없는데 {equipType} 을 장비 해제하려고함");
        }
    }

    public int? GetEquippedItem(string equipType)
    {
        if (equippedItem.TryGetValue(equipType, out var itemId))
        {
            return itemId;
        }
        else
        {
            return null;
        }
    }
}
