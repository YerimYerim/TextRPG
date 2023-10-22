using System;
using Script.Manager;
using Script.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIToolBar : MonoBehaviour
{
    enum BottomButtonType
    {
        INVENTORY,
        STATUS,
        COLLECTION,
        ACHIEVEMENT,
        SETTING,
    }

    private ToolBarButton[] toolbarButton = new ToolBarButton[5];
    
    private void Awake()
    {
        for (int i = 0; i < toolbarButton.Length; ++i)
        {
            toolbarButton[i] = transform.GetChild(i).AddComponent<ToolBarButton>();
            toolbarButton[i].SetUI((BottomButtonType) i);
        }
    }

    #region INNER
    private class ToolBarButton : MonoBehaviour
    {
        private DTButton _button;
        private Image _image;
        private TextMeshProUGUI _text;
        
        private void Awake()
        {
            _button = GetComponent<DTButton>();
            _image = transform.Find("Image").GetComponent<Image>();
            _text = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        }

        public void SetUI(BottomButtonType type)
        {
            switch (type)
            {
                case BottomButtonType.INVENTORY:
                    _text.text = "가방";
                    break;
                case BottomButtonType.STATUS:
                    _text.text = "상태";
                    break;
                case BottomButtonType.COLLECTION:
                    _text.text = "도감";
                    break;
                case BottomButtonType.ACHIEVEMENT:
                    _text.text = "업적";
                    break;
                case BottomButtonType.SETTING:
                    _text.text = "설정";
                    _image.sprite = GameResourceManager.Instance.GetImage("ui_icon_toolbar_setting");
                    break;
                default:
                    break;
            }
        }
    }
    
    #endregion
}
