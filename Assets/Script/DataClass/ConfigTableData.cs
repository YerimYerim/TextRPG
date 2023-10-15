using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ConfigTableData
{
    public object GetValueConfigData()
    {
        switch (data_type)
        {
            case "string[]":
            {
                return GetStringList(this);
            }
            case "int":
            {
                return value;
            }
            default:
                return string.Empty;
        }
    }

    private string[] GetStringList(ConfigTableData tableData)
    {
        string values = (string) tableData.value;
        string[] strings = values.Split(", ");
        return strings;
    }
}
