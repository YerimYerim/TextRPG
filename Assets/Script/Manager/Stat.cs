using System;
using System.Collections.Generic;
using System.Linq;
using Script.DataClass;

public class Stat 
{
    private Dictionary<int, int> status = new();
    
    public Stat()
    {
        var statusData = GameDataManager.Instance._statusData;
        foreach (var data in statusData)
        {
            AddStat(data.status_id, 0);
        }
    }
    public void AddStat(int statusID, int addValue)
    {
        var statusTableData = GameDataManager.Instance._statusData.FirstOrDefault(_ => _.status_id == statusID);
        if (statusTableData != null)
        {
            if ( status.ContainsKey(statusID))
            {
                var functionType = statusTableData.function_type.to_Status_function_type_enum();
                if (functionType == STATUS_FUNCTION_TYPE.STATUS_FUNCTION_TYPE_MAX_STAT)
                {
                    var statusMaxData = GameDataManager.Instance._statusData.FirstOrDefault(_ => _.status_id == statusTableData.function_value[0]);
                    if (statusMaxData != null)
                    {
                        status[statusID] = Math.Min(status[statusID] + addValue, statusMaxData.stack_amount);
                    }
                }
                else
                {
                    status[statusID] += addValue;
                }
            }
            else
            {
                status.Add(statusID, addValue);
            }
        }
    }

    public int GetStat(int statusID)
    {
        if (status.TryGetValue(statusID, out var statusValue))
        {
            return statusValue;
        }
        else
        {
            status.Add(statusID, 0);
            return 0;
        }
    }
    public StatusTableData GetStatusData(int statusID)
    {
        return GameDataManager.Instance._statusData.Find(_ => _.status_id == statusID);
    }
}
