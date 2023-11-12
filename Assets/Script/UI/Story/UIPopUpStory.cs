using System;
using System.Collections.Generic;
using Lofle.Tween;
using Script.DataClass;
using Script.Manager;
using Script.UI;
using Script.UI.Story;
using Unity.VisualScripting;
using UnityEngine;

public class UIPopUpStory : UIBase
{
    [SerializeField] private DTScrollView _scrollRect;
    [SerializeField] private UIStoryImagePanel _imagePanel;
    [SerializeField] private UIStoryButtonPanel _buttonsPanel;
    [SerializeField] private UIStroyTextPanel _textPanel;
    [SerializeField] private float _tweenTime;
    [SerializeField] private DTButton _indicator;
    [SerializeField] private float indicatorOffset;
    [SerializeField] private float scrollFinishOffset;
    [SerializeField] private Tween _indicatorTween;
    ScenarioData _scenarioData;
    private List<LTDescr> leantweenList = new List<LTDescr>();
    private void Awake()
    {
        GameDataManager.Instance.LoadData();
        _scrollRect.InitScrollView(OnUpdateScrollView, _imagePanel.GameObject(), _buttonsPanel.gameObject, _textPanel.gameObject );
        var scenarioData = GamePageManager.Instance.GetScenarioData(0);
        GamePageManager.Instance.EnqueueCurPageData(scenarioData.page_id ?? 0);
        _scrollRect.MakeList( GamePageManager.Instance.QueueCount);
        _indicator.onClick.AddListener(OnClickIndicator);
    }

    private void OnClickIndicator()
    {
        _scrollRect.MoveScrollEndVertical();
        _indicator.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _scrollRect.onValueChanged.AddListener(OnUpdateScrollEvent);
    }

    private void OnUpdateScrollEvent(Vector2 value)
    { 
        var isFinishScroll = _scrollRect.IsFinishScroll(scrollFinishOffset);
        if (isFinishScroll)
        {
            _indicator.gameObject.SetActive(false);
        }
        else
        {
            _indicator.gameObject.SetActive(_scrollRect.IsOverViewportVertical(indicatorOffset));
            if (_indicator.IsActive())
            {
                _indicatorTween.Play(true);
            }
        }
    }

    private void SetUI()
    {
        _scrollRect.AddList(GamePageManager.Instance.QueueCount);
    }
    
