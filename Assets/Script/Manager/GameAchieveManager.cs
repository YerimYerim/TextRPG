using System.Collections.Generic;
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
    
}
