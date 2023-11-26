using System;
using System.Collections.Generic;
using System.Linq;
using Script.DataClass;
using Script.Manager;
using UnityEngine;

public class GameAchieveManager : Singleton<GameAchieveManager>
{
        
    public enum ACHIEVE_STATE
    {
        COMPLETE,
        INCOMPLETE,
        RECEIVED
    }

    
    private Dictionary<int, int> _achievementCount = new();
    private Dictionary<int, bool> _received = new();

    public (ACHIEVE_STATE state, AchievementTableData data) GetAchievementInfo(int id)
    {
        var data = GameDataManager.Instance._achievementTableData.Find(_ => _.ach_id == id);

        var state = GetAchieveState(id, data);

        return (state, data);
    }

    public ACHIEVE_STATE GetAchieveState(int id, AchievementTableData data)
    {
        ACHIEVE_STATE state = ACHIEVE_STATE.INCOMPLETE;

        if (!_achievementCount.TryGetValue(id, out var achievedCount))
        {
            achievedCount = 0;
        }

        bool isRewardAble = achievedCount >= data.ach_count;

        _received.TryGetValue(id, out var isReceived);

        if (isReceived)
        {
            state = ACHIEVE_STATE.RECEIVED;
        }
        else if (isRewardAble)
        {
            state = ACHIEVE_STATE.COMPLETE;
        }

        return state;
    }

    public void AddReceived(int id)
    {
        if (_received.TryAdd(id, true) == false)
        {
            Debug.Log("isNot");
        }
    }

    public void UpdateAchievementCount()
    {
        var tableData = GameDataManager.Instance._achievementTableData;
        for (int i = 0; i < tableData.Count; ++i)
        {
            var achType = tableData[i].ach_type.to_Ach_type_enum();
            int achievementCount = 0;
            switch (achType)
            {
                case ACH_TYPE.ACH_TYPE_PAGE_VIEW:
                    if (GamePageManager.Instance.IsRead(tableData[i].ach_id ?? 0))
                    {
                        achievementCount = 1;
                    }
                    break;
                case ACH_TYPE.ACH_TYPE_OWN_ITEM:
                    {
                        var ownItemCount = GameItemManager.Instance.GetItemCountAll();
                        for (int j = 0; j < ownItemCount; ++j)
                        {
                            var item = GameItemManager.Instance.GetItemByIndex(j);
                            var itemData = GameDataManager.Instance._itemData.Find(_ => _.item_id == item.itemKey);
                            if (tableData[i].ach_value_item_type.Contains(itemData.item_type))
                            {
                                ++achievementCount;
                            }
                        }
                    }
                    break;
                case ACH_TYPE.ACH_TYPE_KILL_MONSTER:
                    if (tableData[i].ach_value == null || tableData[i].ach_value.Count <= 0)
                    {
                        achievementCount = GamePlayerManager.Instance._killMonsterDic.Values.Sum();
                    }
                    else
                    {
                        for (int j = 0; j < tableData[i].ach_value.Count; ++i)
                        {
                            if (GamePlayerManager.Instance._killMonsterDic.TryGetValue(tableData[i].ach_value[j], out var count))
                            {
                                achievementCount += count;
                            }
                        }
                    }
                    break;
                case ACH_TYPE.ACH_TYPE_DEAD_COUNT:
                {
                    achievementCount = GamePlayerManager.Instance.DeadCount;
                } break;
            }

            if (_achievementCount.TryGetValue(tableData[i]?.ach_id ?? 0, out var achieveCount) == false)
            {
                _achievementCount.TryAdd(tableData[i]?.ach_id ?? 0, achievementCount);
            }
            else
            {
                _achievementCount[tableData[i]?.ach_id ?? 0] = achievementCount;
            }
            
        }
    }
}
