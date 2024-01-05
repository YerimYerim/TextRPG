using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class GameDataManager : Script.Manager.Singleton<GameDataManager>
{
    private readonly string[] _pageJsonFileNames = { "page", "status", "item", "rarity", "config", 
        "monster", "action_group", "action_group_player", "achievement","toast_message" };
    internal List<PageTableData> _pageData = new();
    internal List<StatusTableData> _statusData = new();
    internal List<ItemTableData> _itemData = new();
    internal List<RarityTableData> _rarityData = new();
    internal List<ConfigTableData> _configTableData = new();
    internal List<MonsterTableData> _monsterTableData = new();
    internal List<ActionGroupTableData> _monsterActionTableData = new();
    internal List<ActionGroupPlayerTableData> _playerActionTableData = new();
    internal List<AchievementTableData> _achievementTableData = new();
    internal List<ToastMessageTableData> _toastMessageTableData = new();

    public void LoadData()
    {
        _pageData = ReadJsonFiles<PageTableData>(_pageJsonFileNames[0]);
        _statusData = ReadJsonFiles<StatusTableData>(_pageJsonFileNames[1]);
        _itemData = ReadJsonFiles<ItemTableData>(_pageJsonFileNames[2]);
        _rarityData = ReadJsonFiles<RarityTableData>(_pageJsonFileNames[3]);
        _configTableData = ReadJsonFiles<ConfigTableData>(_pageJsonFileNames[4]);
        _monsterTableData = ReadJsonFiles<MonsterTableData>(_pageJsonFileNames[5]);
        _monsterActionTableData = ReadJsonFiles<ActionGroupTableData>(_pageJsonFileNames[6]);
        _playerActionTableData = ReadJsonFiles<ActionGroupPlayerTableData>(_pageJsonFileNames[7]);
        _achievementTableData = ReadJsonFiles<AchievementTableData>(_pageJsonFileNames[8]);
        _toastMessageTableData = ReadJsonFiles<ToastMessageTableData>(_pageJsonFileNames[9]);
    }

    private static List<T> ReadJsonFiles<T>(string fileName)
    {
        List<T> dataList = new List<T>();
        var jsonData = Resources.Load<TextAsset>($"Json/{fileName}");
        try
        {
            List<T> data = JsonConvert.DeserializeObject<List<T>>(jsonData.text);

            if (data != null)
            {
                dataList.AddRange(data);
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
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
                return (int)long.Parse(tableData.value);
            }
            default:
                return string.Empty;
        }
    }

    private string[] GetStringList(ConfigTableData tableData)
    {
        string values = tableData.value;
        string[] strings = values.Split(", ");
        return strings;
    }
    
    public object GetValueConfigData(ConfigTableData tableData)
    {
        switch (tableData.data_type)
        {
            case "string[]":
            {
                return GetStringList(tableData);
            }
            case "int":
            {
                return (int)long.Parse(tableData.value);
            }
            default:
                return string.Empty;
        }
    }

}
