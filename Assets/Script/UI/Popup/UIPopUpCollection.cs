using System;
using System.Collections.Generic;
using Script.Manager;
using Script.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIPopUpCollection : UIBase
{
    enum Tab_Type
    {
        ITEM,
        STATUS,
        MONSTER,
    }

    [Header("")] 
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI titleInfo;
    [SerializeField] private DTButton dimButton;
    [SerializeField] private DTButton hideButton;
    
    
    [Header("main")]
    [SerializeField] private DTScrollView _scrollView;
    [SerializeField] private GameObject _itemThumbnail;
    [SerializeField] private GameObject _status;
    [SerializeField] private GameObject _monster;
    [SerializeField] private DTButton[] _tabButtons;
    [SerializeField] private Transform[] _trSelectedDim;
    
    [Header("ItemInfo")]
    [SerializeField] private TextMeshProUGUI _textStatName;
    [SerializeField] private TextMeshProUGUI _textStatDesc;
    [SerializeField] private GameObject[] _relatedStats;
    [SerializeField] private GameObject _statInfoParents;
    private UIInfoRelatedStat[] _relatedStatsUI = new UIInfoRelatedStat[3];
    [SerializeField] private TextMeshProUGUI _txtStatInfoEmpty;
    [SerializeField] private Image _equipType;
    [SerializeField] private Transform _equipTypeParent;

    private Tab_Type _curTabType = Tab_Type.ITEM;
   
    private List<MonsterTableData> _monsterData = new();
    private List<ItemTableData> _itemData = new();
    private List<StatusTableData> _statusTable = new();

    private int _curSelected = 0;
    private bool _isSelectedDataNull = true;
    void Awake()
    {
        _scrollView.InitScrollView(OnUpdateScrollView, _itemThumbnail, _status, _monster);
        for (int i = 0; i < _tabButtons.Length; ++i)
        {
            int capturedIndex = i;
            _tabButtons[i].onClick.AddListener(()=>OnSelectTab((Tab_Type)capturedIndex));
        }
        
        _monsterData = GameDataManager.Instance._monsterTableData.FindAll(_ => _.is_collection_check == true);
        _itemData = GameDataManager.Instance._itemData.FindAll(_ => _.is_collection_check == true);
        _statusTable = GameDataManager.Instance._statusData.FindAll(_ => _.is_collection_check == true);

        for (int i = 0; i < _relatedStats.Length; ++i)
        {
           _relatedStatsUI[i]  =  _relatedStats[i].GetOrAddComponent<UIInfoRelatedStat>();
        }
        
        dimButton.onClick.AddListener(Hide);
        hideButton.onClick.AddListener(Hide);
    }

    protected override void OnShow(params object[] param)
    {
        base.OnShow(param);
        RefreshUI();
    }

    private void OnSelectTab(Tab_Type tabType)
    {
        _curTabType = tabType;
        _curSelected = 0;
        
        for (int i = 0; i < _trSelectedDim.Length; ++i)
        {
            _trSelectedDim[i].gameObject.SetActive(i == (int)_curTabType);
        }
        RefreshUI();
    }

    private void RefreshUI()
    {
        int listCount = _curTabType switch
        {
            Tab_Type.ITEM => _itemData.Count,
            Tab_Type.STATUS => _statusTable.Count,
            Tab_Type.MONSTER => _monsterData.Count,
            _ => 0
        };
        switch (_curTabType)
        {
            case Tab_Type.ITEM:
                SetItemInfo(_curSelected);
                break;
            case Tab_Type.STATUS:
                SetStatInfo(_curSelected);
                break;
            case Tab_Type.MONSTER:
                SetMonsterInfo(_curSelected);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        title.text = _curTabType switch
        {
            Tab_Type.ITEM => "아이템",
            Tab_Type.STATUS => "능력치",
            Tab_Type.MONSTER => "몬스터",
            _ => string.Empty
        };
        titleInfo.text = _curTabType switch
        {
            Tab_Type.ITEM => "아이템 상세",
            Tab_Type.STATUS => "능력치 상세",
            Tab_Type.MONSTER => "몬스터 상세",
            _ => string.Empty
        };;
        _scrollView.MakeList(listCount);
    }
    GameObject OnUpdateScrollView(int index)
    {
        switch (_curTabType)
        {
            case Tab_Type.ITEM:
            {
                var item = _scrollView.GetItem( _itemThumbnail.GameObject());
                var itemThumbnail = item.GetComponent<UIItemThumbnail>();
                var itemInfo = GameItemManager.Instance.GetItem(_itemData[index]?.item_id ??0);
                
                itemThumbnail.SetItemInfo(_itemData[index]?.item_id ?? 0, false);
                itemThumbnail.SetEquipIcon(false);
                itemThumbnail.SetOnClickEvent(()=>OnClickThumbnail(_itemData[index]?.item_id?? 0 , itemInfo <= 0));
                itemThumbnail.SetNull(itemInfo <= 0);
                
                return itemThumbnail.gameObject;
            } break;
            case Tab_Type.STATUS:
            {
                var item = _scrollView.GetItem( _status.GameObject());
                var normalThumbnail = item.GetOrAddComponent<NormalThumbnail>();
                var isHaveItem = GamePlayerManager.Instance.myActor?.playerStat?.GetStat(_statusTable[index]?.status_id?? 0) ?? 0;
                var id = _statusTable[index].status_id ?? 0;
                normalThumbnail.SetImage(_statusTable[index].status_rsc, isHaveItem <= 0, () => OnClickThumbnail(id, isHaveItem <=0));
                return normalThumbnail.gameObject;
            } break;
            case Tab_Type.MONSTER:
            {
                var item = _scrollView.GetItem( _monster.GameObject()); 
                var normalThumbnail = item.GetOrAddComponent<NormalThumbnail>();

                int metCount = 0;
                var isHaveKill = GamePlayerManager.Instance._metMonsterDic?.TryGetValue(_monsterData[index].monster_id ?? 0, out metCount);
                var monsterID = _monsterData[index]?.monster_id ?? 0;
                normalThumbnail.SetImage(_monsterData[index].monter_img, metCount <= 0, ()=>OnClickThumbnail(monsterID, metCount <= 0) );
                return normalThumbnail.gameObject;
            } break;
        }

        return null;
    }

    private void OnClickThumbnail(int id, bool isNull)
    {
        _curSelected = id;
        _isSelectedDataNull = isNull;
        RefreshUI();
    }
    
    private void SetItemInfo(int itemID)
    {
        _curSelected = itemID;
        var itemInfo = GameDataManager.Instance._itemData.Find(_ => _.item_id == itemID);
        _equipTypeParent.gameObject.SetActive(false);
        if (itemInfo == null)
        {
            _statInfoParents.SetActive(false);
            _txtStatInfoEmpty.gameObject.SetActive(true);
            _txtStatInfoEmpty.text = "선택된 아이템이 없습니다.\n아이템을 눌러 정보를 확인하세요.";
        }
        else
        {
            if (_isSelectedDataNull)
            {
                _statInfoParents.SetActive(false);
                _txtStatInfoEmpty.gameObject.SetActive(true);
                _txtStatInfoEmpty.text = "아직 획득한 적 없는 아이템입니다.";
            }
            else
            {
                _statInfoParents.SetActive(true);
                _txtStatInfoEmpty.gameObject.SetActive(false);
                _textStatDesc.text = itemInfo.item_desc;
                var rarityData = GameDataManager.Instance._rarityData.Find(_ => _.rarity_id == itemInfo.rarity_id);
                string rarityString = $"<color={rarityData.rarity_color}>[{rarityData.rarity_string}]</color>{itemInfo.item_name}";

                _textStatName.text = rarityString;
                for (int i = 0; i < _relatedStats.Length; ++i)
                {
                    if (itemInfo.function_type is ITEM_FUNCTION_TYPE.ITEM_FUNCTION_TYPE_CHAGE_STATUS or ITEM_FUNCTION_TYPE.ITEM_FUNCTION_TYPE_EQUIP && i < itemInfo.function_value_1.Length)
                    {
                        _relatedStats[i].gameObject.SetActive(true);
                        var subStatInfo = GameDataManager.Instance._statusData.Find(_ => _.status_id == itemInfo.function_value_1[i]);
                        _relatedStatsUI[i].SetUI(subStatInfo.status_rsc, subStatInfo.status_name, itemInfo.function_value_2[i], subStatInfo?.status_id ??0);
                    }
                    else
                    {
                        _relatedStats[i].gameObject.SetActive(false);
                    }
                }

                if (itemInfo.function_type.Equals("equip"))
                {
                    _equipTypeParent.gameObject.SetActive(true);
                    var equipIconName = $"ui_icon_item_type_{itemInfo.item_type}";
                    _equipType.sprite = GameResourceManager.Instance.GetImage(equipIconName);
                }
                else
                {
                    _equipTypeParent.gameObject.SetActive(false);
                }
                
            }
            
        }
    }
    public void SetStatInfo(int id)
    {
        _curSelected = id;
        var statInfo = GameDataManager.Instance._statusData.Find(_ => _.status_id == id);
        _equipTypeParent.gameObject.SetActive(false);
        if (statInfo == null)
        {
            _statInfoParents.SetActive(false);
            _txtStatInfoEmpty.gameObject.SetActive(true);
            _txtStatInfoEmpty.text = "선택된 능력치가 없습니다.\n능력치를 눌러 정보를 확인하세요.";
        }
        else
        {
            if (_isSelectedDataNull)
            {
                _statInfoParents.SetActive(false);
                _txtStatInfoEmpty.gameObject.SetActive(true);
                _txtStatInfoEmpty.text = "아직 획득한 적 없는 능력치입니다.";
            }
            else
            {
                _statInfoParents.SetActive(true);
                _txtStatInfoEmpty.gameObject.SetActive(false);
                _textStatName.text = statInfo.status_name;
                _textStatDesc.text = statInfo.status_desc;
                
                for (int i = 0; i < _relatedStats.Length; ++i)
                {
                    if (statInfo.function_type is STATUS_FUNCTION_TYPE.STATUS_FUNCTION_TYPE_GET_STAT && i < statInfo.function_value_1.Length)
                    {
                        _relatedStats[i].gameObject.SetActive(true);
                        var subStatInfo = GameDataManager.Instance._statusData.Find(_ => _.status_id == statInfo.function_value_1[i]);
                        _relatedStatsUI[i].SetUI(subStatInfo.status_rsc, subStatInfo.status_name, statInfo.function_value_2[i], subStatInfo?.status_id ?? 0);
                    }
                    else
                    {
                        _relatedStats[i].gameObject.SetActive(false);
                    }
                }
            }
           
        }
    }
    public void SetMonsterInfo(int id)
    {
        _curSelected = id;
        var monsterdata = GameDataManager.Instance._monsterTableData.Find(_ => _.monster_id == id);
        _equipTypeParent.gameObject.SetActive(false);
        foreach (var stat in _relatedStats)
        {
            stat.gameObject.SetActive(false);
        }
        if (monsterdata == null)
        {
            _statInfoParents.SetActive(false);
            _txtStatInfoEmpty.gameObject.SetActive(true);
            _txtStatInfoEmpty.text = "선택된 몬스터가 없습니다.\n몬스터 아이콘을 눌러 정보를 확인하세요.";
        }
        else
        {
            if (_isSelectedDataNull)
            {
                _statInfoParents.SetActive(false);
                _txtStatInfoEmpty.gameObject.SetActive(true);
                _txtStatInfoEmpty.text = "아직 만난적 없는 몬스터입니다.";
            }
            else
            {
                _statInfoParents.SetActive(true);
                _txtStatInfoEmpty.gameObject.SetActive(false);
                _textStatName.text = monsterdata.monster_name;
                _textStatDesc.text = monsterdata.monster_desc;
            }
        }
    }

    private class NormalThumbnail : MonoBehaviour
    {
        private Image _portrait;
        private Image _nullThumbnail;
        private DTButton _button;
        private void Awake()
        {
            _portrait = transform.FindComponent<Image>("ImageContent");
            _nullThumbnail = transform.FindComponent<Image>("ImageNullContent");
            _button = transform.FindComponent<DTButton>("btn");
        }

        internal void SetImage(string imageName, bool isNull, Action onClickAction)
        {
            _nullThumbnail.gameObject.SetActive(isNull);
            _portrait.gameObject.SetActive(isNull == false);
            if (isNull == false)
            {
                _portrait.sprite = GameResourceManager.Instance.GetImage(imageName);
            }
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(()=>onClickAction());
        }
    }
}
