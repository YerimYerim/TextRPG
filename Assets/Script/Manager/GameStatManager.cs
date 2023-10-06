using System;
using System.Collections.Generic;
using System.Linq;
using Script.DataClass;
using Script.Manager;

public class GameStatManager : Singleton<GameStatManager>
{
    private Dictionary<StatusTableData, int> status = new();

    public void AddStat(int statusID, int addValue)
    {
        var statusTableData = GameDataManager.Instance._statusData.FirstOrDefault(_ => _.status_id == statusID);
        if (statusTableData != null)
        {
            if ( status.ContainsKey(statusTableData))
            {
                var functionType = statusTableData.function_type.to_Status_function_type_enum();
                if (functionType == STATUS_FUNCTION_TYPE.STATUS_FUNCTION_TYPE_MAX_STAT)
                {
                    var statusMaxData = GameDataManager.Instance._statusData.FirstOrDefault(_ => _.status_id == statusTableData.function_value[0]);
                    if (statusMaxData != null)
                    {
                        status[statusTableData] = Math.Min(status[statusTableData] + addValue, statusMaxData.stack_amount);
                    }
                }
                else
                {
                    status[statusTableData] += addValue;
                }
            }
            else
            {
                status.Add(statusTableData, addValue);
            }
        }
    }

    public (StatusTableData statusTableData, int value) GetStat(int statusID)
    {
        var statusTableData = GameDataManager.Instance._statusData.Find(_ => _.status_id == statusID);
        
        if (status.TryGetValue(statusTableData, out var statusValue))
        {
            return (statusTableData, statusValue);
        }
        else
        {
            status.Add(statusTableData, 0);
            return (statusTableData, 0);
        }
    }
}
