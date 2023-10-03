using System;
using Script.DataClass;
using Script.Manager;
using Script.UI;
using Script.UI.Story;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIStoryView : MonoBehaviour
{
    [SerializeField] private DTScrollView _scrollRect;
    [SerializeField] private UIStoryImagePanel _imagePanel;
    [SerializeField] private UIStoryButtonPanel _buttonsPanel;
    [SerializeField] private UIStroyTextPanel _textPanel;
    
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
            case TemplateType.Text:
            {
                var item = _scrollRect.GetItem( _textPanel.GameObject());
                var textPanel = item.GetComponent<UIStroyTextPanel>();
                textPanel.SetText(scenarioData.output_txt);
                return item;
            }
            case TemplateType.Image:
            {
                var item = _scrollRect.GetItem( _imagePanel.GameObject());
                var imgPanel = item.GetComponent<UIStoryImagePanel>();
                imgPanel.SetImage(scenarioData.relate_value);
                return item;
            }
            case TemplateType.Choice:
            {
                var item = _scrollRect.GetItem(_buttonsPanel.GameObject());
                var buttonPanel = item.GetComponent<UIStoryButtonPanel>();
                buttonPanel.SetButton(scenarioData, ()=>OnClickButtonAction(scenarioData));
                return item;
            }
            case TemplateType.ItemGet:
            {
                var item = _scrollRect.GetItem( _textPanel.GameObject());
                var textPanel = item.GetComponent<UIStroyTextPanel>();
                textPanel.SetText(scenarioData.output_txt);
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
