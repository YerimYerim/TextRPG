using System;
using System.Collections.Generic;
using Script.DataClass;
using Script.Manager;
using Script.UI;
using Script.UI.Story;
using Unity.VisualScripting;
using UnityEngine;

public class UIStoryView : MonoBehaviour
{
    [SerializeField] private DTScrollView _scrollRect;
    [SerializeField] private UIStoryImagePanel _imagePanel;
    [SerializeField] private UIStoryButtonPanel _buttonsPanel;
    [SerializeField] private UIStroyTextPanel _textPanel;
    [SerializeField] private float _tweenTime;
    private List<LTDescr> leantweenList = new List<LTDescr>();
    private void Awake()
    {
        GameDataManager.Instance.LoadData();
        _scrollRect.InitScrollView(OnUpdateScrollView, _imagePanel.GameObject(), _buttonsPanel.gameObject, _textPanel.gameObject );
        var scenarioData = GamePageManager.Instance.GetScenarioData(0);
        GamePageManager.Instance.EnqueueCurPageData(scenarioData.page_id ?? 0);
        _scrollRect.MakeList( GamePageManager.Instance.QueueCount);
    }
    
    private void SetUI()
    {
        _scrollRect.AddList(GamePageManager.Instance.QueueCount);
    }
    
    GameObject OnUpdateScrollView(int index)
    {
        var scenarioData = GamePageManager.Instance.DequeueCurPageData();
        var typeEnum = scenarioData.type.to_TemplateType_enum();
        if (scenarioData.is_renew_page == true)
        {
            _scrollRect.ClearAll();
        }
        switch (typeEnum)
        {
            case PAGE_TYPE.PAGE_TYPE_TEXT:
            {
                var item = _scrollRect.GetItem( _textPanel.GameObject());
                var textPanel = item.GetComponent<UIStroyTextPanel>();
                textPanel.SetText(scenarioData.output_txt);
                leantweenList.Add(LeanTween.alphaCanvas( textPanel._canvas, 1, _tweenTime).setDelay(index).setEase(LeanTweenType.animationCurve).setLoopOnce());
                return item;
            }
            case PAGE_TYPE.PAGE_TYPE_IMG:
            {
                var item = _scrollRect.GetItem( _imagePanel.GameObject());
                var imgPanel = item.GetComponent<UIStoryImagePanel>();
                imgPanel.SetImage(scenarioData.relate_value);
                leantweenList.Add(LeanTween.alphaCanvas( imgPanel._canvas, 1, _tweenTime).setDelay(index).setEase(LeanTweenType.animationCurve).setLoopOnce());
                return item;
            }
            case PAGE_TYPE.PAGE_TYPE_BUTTON:
            {
                var item = _scrollRect.GetItem(_buttonsPanel.GameObject());
                var buttonPanel = item.GetComponent<UIStoryButtonPanel>();
                buttonPanel.SetButton(scenarioData, ()=>OnClickButtonAction(scenarioData));
                leantweenList.Add(LeanTween.alphaCanvas( buttonPanel._canvas, 1, _tweenTime).setDelay(index).setEase(LeanTweenType.animationCurve).setLoopOnce());
                return item;
            }
            case PAGE_TYPE.PAGE_TYPE_GET_ITEM:
            {
                var item = _scrollRect.GetItem( _textPanel.GameObject());
                var textPanel = item.GetComponent<UIStroyTextPanel>();
                GameItemManager.Instance.GetItem(scenarioData.result_value[0], scenarioData.result_value[1]);
                textPanel.SetText(scenarioData.output_txt);
                leantweenList.Add(LeanTween.alphaCanvas( textPanel._canvas, 1, _tweenTime).setDelay(index).setEase(LeanTweenType.animationCurve).setLoopOnce());
                return item;
            }
            case PAGE_TYPE.PAGE_TYPE_STATUS:
            {
                GameStatManager.Instance.AddStat(scenarioData.result_value[0], scenarioData.result_value[1]);
                var stat = GameStatManager.Instance.GetStat(scenarioData.result_value[0]);
                
                var item = _scrollRect.GetItem( _textPanel.GameObject());
                var textPanel = item.GetComponent<UIStroyTextPanel>();
                var statValue = Math.Abs(stat);
                var statusData = GameStatManager.Instance.GetStatusData(scenarioData.result_value[0]);
                
                // ?? 예림 : string 대응시 변경 해야할 부분 
                var doString = stat < 0 ? "소모" : "획득";
                var str = string.Format(scenarioData.output_txt, statusData.status_name, statValue, doString);
                
                textPanel.SetText(str);
                leantweenList.Add(LeanTween.alphaCanvas( textPanel._canvas, 1, _tweenTime).setDelay(index).setEase(LeanTweenType.animationCurve).setLoopOnce());
                return item;
            }
            case PAGE_TYPE.PAGE_TYPE_RECURSIVE_GROUP:
            {
                var item = _scrollRect.GetItem( _textPanel.GameObject());
                var textPanel = item.GetComponent<UIStroyTextPanel>();
                textPanel.SetText(scenarioData.output_txt);
                GamePageManager.Instance.NextDataEnqueue(scenarioData);
                SetUI();
                leantweenList.Add(LeanTween.alphaCanvas( textPanel._canvas, 1, _tweenTime).setDelay(index).setEase(LeanTweenType.animationCurve).setLoopOnce());
                return item; 
            }
        }

        return null;
    }

    void OnDeleteButtons()
    {
        var buttonItems = _scrollRect.GetItemsByComponent<UIStoryButtonPanel>();
        foreach (var button in buttonItems)
        {
            button.SetActive(false);
        }
    }
    
    /// <summary>
    /// is_renew_page  == true 일 경우 발동될 이벤트.
    /// </summary>
    void OnEventClear()
    {
        _scrollRect.ClearAll();
        _scrollRect.MakeList(GamePageManager.Instance.QueueCount);
    }
    
    void OnClickButtonAction(ScenarioData scenarioData)
    {
        OnDeleteButtons();
        if (scenarioData.is_renew_page == true)
        {
            OnEventClear();
        }
        else
        {
            SetUI();
        }
    }
}
