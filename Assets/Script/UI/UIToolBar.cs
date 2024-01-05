
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
                    _button.onClick.AddListener(OnClickInventory);
                    break;
                case BottomButtonType.STATUS:
                    _text.text = "상태";
                    _button.onClick.AddListener(OnClickStatus);
                    break;
                case BottomButtonType.COLLECTION:
                    _text.text = "도감";
                    _button.onClick.AddListener(OnClickCollection);
                    break;
                case BottomButtonType.ACHIEVEMENT:
                    _text.text = "업적";
                    _button.onClick.AddListener(OnClickAchievement);
                    break;
                case BottomButtonType.SETTING:
                    _text.text = "설정";
                    _image.sprite = GameResourceManager.Instance.GetImage("ui_icon_toolbar_setting");
                    break;
                default:
                    break;
            }
        }

        private void OnClickStatus()
        {
            if (GameUIManager.Instance.TryGetOrCreate<UIPopUpStatus>(false, UILayer.LEVEL_4, out var ui))
            {
                ui.Show();
            }
        }        
        
        private void OnClickInventory()
        {
            if (GameUIManager.Instance.TryGetOrCreate<UIPopUpInventory>(false, UILayer.LEVEL_3, out var ui))
            {
                ui.Show();
            }
        }
        private void OnClickAchievement()
        {
            GameShortCutManager.Instance.GoToContent(CONTENT_TYPE.CONTENT_TYPE_ACHIEVEMENT);
        }

        private void OnClickCollection()
        {
            GameShortCutManager.Instance.GoToContent(CONTENT_TYPE.CONTENT_TYPE_COLLECTION);
        }
    }
    
    #endregion
}
