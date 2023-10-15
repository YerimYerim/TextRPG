using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class GameDataManager : Script.Manager.Singleton<GameDataManager>
{
    private readonly string[] _pageJsonFileNames = { "page", "status", "item", "rarity", "config"};
    internal List<ScenarioData> _pageData = new();
    internal List<StatusTableData> _statusData = new();
    internal List<ItemTableData> _itemData = new();
    internal List<RarityTableData> _rarityData = new();
    internal List<ConfigTableData> _configTableData = new();
    public void LoadData()
    {
        _pageData = ReadJsonFiles<ScenarioData>(_pageJsonFileNames[0]);
        _statusData = ReadJsonFiles<StatusTableData>(_pageJsonFileNames[1]);
        _itemData = ReadJsonFiles<ItemTableData>(_pageJsonFileNames[2]);
        _rarityData = ReadJsonFiles<RarityTableData>(_pageJsonFileNames[3]);
        _configTableData = ReadJsonFiles<ConfigTableData>(_pageJsonFileNames[4]);
    }

    private static List<T> ReadJsonFiles<T>(string fileName)
    {
        List<T> dataList = new List<T>();
        var jsonData = Resources.Load<TextAsset>($"Json/{fileName}");
        List<T> data = JsonConvert.DeserializeObject<List<T>>(jsonData.text);

        if (data != null)
        {
            dataList.AddRange(data);
        }

        return dataList;
    }
    
    public object GetValueConfigData(string key)
    {
        var tableData = _configTableData.Find(_ => _.config_id == key);
        switch (tableData.data_type)
        {
            case "string[]":
            {
                return GetStringList(tableData);
            }
            case "int":
            {
                return tableData.value;
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
