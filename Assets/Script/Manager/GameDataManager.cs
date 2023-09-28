using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Script.DataClass;

public class GameDataManager : Script.Manager.Singleton<GameDataManager>
{
    private string[] jsonFileName = {"Team_newbie_sample 1"};
    
    
    private List<ScenarioData> _scenarioDatas = new List<ScenarioData>();
    public void LoadScenarioData(int start, int end)
    {
        for(int i = 0; i< end; ++i)
        {
            string jsonContent = File.ReadAllText($"Assets/Resource/Json/{jsonFileName[start]}.json");
            List<ScenarioData> scenarioDataList = JsonConvert.DeserializeObject<List<ScenarioData>>(jsonContent);

            _scenarioDatas.AddRange(scenarioDataList);
        }
    }
    
}
