using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Script.Manager;
using UnityEngine;

public class GameDataSaveManager : Singleton<GameDataSaveManager>
{
    private static string[] PageManagerFileName = {"a", "b", "c" };
    private static string[] ItemManagerFileName = {"d", "e"};
    private static string[] AchieveManagerFileName = { "f", "g" };
    private static string[] PlayerManagerFileName = { "h", "i", "j","k"};
    
    private static string fileSaveRoot = "";
    
    [Serializable]
    public class DataDictionary<TKey,TValue>
    {
        public TKey Key;
        public TValue Value;
    }

    [Serializable]
    public class JsonDataArray<TKey, TValue>
    {
        public List<DataDictionary<TKey, TValue>> data;
    }
    public static string ToJson<TKey, TValue>(Dictionary<TKey, TValue> jsonDicData, bool pretty = false)
    {
        List<DataDictionary<TKey, TValue>> dataList = new List<DataDictionary<TKey, TValue>>();
        DataDictionary<TKey, TValue> dictionaryData = new DataDictionary<TKey, TValue>();
        if (jsonDicData == null)
        {
            return null;
        }
        foreach (TKey key in jsonDicData.Keys)
        {
            dictionaryData = new DataDictionary<TKey, TValue>
            {
                Key = key,
                Value = jsonDicData[key]
            };
            dataList.Add(dictionaryData);
        }
        var arrayJson = new JsonDataArray<TKey, TValue>
        {
            data = dataList
        };

        return JsonUtility.ToJson(arrayJson, pretty);
    }
    public static string ToJson(int jsonData, bool pretty = false)
    {
        return jsonData.ToString();
    }
    
    public static string ToJson(int[] jsonData, bool pretty = false)
    {
        string json = JsonConvert.SerializeObject(jsonData, new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.Indented,
        });
        return json;
    }
    public static Dictionary<TKey, TValue> FromJson<TKey, TValue>(string jsonData)
    {
        JsonDataArray<TKey, TValue> dataList = JsonUtility.FromJson<JsonDataArray<TKey, TValue>>(jsonData);
        Dictionary<TKey, TValue> returnDictionary = new Dictionary<TKey, TValue>();
        if(dataList != null)
        {
            for (int i = 0; i < dataList.data.Count; i++)
            {
                DataDictionary<TKey, TValue> dictionaryData = dataList.data[i];
                returnDictionary[dictionaryData.Key] = dictionaryData.Value;
            }
        }
        return returnDictionary;
    }
    public static int FromIntJson(string jsonData)
    {
        return jsonData == null ? 0 : int.Parse(jsonData);
    }
    public static List<T> FromIntArrayJson<T>(string jsonData)
    {
        if (jsonData == null)
            return null;
        List<T> data = JsonConvert.DeserializeObject<List<T>>(jsonData);
        return data;
    }
    public static string Load(string fileName)
    {
        return Resources.Load<TextAsset>($"Json/Player/{fileName}")?.text;
    }

    public static void LoadDataAll()
    {
        GamePageManager.Instance.LoadData(PageManagerFileName);
        GameItemManager.Instance.LoadData(ItemManagerFileName);
        GameAchieveManager.Instance.LoadData(AchieveManagerFileName);
        GamePlayerManager.Instance.LoadData(PlayerManagerFileName);
    }
    
    public static void SaveDataAll()
    {
        GamePageManager.Instance.SaveData(PageManagerFileName);
        GameItemManager.Instance.SaveData(ItemManagerFileName);
        GameAchieveManager.Instance.SaveData(AchieveManagerFileName);
        GamePlayerManager.Instance.SaveData(PlayerManagerFileName);
    }

    public static void Save(string FileName, string jsonData)
    {
        string path = $"{Application.dataPath}/Resources/Json/Player/{FileName}.json";
        File.WriteAllText(path, jsonData);
    }
}
