using System;
using TMPro;
using UnityEngine;

public class UIAchievementItem : MonoBehaviour
{
    [SerializeField] private Transform imgBgUndo;
    [SerializeField] private TextMeshProUGUI textDescUndo;
    [SerializeField] private Transform imgBGHighlight;
    [SerializeField] private Transform imgBGAch;
    [SerializeField] private TextMeshProUGUI textDescAch;
    [SerializeField] private UIItemThumbnail _uiItemThumbnail;
    [SerializeField] private Transform imageRewardDone;
    [SerializeField] private TextMeshProUGUI textTitle;
    [SerializeField] private Transform imageLine;
    private GameAchieveManager.ACHIEVE_STATE _state;
    

    private void ResetUI()
    {
        imgBgUndo.gameObject.SetActive(false);
        textDescUndo.gameObject.SetActive(false);
        imgBGHighlight.gameObject.SetActive(false);
        imgBGAch.gameObject.SetActive(false);
        textDescAch.gameObject.SetActive(false);
        imageRewardDone.gameObject.SetActive(false);
    }
    
    private void SetState()
    {
        ResetUI();
        switch (_state)
        {
            case GameAchieveManager.ACHIEVE_STATE.INCOMPLETE:
                imgBgUndo.gameObject.SetActive(true);
                textDescUndo.gameObject.SetActive(true);
                break;
            case GameAchieveManager.ACHIEVE_STATE.COMPLETE:
                imgBGHighlight.gameObject.SetActive(true);
                imgBGAch.gameObject.SetActive(true);
                textDescAch.gameObject.SetActive(true);
                break;
            case GameAchieveManager.ACHIEVE_STATE.RECEIVED:
                imgBGAch.gameObject.SetActive(true);
                textDescAch.gameObject.SetActive(true);
                imageRewardDone.gameObject.SetActive(true);
                break;
        }
    }

    public void SetUI(int id, Action onClickEvent)
    {
        var info = GameAchieveManager.Instance.GetAchievementInfo(id);
        _state = info.state;
        
        SetState();
        SetText(info.data);
        SetThumbnail(info.data, onClickEvent);
    }

    private void SetThumbnail(AchievementTableData data,Action onClickEvent)
    {
        _uiItemThumbnail.SetItemInfo(data.reward_item ?? 0, true);
        _uiItemThumbnail.SetEquipIcon(true);
        _uiItemThumbnail.SetOnClickEvent(onClickEvent);
    }

    private void SetText(AchievementTableData data)
    {
        textTitle.text = data.ach_title;
        textDescUndo.text = data.ach_desc;
        textDescAch.text = data.ach_desc;
    }

}
