using System.Collections.Generic;
using System.Linq;
using Script.DataClass;
using UnityEngine;


namespace Script.Manager
{
    public class GamePageManager : Singleton<GamePageManager>
    {
        private Queue<ScenarioData> _curPageData;
        private readonly HashSet<int> _pastReadPageID = new();
        private int curPageDataIndex = 0;
        private int curPageDataID = 0;
        public int QueueCount => _curPageData.Count;

        public ScenarioData GetScenarioData(int index)
        {
            if (index < GameDataManager.Instance._pageData.Count)
            {
                curPageDataIndex = index;
                curPageDataID = GameDataManager.Instance._pageData[index].page_id ?? 0;
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
                _curPageData.Enqueue(data);
            }
        }

        public ScenarioData DequeueCurPageData()
        {
            var returnData = _curPageData.Dequeue();
            if (_curPageData.Count <= 1)
            {
                var nextPageID = GetNextPageID(returnData);
                EnqueueCurPageData(nextPageID);
                return returnData;
            }
            return _curPageData.Dequeue();
        }

        public int GetNextPageID(ScenarioData scenarioData)
        {
            if (scenarioData.type.to_TemplateType_enum() == TemplateType.Choice)
            {
                return RandProb(scenarioData.result_prob, scenarioData.result_value);
            }

            return GetScenarioData(curPageDataIndex + 1).page_id ?? 0;
        }

        private int RandProb(int[] prob, int[] probResult)
        {
            if (prob.Length != probResult.Length)
            {
                return 0;
            }

            int sumProb = prob.Sum();
            var result = Random.Range(0, sumProb);
            int index = 0;
            for (; index < prob.Length; ++index)
            {
                if(index >= 1)
                {
                    if (prob[index - 1] < result && result <= prob[index])
                    {
                        break;
                    }
                }
                else
                {
                    if (result <= prob[index])
                    {
                        break;
                    }
                }
            }
            return index;
        }
        
    }
}
