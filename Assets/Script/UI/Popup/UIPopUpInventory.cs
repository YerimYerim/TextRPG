using System;
using Script.Manager;
using Script.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIPopUpInventory : UIBase
{
    [SerializeField] private DTButton hideButton;
    [SerializeField] private DTButton hideBGButton;
    [Header("Inventory")] 
    [SerializeField] private UIItemThumbnail _itemThumbnail;
    [SerializeField] private DTScrollView _itemScrollView;
    [SerializeField] private GameObject _objInvenEmpty;
    [Header("ItemInfo")]
    [SerializeField] private TextMeshProUGUI _textStatName;
    [SerializeField] private TextMeshProUGUI _textStatDesc;
    [SerializeField] private GameObject[] _relatedStats;
    [SerializeField] private GameObject _statInfoParents;
    private UIInfoRelatedStat[] _relatedStatsUI = new UIInfoRelatedStat[3];
    [SerializeField] private GameObject _objStatInfoEmpty;
    [SerializeField] private Image _equipType;
    [SerializeField] private Transform _equipTypeParent;
    private int _curSelected = 0;
    private void Awake()
    {
        hideButton.onClick.AddListener(Hide);
        hideBGButton.onClick.AddListener(Hide);
        _itemScrollView.InitScrollView(OnUpdateScrollView, _itemThumbnail.gameObject);

        for (int i = 0; i < _relatedStats.Length; ++i)
        {
            _relatedStatsUI[i] = _relatedStats[i].GetOrAddComponent<UIInfoRelatedStat>();
        }
    }

    public override void Show()
    {
        base.Show();
        SetItemInfo(-1);
        SetInventory();
    }
    GameObject OnUpdateScrollView(int index)
    {
        var item = _itemScrollView.GetItem( _itemThumbnail.gameObject).GetOrAddComponent<UIItemThumbnail>();
        item.SetItemInfo(GameItemManager.Instance.GetItemByIndex(index).itemKey, true);
        item.SetOnClickEvent(()=>SetItemInfo(GameItemManager.Instance.GetItemByIndex(index).itemKey));
  
        return item.gameObject;
    }
    
    private void SetInventory()
    {
        if (GameItemManager.Instance.GetItemCountAll() > 0)
        {
            _itemScrollView.gameObject.SetActive(true);
            _objInvenEmpty.SetActive(false);
            _itemScrollView.MakeList(GameItemManager.Instance.GetItemCountAll());
        }
        else
        {
            _itemScrollView.gameObject.SetActive(false);
            _objInvenEmpty.SetActive(true);
        }
    }

    private void SetItemInfo(int itemID)
    {
        _curSelected = itemID;
        var itemInfo = GameDataManager.Instance._itemData.Find(_ => _.item_id == itemID);
        if (itemInfo == null)
        {
            _statInfoParents.SetActive(false);
            _objStatInfoEmpty.SetActive(true);
        }
        else
        {
            _statInfoParents.SetActive(true);
            _objStatInfoEmpty.SetActive(false);
            _textStatDesc.text = itemInfo.item_desc;
            var rarityData = GameDataManager.Instance._rarityData.Find(_ => _.rarity_id == itemInfo.rarity_id);
            string rarityString = $"<color={rarityData.rarity_color}>[{rarityData.rarity_string}]</color>{itemInfo.item_name}";

            _textStatName.text = rarityString;
            for (int i = 0; i < _relatedStats.Length; ++i)
            {
                if ((itemInfo.function_type is "change_status" || itemInfo.function_type is "equip")  && i < itemInfo.function_value_1.Count)
                {
                    _relatedStats[i].gameObject.SetActive(true);
                    var subStatInfo = GameDataManager.Instance._statusData.Find(_ => _.status_id == itemInfo.function_value_1[i]);
                    _relatedStatsUI[i].SetUI(subStatInfo.status_rsc, subStatInfo.status_name, itemInfo.function_value_2[i], subStatInfo.status_id);
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
