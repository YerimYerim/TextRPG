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
        _scrollRect.MakeList(19);
    }
    
    private void SetUI()
    {

    }
    
    GameObject OnUpdateScrollView(int index)
    {
        var scenarioData = GamePageManager.Instance.GetScenarioData(index);
        var typeEnum = scenarioData.type.to_TemplateType_enum();

        switch (typeEnum)
        {
            case TemplateType.Text:
            {
                var item = _scrollRect.GetItem(index, _textPanel.GameObject());
                var textPanel = item.GetComponent<UIStroyTextPanel>();
                textPanel.SetText(scenarioData.output_txt);
                return item;
            }
            case TemplateType.Image:
            {
                var item = _scrollRect.GetItem(index, _imagePanel.GameObject());
                var imgPanel = item.GetComponent<UIStoryImagePanel>();
                imgPanel.SetImage(scenarioData.relate_value);
                return item;
            }
            case TemplateType.Choice:
            {
                var item = _scrollRect.GetItem(index, _buttonsPanel.GameObject());
                var buttonPanel = item.GetComponent<UIStoryButtonPanel>();
                buttonPanel.SetButton(scenarioData, OnClickButtonAction);
                return item;
            }
            case TemplateType.ItemGet:
            {
                var item = _scrollRect.GetItem(index, _textPanel.GameObject());
                var textPanel = item.GetComponent<UIStroyTextPanel>();
                textPanel.SetText(scenarioData.output_txt);
                return item;
            }
        }

        return null;

    }

    /// <summary>
    /// is_renew_page  == true 일 경우 발동될 이벤트.
    /// </summary>
    void OnEventClear()
    {
        _scrollRect.ClearAll();
    }

    void OnClickButtonAction()
    {
        OnEventClear();
    }
}
