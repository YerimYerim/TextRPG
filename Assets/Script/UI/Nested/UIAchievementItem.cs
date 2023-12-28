using System;
using TMPro;
using UnityEngine;

public class UIAchievementItem : MonoBehaviour
{
    [SerializeField] private Transform imgBgUndo;
    [SerializeField] private TextMeshProUGUI textDescUndo;
    [SerializeField] private Transform imgBGAch;
    [SerializeField] private TextMeshProUGUI textDescAch;
    [SerializeField] private TextMeshProUGUI textTitle;
    private GameAchieveManager.ACHIEVE_STATE _state;
    

    private void ResetUI()
    {
        imgBgUndo.gameObject.SetActive(false);
        textDescUndo.gameObject.SetActive(false);
        imgBGAch.gameObject.SetActive(false);
        textDescAch.gameObject.SetActive(false);
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
                imgBGAch.gameObject.SetActive(true);
                textDescAch.gameObject.SetActive(true);
                break;
            case GameAchieveManager.ACHIEVE_STATE.RECEIVED:
                imgBGAch.gameObject.SetActive(true);
                textDescAch.gameObject.SetActive(true);
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
    }

    private void SetText(AchievementTableData data)
    {
        textTitle.text = data.ach_title;
        textDescAch.text = data.ach_desc;
    }

}
