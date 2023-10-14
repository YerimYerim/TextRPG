using System;
using System.Collections.Generic;
using System.Linq;
using Script.DataClass;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;


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

        /// <summary>
        /// 해당  id의 page 를 모두 가져온다
        /// </summary>
        /// <param name="pageID"></param>
        /// <param name="isAddReadPage"></param>
        public void EnqueueCurPageData(int pageID, bool isAddReadPage = true)
        {
            var scenarioData = GameDataManager.Instance._pageData.FindAll(_=>_.page_id == pageID && IsCanOccur(_));
            if (isAddReadPage == true)
            {
                _pastReadPageID.Add(pageID);
            }
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
            if (returnData.result_value.Length <= 0 || returnData.type.to_TemplateType_enum() == PAGE_TYPE.PAGE_TYPE_GET_ITEM ||  returnData.type.to_TemplateType_enum() == PAGE_TYPE.PAGE_TYPE_STATUS )
                return;
            if (returnData.type.to_TemplateType_enum() == PAGE_TYPE.PAGE_TYPE_RECURSIVE_GROUP)
            {
                for (int i = 0; i < returnData.result_count;)
                {
                    var nextPageID = GetNextPageID(returnData);
                    var page = GameDataManager.Instance._pageData.Find(_ => _.page_id == nextPageID);
                    
                    if (IsCanOccur(page))
                    {
                        EnqueueCurPageData(nextPageID, false);
                        ++i;
                    }
                    
                }
            }
            else
            {
                var nextPageID = GetNextPageID(returnData);
                EnqueueCurPageData(nextPageID);
            }
        }

        /// <summary>
        /// 다음  Page ID 를 가져올 수 있음. 
        /// </summary>
        /// <param name="scenarioData"></param>
        /// <returns></returns>
        public int GetNextPageID(ScenarioData scenarioData)
        {
            var pageType = scenarioData.type.to_TemplateType_enum();
            int resultValueCount = scenarioData.result_value.Length;
            
            List<int> prob = new List<int>();
            List<int> result = new List<int>();

            for (int i = 0; i < resultValueCount; ++i)
            {
                var next = GameDataManager.Instance._pageData.Find(_=>_.page_id == scenarioData.result_value[i]);
                if (IsCanOccur(next))
                {
                    if (i < scenarioData.result_prob.Length)
                    {
                        prob.Add(scenarioData.result_prob[i]);
                    }
                    result.Add(scenarioData.result_value[i]);
                }
            }
       
            scenarioData.result_prob = prob.ToArray();
            scenarioData.result_value = result.ToArray();
            if (pageType is PAGE_TYPE.PAGE_TYPE_BUTTON or PAGE_TYPE.PAGE_TYPE_RECURSIVE_GROUP)
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
            
            for (int i = 0; i < prob.Length; ++i)
            {
                int probBefore = 0;
                for(int j = 0; j < i; ++j)
                {
                    probBefore += prob[j];
                }
                var probNext = 0;
                for(int j = 0; j < Math.Min(i + 1 , prob.Length); ++j)
                {
                    probNext += prob[j];
                }

                if (result >= probBefore && result < probNext)
                {
                    return probResult[i];
                }
            }
            
            return 0;
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

        private bool IsCanOccur(ScenarioData scenarioData)
        {
            var occurCondition = scenarioData.occur_condition.to_OccurCondition_enum();
            var occurValue = scenarioData.occur_value;
            switch (occurCondition)
            {
                case OccurCondition.OCCUR_CONDITION_OWN_ITEM:
                    return GameItemManager.Instance.GetItemCount(occurValue[0]) > occurValue[1];
                case OccurCondition.OCCUR_CONDITION_STATUS_HIGH:
                    break;
                case OccurCondition.OCCUR_CONDITION_STATUS_LOW:
                    break;
                case OccurCondition.OCCUR_CONDITION_PAGE_VIEWED:
                    return _pastReadPageID.Contains(occurValue[0]) && IsNotRead(scenarioData);
                case OccurCondition.OCCUR_CONDITION_NOT_ENOUGH_OWN_ITEM:
                    return GameItemManager.Instance.GetItemCount(occurValue[0]) < occurValue[1] && _curPageData.Contains(scenarioData) == false && IsNotRead(scenarioData);
                default: 
                    
                    break;
            }
            return IsNotRead(scenarioData);

        }

        private bool IsNotRead(ScenarioData scenarioData)
        {
            var isCanNextMove = true;

            //모든 result value 가 _pastReadPageID 에 전부 있어야 안나오게
            for (var i = 0; i < scenarioData.result_value.Length; i++)
            {
                var pageid = scenarioData.result_value[i];
                if (_pastReadPageID.Contains(pageid) == false)
                {
                    isCanNextMove = true;
                }
                else
                {
                    if (i == scenarioData.result_value.Length - 1)
                    {
                        isCanNextMove = false;
                    }
                }
            }

            return _pastReadPageID.Contains(scenarioData.page_id ?? 0) == false && isCanNextMove;
        }
    }
}
