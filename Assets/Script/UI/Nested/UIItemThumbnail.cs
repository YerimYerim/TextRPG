using System;
using Script.Manager;
using Script.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItemThumbnail : MonoBehaviour
{
    private Image _rarityBg;
    private Image _itemThumbnail;
    private Transform _trEquipIconParents;
    private Image _equipIcon;
    private TextMeshProUGUI _itemCount;
    private DTButton _button;
    
    private Transform _equippedIcon;
    private void Awake()
    {
        _rarityBg = transform.FindComponent<Image>("ImageRarityBG");
        _itemThumbnail = transform.FindComponent<Image>("ImageThumbnail");
        _trEquipIconParents = transform.Find("UIEquipIconGroup");
        _equipIcon = transform.FindComponent<Image>("UIEquipIconGroup/ImageEquipIcon");
        _itemCount = transform.FindComponent<TextMeshProUGUI>("TextItemCount");
        _button = transform.GetComponent<DTButton>();
        _equippedIcon = transform.Find("UIEquipIconGroup2");
    }

    public void SetItemInfo(int itemKey, bool useItemCount)
    {
        var itemInfo = GameDataManager.Instance._itemData.Find(_ => _.item_id == itemKey);
        transform.gameObject.SetActive(itemInfo != null);
        if (itemInfo == null)
        {
            return;
        }
        var rarityInfo = GameDataManager.Instance._rarityData.Find(_ => _.rarity_id == itemInfo.rarity_id);
        _rarityBg.sprite = GameResourceManager.Instance.GetImage(rarityInfo.rarity_rsc);
        _itemThumbnail.sprite = GameResourceManager.Instance.GetImage(itemInfo.item_rsc);

        if (itemInfo.function_type.Equals("equip"))
        {
            _trEquipIconParents.gameObject.SetActive(true);
            var equipIconName = $"ui_icon_item_type_{itemInfo.item_type}";
            _equipIcon.sprite = GameResourceManager.Instance.GetImage(equipIconName);
        }
        else
        {
            _trEquipIconParents.gameObject.SetActive(false);
        }

        if (useItemCount)
        {
            _itemCount.gameObject.SetActive(true);
            _itemCount.text = GameItemManager.Instance.GetItem(itemKey).ToString();
        }
        else
        {
            _itemCount.gameObject.SetActive(false);
        }
    }

    public void SetOnClickEvent(Action action)
    {
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(() => action?.Invoke());
    }

    public void SetOnClickLongEvent(Action action)
    {
        _button.SetLongClickEvent(()=>action?.Invoke());
    }

    public void SetEquipIcon(bool active)
    {
        _equippedIcon.gameObject.SetActive(active);
    }
}
