using System.Collections.Generic;
using Script.DataClass;

namespace Script.Manager
{
    public class GameScenarioManager : Singleton<GameScenarioManager>
    {
        private Queue<ScenarioData> ScenarioQueue;

        public GameScenarioManager()
        {
            ScenarioQueue = new Queue<ScenarioData>();
        }

        public void EnqueueScenarioData(ScenarioData scenarioDataBase)
        {
            ScenarioQueue.Enqueue(scenarioDataBase);
        }

        public void DequeueScenarioData()
        {
            if (ScenarioQueue.Count > 0)
            {
                ScenarioQueue.Dequeue();
            }
        }


    }
}
