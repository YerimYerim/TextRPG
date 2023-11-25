using System;
using System.Collections.Generic;
using Script.UI;
using Unity.VisualScripting;
using UnityEngine;

public class UIPopUpAchievement : UIBase
{
    [SerializeField] private DTButton hideButton;
    [SerializeField] private DTButton hideBGButton;

    [SerializeField] private DTScrollView _scrollView;
    [SerializeField] private UIAchievementItem _achievementItem;

    private List<AchievementData> _tableDatas = new ();

    private class AchievementData : AchievementTableData
    {
        private GameAchieveManager.ACHIEVE_STATE state;
        public AchievementData(AchievementTableData tableData)
        {
            this.ach_id = tableData.ach_id;
            this.ach_type = tableData.ach_type;
            this.ach_value = tableData.ach_value;
            this.ach_value_item_type = tableData.ach_value_item_type;
            this.ach_count = tableData.ach_count;
            this.ach_title = tableData.ach_title;
            this.ach_desc = tableData.ach_desc;
            this.reward_item = tableData.reward_item;
            this.reward_amount = tableData.reward_amount;
        }
        public void SetState()
        {
            var data = GameAchieveManager.Instance.GetAchievementInfo(ach_id ?? 0);
            state = data.state;
        }

        public GameAchieveManager.ACHIEVE_STATE GetState()
        {
            return state;
        }
    }
    
    private void Awake()
    {
        hideButton.onClick.AddListener(Hide);
        hideBGButton.onClick.AddListener(Hide);
        
        _scrollView.InitScrollView(OnUpdateScrollView);
    }

    protected override void OnShow(params object[] param)
    {
        base.OnShow(param);
        SetData();
        _scrollView.MakeList(_tableDatas.Count);
    }

    GameObject OnUpdateScrollView(int index)
    {
        var item = _scrollView.GetItem( _achievementItem.gameObject).GetOrAddComponent<UIAchievementItem>();
        item.SetUI(_tableDatas[index].ach_id ?? 0 , () => { OnClickGetReward(_tableDatas[index].ach_id ?? 0); });
        return item.gameObject;
    }
    
    void SetData()
    {
        _tableDatas.Clear();
        foreach (var data in GameDataManager.Instance._achievementTableData)
        {
            _tableDatas.Add(new AchievementData(data));
        }
        UpdateData();
    }

    private void UpdateData()
    {
        for (int i = 0; i < _tableDatas.Count; ++i)
        {
            _tableDatas[i].SetState();
            
        }
        _tableDatas.Sort((a, b) => a.GetState().CompareTo(b.GetState()));
    }

    private void OnClickGetReward(int id)
    {
        var data = GameAchieveManager.Instance.GetAchievementInfo(id);
        if(data.state == GameAchieveManager.ACHIEVE_STATE.COMPLETE)
        {
            GameAchieveManager.Instance.AddReceived(id);
            UpdateData();
            _scrollView.MakeList(_tableDatas.Count);
        }
        else
        {
            
        }
    }
}
