using System.Collections.Generic;
using System.Linq;
using Script.DataClass;
using UnityEngine;


namespace Script.Manager
{
    public class GamePageManager : Singleton<GamePageManager>
    {
        private Queue<ScenarioData> _curPageData = new();
        private readonly HashSet<int> _pastReadPageID = new();
        public int QueueCount => _curPageData.Count;

        public ScenarioData GetScenarioData(int index)
        {
            if (index < GameDataManager.Instance._pageData.Count)
            {
                return GameDataManager.Instance._pageData[index];
            }
            return null;
        }

        public void EnqueueCurPageData(int pageID)
        {
            _pastReadPageID.Add(pageID);
            var scenarioData = GameDataManager.Instance._pageData.FindAll(_=>_.page_id == pageID);
            foreach (var data in scenarioData)
            {
                if(RandProbSingle(data.occur_prob ?? 100) == true)
                {
                    _curPageData.Enqueue(data);
                }
            }
        }

        public ScenarioData DequeueCurPageData()
        {
            var returnData = _curPageData.Dequeue();
            return returnData;
        }

        public void NextDataEnqueue(ScenarioData returnData)
        {
            var nextPageID = GetNextPageID(returnData);
            EnqueueCurPageData(nextPageID);
        }

        public int GetNextPageID(ScenarioData scenarioData)
        {
            if (scenarioData.type.to_TemplateType_enum() == TemplateType.Choice)
            {
                return RandProb(scenarioData.result_prob, scenarioData.result_value);
            }

            return 0;
        }

        private int RandProb(int[] prob, int[] probResult)
        {
            if (prob.Length != probResult.Length)
            {
                return probResult[0];
            }

            int sumProb = prob.Sum();
            var result = Random.Range(0, sumProb);
            int index = 0;
            for (; index < prob.Length; ++index)
            {
                if(index >= 1)
                {
                    if (prob[index - 1] < result && result <= (prob[index - 1] + prob[index]))
                    {
                        return probResult[index];
                    }
                }
                else
                {
                    if (result <= prob[index])
                    {
                        return probResult[index];
                    }
                }
            }
            return index;
        }

        private bool RandProbSingle(int successProb)
        {
            int sumProb = 100;
            var result = Random.Range(0, sumProb);

            if (result <= successProb)
            {
                return true;
            }
            return false;
        }
    }
}
