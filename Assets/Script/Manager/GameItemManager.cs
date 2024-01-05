using System;
using System.Collections.Generic;
using System.Linq;
using Script.Manager;
using UnityEngine;

public class GameItemManager : Singleton<GameItemManager>
{
    private Dictionary<int, int> ownItem = new();
    private Dictionary<ITEM_TYPE, int> equippedItem = new();
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
                    var functionType = itemData.function_type;
                    switch (functionType)
                    {
                        case ITEM_FUNCTION_TYPE.ITEM_FUNCTION_TYPE_MOVE_PAGE:
                            //MovePage();
                            break;
                        case ITEM_FUNCTION_TYPE.ITEM_FUNCTION_TYPE_CHAGE_STATUS:
                            ChangeStat(itemData.function_value_1[0], itemData.function_value_2[0] * useCount);
                            break;
                    }
                }
                
                ownItem[itemID] -= useCount;
                if (ownItem[itemID] <= 0)
                {
                    ownItem.Remove(itemID);
                }
            }
            GameDataSaveManager.SaveDataAll();
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
                ownItem[itemID] = Math.Min(ownItem[itemID] + addCount, itemData?.stack_amount ?? 0);
            }
            if(GameUIManager.Instance.TryGetOrCreate<UIToastMsg>(true, UILayer.LEVEL_4,out var ui))
            {
                var rarityInfo = GameDataManager.Instance._rarityData.Find(_ => _.rarity_id == itemData.rarity_id);
                var toastMessageData = GameDataManager.Instance._toastMessageTableData.Find(_ => _.content_type == CONTENT_TYPE.CONTENT_TYPE_ITEM);

                string desc = String.Format(toastMessageData.toast_message_desc, rarityInfo.rarity_string, itemData.item_name);
                GameUIManager.Instance.RegisterSequentialPopup(ui, () => ui.SetUI(CONTENT_TYPE.CONTENT_TYPE_ITEM,  toastMessageData.toast_message_icon,  toastMessageData.toast_message_title, desc));
            }
            GameDataSaveManager.SaveDataAll();
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

            if ( equippedItem.ContainsKey(itemTableData?.item_type ?? ITEM_TYPE.ITEM_TYPE_NORMAL) == true)
            {
                UnEquipItem(itemTableData?.item_type ?? ITEM_TYPE.ITEM_TYPE_NORMAL);
                EquipItem(itemKey);
            }
            else
            {
                equippedItem.Add(itemTableData?.item_type ?? ITEM_TYPE.ITEM_TYPE_NORMAL, itemKey);
                for (int i = 0; i < itemTableData?.function_value_1.Length; ++i)
                {
                    GamePlayerManager.Instance.myActor.playerStat.AddStat(itemTableData.function_value_1[i], itemTableData.function_value_2[i]);
                }
            }
        }
        else
        {
            Debug.Log($"보유한 아이템이 없는데 {itemKey.ToString()} 을 장착하려고함");
        }
    }

    public void UnEquipItem(ITEM_TYPE equipType)
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

    public int? GetEquippedItem(ITEM_TYPE equipType)
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
    
    public override void SaveData(string[] fileName)
    {
        base.SaveData(fileName);
        
        var ownItemData = GameDataSaveManager.ToJson(ownItem);
        var equippedItemData = GameDataSaveManager.ToJson(equippedItem);
        
        GameDataSaveManager.Save(fileName[0], ownItemData);
        GameDataSaveManager.Save(fileName[1], equippedItemData);
    }
    public override void LoadData(string[] fileName)
    {
        base.LoadData(fileName);
        
        string ownItemJson = GameDataSaveManager.Load(fileName[0]);
        string equipItemJson = GameDataSaveManager.Load(fileName[1]);

        ownItem = GameDataSaveManager.FromJson<int, int>(ownItemJson);
        equippedItem = GameDataSaveManager.FromJson<ITEM_TYPE, int>(equipItemJson);
    }
}
