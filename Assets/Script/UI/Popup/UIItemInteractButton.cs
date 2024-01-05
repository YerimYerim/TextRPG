using System;
using Script.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIItemInteractButton : UIBase
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private DTButton _button;
    [SerializeField] private Transform _tr;

    private event Action _onclickEvent ;
    public void SetData(int itemKey, Vector3 position, Action action = null)
    {
        var rect = _tr as RectTransform;
        _onclickEvent = action;
        if (rect != null)
        {
            var clonedPosition = new Vector3(position.x, position.y, position.z)
            {
                y = position.y - rect.rect.height * 0.5f
            };
            rect.position = clonedPosition;
        }

        var itemTableData = GameDataManager.Instance._itemData.Find(_ => _.item_id == itemKey);
        
        _button.onClick.RemoveAllListeners();
        
        if (itemTableData.function_type == ITEM_FUNCTION_TYPE.ITEM_FUNCTION_TYPE_EQUIP)
        {
            var itemID = GameItemManager.Instance.GetEquippedItem(itemTableData?.item_type ?? ITEM_TYPE.ITEM_TYPE_NORMAL);
            if (itemID != null)
            {
                _text.text = "해제";
                _button.onClick.AddListener(()=>
                {
                    OnClickUnEquip(itemTableData?.item_type ?? ITEM_TYPE.ITEM_TYPE_NORMAL);
                    SetData(itemKey, position);
                });
            }
            else
            {
                _text.text = "착용";
                _button.onClick.AddListener(()=>
                {
                    OnClickEquip(itemKey);
                    SetData(itemKey, position);
                });
            }
        }

        else
        {
            if(itemTableData.function_type.Equals("change_status"))
            {
                _text.text = "사용";
                _button.onClick.AddListener(()=>
                {
                    OnClickUseItem(itemKey);
                    SetData(itemKey, position);
                });
            }
            else
            {
                Hide();
            }
        }
    }

    void OnClickEquip(int itemKey)
    {
        GameItemManager.Instance.EquipItem(itemKey);
        _onclickEvent?.Invoke();
    }

    void OnClickUnEquip(ITEM_TYPE equipType)
    {
        GameItemManager.Instance.UnEquipItem(equipType);
        _onclickEvent?.Invoke();
    }

    void OnClickUseItem(int itemKey)
    {
        GameItemManager.Instance.UseItem(itemKey, 1);
        _onclickEvent?.Invoke();
    }
}
