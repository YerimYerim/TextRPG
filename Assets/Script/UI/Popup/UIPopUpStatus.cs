using System;
using System.Collections.Generic;
using Script.Manager;
using Script.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIPopUpStatus : UIBase
{
    [Header("기본 능력치")]
    [SerializeField] private DTButton hideButton;
    [SerializeField] private DTButton hideBGButton;
    [SerializeField] private GameObject[] _goUIstat;
    private UIStat[] _uiStat;

    [Header("특수 능력치")]
    [SerializeField] private DTScrollView _scrollView;
    [SerializeField] private GameObject _scrollViewObj;
    [SerializeField] private GameObject _objSpecialStatEmpty;

    private List<StatusTableData> _specialStat = new();
    
    [Header("상세 보기")]
    [SerializeField] private TextMeshProUGUI _textStatName;
    [SerializeField] private TextMeshProUGUI _textStatDesc;
    [SerializeField] private GameObject[] _relatedStats;
    [SerializeField] private GameObject _statInfoParents;
    private UIInfoRelatedStat[] _relatedStatsUI = new UIInfoRelatedStat[3];
    [SerializeField] private GameObject _objStatInfoEmpty;

    private int curSelected = 0;
    
    private string[] stats = new String[6] {
        "status_damage_factor",
        "status_reduce_defense_factor",
        "status_hit_factor",
        "status_hp",
        "status_defense_factor",
        "status_dodge_factor"
    };

    private void Awake()
    {
        hideButton.onClick.AddListener(Hide);
        hideBGButton.onClick.AddListener(Hide);
        _scrollView.InitScrollView(OnUpdateScrollView, _scrollViewObj);

        for (int i = 0; i < _relatedStats.Length; ++i)
        {
            _relatedStatsUI[i] = _relatedStats[i].GetOrAddComponent<UIInfoRelatedStat>();
        }
        
    }

    GameObject OnUpdateScrollView(int index)
    {
        var item = _scrollView.GetItem( _scrollViewObj).GetOrAddComponent<UISpecialStat>();
        item.SetUI(_specialStat[index].status_id);
        return item.gameObject;
    }

    protected override void OnShow(params object[] param)
    {
        base.OnShow(param);
        _uiStat = new UIStat[stats.Length];

        _specialStat = GameDataManager.Instance._statusData.FindAll(_ => _ is {is_ui_show: true, function_type: "get_stat"} && GamePlayerManager.Instance.myActor.playerStat.GetStat(_.status_id ) > 0);
        _scrollView.MakeList(_specialStat.Count);
        _objSpecialStatEmpty.SetActive(_specialStat.Count <= 0);
                
        SetNormaStatUI();
        curSelected = 0;
        SetStatInfo(curSelected);
    }

    private void SetNormaStatUI()
    {
        for (int i = 0; i < stats.Length; ++i)
        {
            _uiStat[i] = _goUIstat[i].AddComponent<UIStat>();
            var statId = GameDataManager.Instance._configTableData.Find(_ => _.config_id.Equals(stats[i]));
            var curAmount = GamePlayerManager.Instance.myActor.playerStat.GetStat((int) statId.GetValueConfigData());
            var tableData = GamePlayerManager.Instance.myActor.playerStat.GetStatusData((int) statId.GetValueConfigData());
            _uiStat[i].SetUI(tableData.status_rsc, tableData.status_name, curAmount, (int)statId.GetValueConfigData());
        }
    }

    public void SetStatInfo(int id)
    {
        curSelected = id;
        var statInfo = GameDataManager.Instance._statusData.Find(_ => _.status_id == id);
        if (statInfo == null)
        {
            _statInfoParents.SetActive(false);
            _objStatInfoEmpty.SetActive(true);
        }
        else
        {
            _statInfoParents.SetActive(true);
            _objStatInfoEmpty.SetActive(false);
            _textStatName.text = statInfo.status_name;
            _textStatDesc.text = statInfo.status_desc;
            
            for (int i = 0; i < _relatedStats.Length; ++i)
            {
                if (statInfo.function_type is "get_stat" && i < statInfo.function_value_1.Count)
                {
                    _relatedStats[i].gameObject.SetActive(true);
                    var subStatInfo = GameDataManager.Instance._statusData.Find(_ => _.status_id == statInfo.function_value_1[i]);
                    _relatedStatsUI[i].SetUI(subStatInfo.status_rsc, subStatInfo.status_name, statInfo.function_value_2[i], subStatInfo.status_id);
                }
                else
                {
                    _relatedStats[i].gameObject.SetActive(false);
                }
            }
        }
    }

    private class UIStat : MonoBehaviour
    {
        private Image _imgStat;
        private TextMeshProUGUI _statName;
        private TextMeshProUGUI _statAmount;
        private DTButton _button;
        private int _statId;
        private void Awake()
        {
            _imgStat = transform.Find("ImageIcon").GetComponent<Image>();
            _statName = transform.Find("TextStatName").GetComponent<TextMeshProUGUI>();
            _statAmount = transform.Find("TextStatAmount").GetComponent<TextMeshProUGUI>();
            _button = transform.GetComponent<DTButton>();
            _button.onClick.AddListener(OnClickButton);
        }

        private void OnClickButton()
        {
            if (GameUIManager.Instance.TryGetOrCreate<UIPopUpStatus>(false, UILayer.LEVEL_2, out var ui))
            {
                ui.SetStatInfo(_statId);
            }
        }

        public void SetUI(string imageName, string textName, int amount, int statId)
        {
            _statId = statId;
            _imgStat.sprite = GameResourceManager.Instance.GetImage(imageName);
            _statName.text = textName;
            _statAmount.text = amount.ToString();
        }
    }

    private class UISpecialStat : MonoBehaviour
    {
        private TextMeshProUGUI _txtStatName;
        private TextMeshProUGUI _txtStatAmount;
        private UIRelatedStat[] _relatedStats = new UIRelatedStat[3];
        private DTButton _button;
        private int _statID;
        private void Awake()
        {
            _txtStatName = transform.Find("TextStatName").GetComponent<TextMeshProUGUI>();
            _txtStatAmount = transform.Find("TextStatAmount").GetComponent<TextMeshProUGUI>();
            var statsParents = transform.Find("UIBonusStatus");
            for (int i = 0; i < statsParents.childCount; ++i)
            {
                _relatedStats[i] = statsParents.GetChild(i).GetOrAddComponent<UIRelatedStat>();
            }

            _button = transform.GetComponent<DTButton>();
            _button.onClick.AddListener(OnClickButton);
        }
        private void OnClickButton()
        {
            if (GameUIManager.Instance.TryGetOrCreate<UIPopUpStatus>(false, UILayer.LEVEL_2, out var ui))
            {
                ui.SetStatInfo(_statID);
            }
        }
        public void SetUI(int id)
        {
            var statAmount = GamePlayerManager.Instance.myActor.playerStat.GetStat(id);
            var statData = GamePlayerManager.Instance.myActor.playerStat.GetStatusData(id);
            _statID = id;
            _txtStatAmount.text = statAmount.ToString();
            _txtStatName.text = statData.status_name;
            for (int i = 0; i < _relatedStats.Length; ++i)
            {
                if (i < statData.function_value_1.Count)
                {
                    var statSubData = GamePlayerManager.Instance.myActor.playerStat.GetStatusData(statData.function_value_1[i]);
                    _relatedStats[i].SetUI(statSubData.status_name, statAmount *  statData.function_value_2[i], statSubData.status_id);
                }
                else
                {
                    _relatedStats[i].gameObject.SetActive(false);
                }
            }
        }
    }

    private class UIRelatedStat : MonoBehaviour
    {
        private Image _statImage;
        private TextMeshProUGUI _statAmount;
        private DTButton _button;
        private int _statID;
        private void Awake()
        {
            _statImage = transform.Find("Image").GetComponent<Image>();
            _statAmount = transform.Find("TextAmount").GetComponent<TextMeshProUGUI>();
            _button = transform.GetComponent<DTButton>();
            _button.onClick.AddListener(OnClickButton);
        }

        public void SetUI(string imageName, int statPerAmount, int statID)
        {
            _statID = statID;
            _statImage.sprite = GameResourceManager.Instance.GetImage(imageName);
            _statAmount.text = statPerAmount > 0 ? $"+{statPerAmount.ToString()}" : statPerAmount.ToString();
        }
        
        private void OnClickButton()
        {
            if (GameUIManager.Instance.TryGetOrCreate<UIPopUpStatus>(false, UILayer.LEVEL_2, out var ui))
            {
                ui.SetStatInfo(_statID);
            }
        }
    }
    
    private class UIInfoRelatedStat : MonoBehaviour
    {
        private Image _statImage;
        private TextMeshProUGUI _statAmount;
        private TextMeshProUGUI _statName;
        private DTButton _button;

        private int _statID;
        private void Awake()
        {
            _statImage = transform.Find("Image").GetComponent<Image>();
            _statName = transform.Find("TextName").GetComponent<TextMeshProUGUI>();
            _statAmount = transform.Find("TextAmount").GetComponent<TextMeshProUGUI>();
            _button = transform.GetComponent<DTButton>();
            _button.onClick.AddListener(OnClickButton);
        }

        public void SetUI(string imageName, string statName ,int statPerAmount, int statID)
        {
            _statID = statID;
            _statImage.sprite = GameResourceManager.Instance.GetImage(imageName);
            _statName.text = statName;
            _statAmount.text = statPerAmount > 0 ? $"+{statPerAmount.ToString()}" : statPerAmount.ToString();
        }
        private void OnClickButton()
        {
            if (GameUIManager.Instance.TryGetOrCreate<UIPopUpStatus>(false, UILayer.LEVEL_2, out var ui))
            {
                ui.SetStatInfo(_statID);
            }
        }
    }
}
