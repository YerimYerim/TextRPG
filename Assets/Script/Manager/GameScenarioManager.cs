using System.Collections.Generic;
using Script.DataClass;

namespace Script.Manager
{
    public class GameScenarioManager : Singleton<GameScenarioManager>
    {
        private Queue<ScenarioDataBase> ScenarioQueue;

        public GameScenarioManager()
        {
            ScenarioQueue = new Queue<ScenarioDataBase>();
        }

        public void EnqueueScenarioData(ScenarioDataBase scenarioDataBase)
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
