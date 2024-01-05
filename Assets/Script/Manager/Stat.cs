using System;
using System.Collections.Generic;
using System.Linq;
using Script.Manager;

public class Stat 
{
    private Dictionary<int, int> status = new();
    
    public Stat()
    {
        var statusData = GameDataManager.Instance._statusData;
        foreach (var data in statusData)
        {
            if (data.status_id != null)
            {
                AddStat(data?.status_id ?? 0, 0);
            }
        }
    }

    public void Clear()
    {
        status.Clear();
    }
    public void AddStat(int statusID, int addValue, bool notify = true)
    {
        var statusTableData = GameDataManager.Instance._statusData.FirstOrDefault(_ => _.status_id == statusID);
        if (statusTableData != null)
        {
            if ( status.ContainsKey(statusID))
            {
                var functionType = statusTableData?.function_type;
                switch (functionType)
                {
                    case STATUS_FUNCTION_TYPE.STATUS_FUNCTION_TYPE_MAX_STAT:
                    {
                        var statusMaxData = GameDataManager.Instance._statusData.FirstOrDefault(_ => _.status_id == statusTableData.function_value_1[0]);
                        if (statusMaxData != null)
                        {
                            if (statusMaxData.stack_amount != -1)
                            {
                                status[statusID] = Math.Min(status[statusID] + addValue, statusMaxData?.stack_amount ?? 0);
                            }
                            else
                            {
                                status[statusID] += addValue;
                            }
                        }

                        break;
                    }
                    case STATUS_FUNCTION_TYPE.STATUS_FUNCTION_TYPE_GET_STAT:
                    {
                        for (int i = 0; i < statusTableData.function_value_2.Length; ++i)
                        {
                            status[statusTableData.function_value_1[i]] += statusTableData.function_value_2[i] * addValue;
                            status[statusID] += addValue;
                        }

                        break;
                    }
                    default:
                        status[statusID] += addValue;
                        break;
                }
            }
            else
            {
                status.Add(statusID, addValue);
            }
            if(addValue > 0 && notify == true)
            {
                if (GameUIManager.Instance.TryGetOrCreate<UIToastMsg>(true, UILayer.LEVEL_4, out var ui))
                {
                    var toastMessageData = GameDataManager.Instance._toastMessageTableData.Find(_ => _.content_type == CONTENT_TYPE.CONTENT_TYPE_STATUS);
                    string desc = String.Format(toastMessageData.toast_message_desc, statusTableData.status_name);
                    GameUIManager.Instance.RegisterSequentialPopup(ui, () => ui.SetUI(CONTENT_TYPE.CONTENT_TYPE_STATUS, toastMessageData.toast_message_icon, toastMessageData.toast_message_title, desc));
                }
            }
            GameDataSaveManager.SaveDataAll();
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

    public Dictionary<int, int> GetStatusDataDictionary()
    {
        return status;
    }
    
    public void SetStatusDataDictionary( Dictionary<int, int> statusDic)
    {
        status = statusDic;
    }
}
