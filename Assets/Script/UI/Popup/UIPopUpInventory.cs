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
    
    [Header("EquipItem")] 
    [SerializeField] private GameObject[] _objEquippedItems;
    private UIEquippedItem[] _equippedItems = new UIEquippedItem[6];
    private ITEM_TYPE[] equipItemTypes = {
        ITEM_TYPE.ITEM_TYPE_EQUIP_WEAPON,
        ITEM_TYPE.ITEM_TYPE_EQUIP_HEAD,
        ITEM_TYPE.ITEM_TYPE_EQUIP_ARMOR,
        ITEM_TYPE.ITEM_TYPE_EQUIP_SHOES,
        ITEM_TYPE.ITEM_TYPE_EQUIP_RING,
        ITEM_TYPE.ITEM_TYPE_EQUIP_NECKLACE,
    };
    
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
        
        for (int i = 0; i < _objEquippedItems.Length; ++i)
        {
            _equippedItems[i] = _objEquippedItems[i].GetOrAddComponent<UIEquippedItem>();
        }
    }

    public override void Show()
    {
        base.Show();
        #if UNITY_EDITOR
            //?? 예림 : 나중에 치트로 빠질 부분
            GameItemManager.Instance.AddItem(10001, 1);
            GameItemManager.Instance.AddItem(10002, 1);
            GameItemManager.Instance.AddItem(10003, 1);
            GameItemManager.Instance.AddItem(10004, 1);
            GameItemManager.Instance.AddItem(10005, 1);
            GameItemManager.Instance.AddItem(10006, 1);
            GameItemManager.Instance.AddItem(20001, 1);
        #endif
        SetItemInfo(-1);
        SetInventory();
        SetEquippedItem();
    }

    protected override void OnHide(params object[] param)
    {
        base.OnHide(param);
        if (GameUIManager.Instance.TryGet<UIItemInteractButton>(out var ui))
        {
            ui.Hide();
        }
    }

    GameObject OnUpdateScrollView(int index)
    {
        var item = _itemScrollView.GetItem( _itemThumbnail.gameObject).GetOrAddComponent<UIItemThumbnail>();
        var itemKey = GameItemManager.Instance.GetItemByIndex(index).itemKey;
        
        item.SetItemInfo(itemKey, true);
        item.SetOnClickEvent(()=>OnClickItem(itemKey, item));
        
        var itemData = GameDataManager.Instance._itemData.Find(_ => _.item_id == itemKey);
        var equippedItem = GameItemManager.Instance.GetEquippedItem(itemData.item_type ?? ITEM_TYPE.ITEM_TYPE_NORMAL);
        
        item.SetEquipIcon(equippedItem == itemKey);
        
        return item.gameObject;
    }

    private void OnClickItem(int itemKey, UIItemThumbnail itemThumbnail)
    {
        SetItemInfo(itemKey);
        if (GameUIManager.Instance.TryGetOrCreate<UIItemInteractButton>(true, UILayer.LEVEL_4, out var ui))
        {
            RectTransform rectTransform = itemThumbnail.transform as RectTransform;
            if (rectTransform != null)
            {
                Vector3 position = itemThumbnail.transform.position;
                position.y -= rectTransform.rect.height * 0.5f;
                ui.SetData(itemKey, position, ()=>
                {
                    SetInventory();
                    SetEquippedItem();
                    ui.Hide();
                });
                ui.Show();
            }
        }
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
                if (itemInfo.function_type is ITEM_FUNCTION_TYPE.ITEM_FUNCTION_TYPE_CHAGE_STATUS or ITEM_FUNCTION_TYPE.ITEM_FUNCTION_TYPE_EQUIP && i < itemInfo.function_value_1.Length)
                {
                    _relatedStats[i].gameObject.SetActive(true);
                    var subStatInfo = GameDataManager.Instance._statusData.Find(_ => _.status_id == itemInfo.function_value_1[i]);
                    _relatedStatsUI[i].SetUI(subStatInfo.status_rsc, subStatInfo.status_name, itemInfo.function_value_2[i], subStatInfo?.status_id ?? 0);
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

    private void SetEquippedItem()
    {
        for (int i = 0; i < _objEquippedItems.Length; ++i)
        {
            var equippedItem = GameItemManager.Instance.GetEquippedItem(equipItemTypes[i]);
            var capturedIndex = i;
            _equippedItems[i].SetUI($"ui_icon_item_type_{equipItemTypes[i]}", equippedItem ?? 0, ()=>
            {
                OnClickItem(equippedItem ?? 0, _equippedItems[capturedIndex].GetUIThumbNail());
            });
        }
    }
    
    public class UIEquippedItem : MonoBehaviour
    {
        private UIItemThumbnail _itemThumbnail;
        private Image _emptyBG;
        private event Action _onClickEvent;
        private void Awake()
        {
            _itemThumbnail = transform.FindComponent<UIItemThumbnail>("UIItemThumbnail");
            _emptyBG = transform.FindComponent<Image>("itemIcon");
        }
        
        public void SetUI(string bgStringItem, int itemKey, Action onClickEvent)
        {
            _onClickEvent = onClickEvent;
            _emptyBG.sprite = GameResourceManager.Instance.GetImage(bgStringItem);
            _itemThumbnail.SetItemInfo(itemKey, false);
            _itemThumbnail.SetOnClickEvent(_onClickEvent);
            _itemThumbnail.SetEquipIcon(false);
        }

        public UIItemThumbnail GetUIThumbNail()
        {
            return _itemThumbnail;
        }
    }
}
