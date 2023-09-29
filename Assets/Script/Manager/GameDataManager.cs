using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Script.DataClass;

public class GameDataManager : Script.Manager.Singleton<GameDataManager>
{
    private readonly string[] _pageDatajsonFileName = {"page"};
    private readonly List<ScenarioData> _scenarioDatas = new();
    private readonly Queue<ScenarioData> _curPageData = new();
    private HashSet<int> _pastReadPageID = new(); 
    public void LoadScenarioData(int start, int end)
    {
        _scenarioDatas.Clear();
        for(int i = 0; i< end; ++i)
        {
            string jsonContent = File.ReadAllText($"Assets/Resource/Json/{_pageDatajsonFileName[start]}.json");
            List<ScenarioData> scenarioDataList = JsonConvert.DeserializeObject<List<ScenarioData>>(jsonContent);

            if (scenarioDataList != null)
            {
                _scenarioDatas.AddRange(scenarioDataList);
            }
        }
    }

    /// <summary>
    /// data 가져옴
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public ScenarioData GetScenarioData(int index)
    {
        if (index < _scenarioDatas.Count)
        {
            return _scenarioDatas[index];
        }
        return null;
    }

    public void EnqueueCurPageData(int pageID)
    {
        _pastReadPageID.Add(pageID);
        var scenarioData = _scenarioDatas.FindAll(_=>_.page_id == pageID);
        foreach (var data in scenarioData)
        {
            _curPageData.Enqueue(data);
        }
    }

    public ScenarioData DequeueCurPageData()
    {
        return _curPageData.Dequeue();
    }
}
