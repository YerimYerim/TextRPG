using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class GameDataManager : Script.Manager.Singleton<GameDataManager>
{
    private readonly string[] _pageJsonFileNames = { "page", "status", "item", "rarity" };
    internal List<ScenarioData> _pageData = new();
    internal List<StatusTableData> _statusData = new();
    internal List<ItemTableData> _itemData = new();
    internal List<RarityTableData> _rarityData = new();
    public void LoadData()
    {
        _pageData = ReadJsonFiles<ScenarioData>(_pageJsonFileNames[0]);
        _statusData = ReadJsonFiles<StatusTableData>(_pageJsonFileNames[1]);
        _itemData = ReadJsonFiles<ItemTableData>(_pageJsonFileNames[2]);
        _rarityData = ReadJsonFiles<RarityTableData>(_pageJsonFileNames[3]);
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
}
