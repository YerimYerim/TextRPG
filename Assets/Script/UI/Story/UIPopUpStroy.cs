using System;
using System.Collections.Generic;
using Lofle.Tween;
using Script.Manager;
using Script.UI;
using Script.UI.Story;
using Unity.VisualScripting;
using UnityEngine;

public class UIPopUpStroy : UIBase
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
    PageTableData _scenarioData;
    private List<LTDescr> leantweenList = new List<LTDescr>();
    private void Awake()
    {
        _scrollRect.InitScrollView(OnUpdateScrollView, _imagePanel.GameObject(), _buttonsPanel.gameObject, _textPanel.gameObject);
        OnEventClear();
        _indicator.onClick.AddListener(OnClickIndicator);
    }

    public override void Show()
    {
        base.Show();
    }

    protected override void OnHide(params object[] param)
    {
        base.OnHide(param);
        _scrollRect.MakeList(0);
        OnEventClear();
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
        var data = GamePageManager.Instance.DequeueCurPageData();
        _scenarioData = data;
        var typeEnum = data.type;
        if (data.is_renew_page == true)
        {
            _scrollRect.ClearAll();
        }
        switch (typeEnum)
        {
            case PAGE_TYPE.PAGE_TYPE_TEXT:
            {
                var item = _scrollRect.GetItem( _textPanel.GameObject()); 
                var textPanel = item.GetComponent<UIStroyTextPanel>();
                textPanel.SetText(data.output_txt);
                textPanel.gameObject.SetActive(true);
                textPanel.SetTween(index,_tweenTime);
                return item;
            }
            case PAGE_TYPE.PAGE_TYPE_IMG:
            {
                var item = _scrollRect.GetItem( _imagePanel.GameObject());
                var imgPanel = item.GetComponent<UIStoryImagePanel>();
                imgPanel.SetImage(data.relate_value);
                imgPanel.gameObject.SetActive(true);
                imgPanel.SetTween(index,_tweenTime);
                return item;
            }
            case PAGE_TYPE.PAGE_TYPE_BUTTON:
            {
                var item = _scrollRect.GetItem(_buttonsPanel.GameObject());
                var buttonPanel = item.GetComponent<UIStoryButtonPanel>();
                buttonPanel.SetButton(data, ()=>
                {
                    GamePageManager.Instance.NextDataEnqueue(data);
                    OnClickButtonAction(data);
                });
                buttonPanel.gameObject.SetActive(true);
                buttonPanel._canvas.alpha = 0;
                buttonPanel.SetTween(index, _tweenTime);
                return item;
            }
            case PAGE_TYPE.PAGE_TYPE_GET_ITEM:
            {
                var item = _scrollRect.GetItem( _textPanel.GameObject());
                var textPanel = item.GetComponent<UIStroyTextPanel>();
                GameItemManager.Instance.AddItem(data.result_value[0], data.result_value[1]);
                textPanel.SetText(_scenarioData.output_txt);
                textPanel._canvas.alpha = 0;
                textPanel.gameObject.SetActive(true);
                textPanel.SetTween(index, _tweenTime);
                return item;
            }
            case PAGE_TYPE.PAGE_TYPE_STATUS:
            {
                GamePlayerManager.Instance.myActor.playerStat.AddStat(data.result_value[0], data.result_value[1]);
                var stat = GamePlayerManager.Instance.myActor.playerStat.GetStat(data.result_value[0]);
                var statusData = GamePlayerManager.Instance.myActor.playerStat.GetStatusData(data.result_value[0]);
                
                var item = _scrollRect.GetItem( _textPanel.GameObject());
                var textPanel = item.GetComponent<UIStroyTextPanel>();
                var statValue = Math.Abs(stat);
                
                // ?? 예림 : string 대응시 변경 해야할 부분 
                var doString = stat < 0 ? "소모" : "획득";
                var str = string.Format(data.output_txt, statusData.status_name, statValue.ToString(), doString);
                
                textPanel.SetText(str);
                textPanel.gameObject.SetActive(true);
                textPanel._canvas.alpha = 0;
                textPanel.SetTween(index,_tweenTime);

                if (GamePlayerManager.Instance.CheckPlayerDead())
                {
                    if (GameUIManager.Instance.TryGetOrCreate<UIPlayerDead>(true, UILayer.LEVEL_4, out var ui))
                    {
                        ui.Show();
                    }
                }
                return item;
            }
            case PAGE_TYPE.PAGE_TYPE_RECURSIVE_GROUP:
            {
                var item = _scrollRect.GetItem( _textPanel.GameObject());
                var textPanel = item.GetComponent<UIStroyTextPanel>();
                textPanel.SetText(data.output_txt);
                GamePageManager.Instance.NextDataEnqueue(data);
                SetUI();
                textPanel.gameObject.SetActive(true);
                textPanel._canvas.alpha = 0;
                textPanel.SetTween(index,_tweenTime);
                return item; 
            }
            case PAGE_TYPE.PAGE_TYPE_BATTLE:
            {
                var item = _scrollRect.GetItem(_buttonsPanel.GameObject());
                var buttonPanel = item.GetComponent<UIStoryButtonPanel>();
                buttonPanel.SetButton(data, ()=>OnClickBattleButton(data));
                buttonPanel._canvas.alpha = 0;
                buttonPanel.gameObject.SetActive(true);
                buttonPanel.SetTween(index,_tweenTime);
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
        _scrollRect.MakeList(GamePageManager.Instance.QueueCount);
    }
    
    void OnClickButtonAction(PageTableData scenarioData)
    {
        OnDeleteButtons();
        GameDataSaveManager.SaveDataAll();
        if (scenarioData?.is_renew_page == true)
        {
            OnEventClear();
        }
        else
        {
            SetUI();
        }
    }

    void OnClickBattleButton(PageTableData scenarioData)
    {
        GameDataSaveManager.SaveDataAll();
        if (GameUIManager.Instance.TryGetOrCreate<UIPopupBattle>(false, UILayer.LEVEL_1, out var ui))
        {
            ui.Show();
            ui.InitMonsterData(scenarioData.result_value[0], scenarioData.result_value[1], _scrollRect.transform);
        }
    }
}