    GameObject OnUpdateScrollView(int index)
    {
        _indicator.gameObject.SetActive(_scrollRect.IsOverViewportVertical(indicatorOffset));
        if (_indicator.IsActive())
        {
            _indicatorTween.Play(true);
        }
        _scenarioData = GamePageManager.Instance.DequeueCurPageData();
        var typeEnum = _scenarioData.type.to_TemplateType_enum();
        if (_scenarioData.is_renew_page == true)
        {
            _scrollRect.ClearAll();
        }
        switch (typeEnum)
        {
            case PAGE_TYPE.PAGE_TYPE_TEXT:
            {
                var item = _scrollRect.GetItem( _textPanel.GameObject());
                var textPanel = item.GetComponent<UIStroyTextPanel>();
                textPanel.SetText(_scenarioData.output_txt);
                leantweenList.Add(LeanTween.alphaCanvas( textPanel._canvas, 1, _tweenTime).setDelay(index).setEase(LeanTweenType.animationCurve).setLoopOnce());
                return item;
            }
            case PAGE_TYPE.PAGE_TYPE_IMG:
            {
                var item = _scrollRect.GetItem( _imagePanel.GameObject());
                var imgPanel = item.GetComponent<UIStoryImagePanel>();
                imgPanel.SetImage(_scenarioData.relate_value);
                leantweenList.Add(LeanTween.alphaCanvas( imgPanel._canvas, 1, _tweenTime).setDelay(index).setEase(LeanTweenType.animationCurve).setLoopOnce());
                return item;
            }
            case PAGE_TYPE.PAGE_TYPE_BUTTON:
            {
                var item = _scrollRect.GetItem(_buttonsPanel.GameObject());
                var buttonPanel = item.GetComponent<UIStoryButtonPanel>();
                buttonPanel.SetButton(_scenarioData, ()=>
                {
                    GamePageManager.Instance.NextDataEnqueue(_scenarioData);
                    OnClickButtonAction(_scenarioData);
                });
                leantweenList.Add(LeanTween.alphaCanvas( buttonPanel._canvas, 1, _tweenTime).setDelay(index).setEase(LeanTweenType.animationCurve).setLoopOnce());
                return item;
            }
            case PAGE_TYPE.PAGE_TYPE_GET_ITEM:
            {
                var item = _scrollRect.GetItem( _textPanel.GameObject());
                var textPanel = item.GetComponent<UIStroyTextPanel>();
                GameItemManager.Instance.AddItem(_scenarioData.result_value[0], _scenarioData.result_value[1]);
                textPanel.SetText(_scenarioData.output_txt);
                leantweenList.Add(LeanTween.alphaCanvas( textPanel._canvas, 1, _tweenTime).setDelay(index).setEase(LeanTweenType.animationCurve).setLoopOnce());
                return item;
            }
            case PAGE_TYPE.PAGE_TYPE_STATUS:
            {
                GamePlayerManager.Instance.myActor.playerStat.AddStat(_scenarioData.result_value[0], _scenarioData.result_value[1]);
                var stat = GamePlayerManager.Instance.myActor.playerStat.GetStat(_scenarioData.result_value[0]);
                var statusData = GamePlayerManager.Instance.myActor.playerStat.GetStatusData(_scenarioData.result_value[0]);
                
                var item = _scrollRect.GetItem( _textPanel.GameObject());
                var textPanel = item.GetComponent<UIStroyTextPanel>();
                var statValue = Math.Abs(stat);
                
                // ?? 예림 : string 대응시 변경 해야할 부분 
                var doString = stat < 0 ? "소모" : "획득";
                var str = string.Format(_scenarioData.output_txt, statusData.status_name, statValue, doString);
                
                textPanel.SetText(str);
                leantweenList.Add(LeanTween.alphaCanvas( textPanel._canvas, 1, _tweenTime).setDelay(index).setEase(LeanTweenType.animationCurve).setLoopOnce());
                return item;
            }
            case PAGE_TYPE.PAGE_TYPE_RECURSIVE_GROUP:
            {
                var item = _scrollRect.GetItem( _textPanel.GameObject());
                var textPanel = item.GetComponent<UIStroyTextPanel>();
                textPanel.SetText(_scenarioData.output_txt);
                GamePageManager.Instance.NextDataEnqueue(_scenarioData);
                SetUI();
                leantweenList.Add(LeanTween.alphaCanvas( textPanel._canvas, 1, _tweenTime).setDelay(index).setEase(LeanTweenType.animationCurve).setLoopOnce());
                return item; 
            }
            case PAGE_TYPE.PAGE_TYPE_BATTLE:
            {
                var item = _scrollRect.GetItem(_buttonsPanel.GameObject());
                var buttonPanel = item.GetComponent<UIStoryButtonPanel>();
                buttonPanel.SetButton(_scenarioData, ()=>OnClickBattleButton(_scenarioData));
                leantweenList.Add(LeanTween.alphaCanvas( buttonPanel._canvas, 1, _tweenTime).setDelay(index).setEase(LeanTweenType.animationCurve).setLoopOnce());
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

    void OnClickBattleButton(ScenarioData scenarioData)
    {
        if (GameUIManager.Instance.TryGetOrCreate<UIPopupBattle>(false, UILayer.LEVEL_1, out var ui))
        {
            ui.Show();
            ui.InitMonsterData(scenarioData.result_value[0]);
            this.Hide();
        }
    }
}
