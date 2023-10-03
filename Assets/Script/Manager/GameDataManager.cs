using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Script.DataClass;

public class GameDataManager : Script.Manager.Singleton<GameDataManager>
{
    private readonly string[] _pageJsonFileNames = {"page"};
    internal List<ScenarioData> _pageData = new();

    public void LoadData()
    {
        _pageData = ReadJsonFiles<ScenarioData>(_pageJsonFileNames[0]);
    }

    private static List<T> ReadJsonFiles<T>(string fileName)
    {
        List<T> dataList = new List<T>();
        string filePath = $"Assets/Resource/Json/{fileName}.json";

        if (File.Exists(filePath))
        {
            string jsonContent = File.ReadAllText(filePath);
            List<T> data = JsonConvert.DeserializeObject<List<T>>(jsonContent);

            if (data != null)
            {
                dataList.AddRange(data);
            }
        }
        return dataList;
    }
}
