using System.Collections;
using System.Collections.Generic;
using Script.Manager;
using Script.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInfoRelatedStat : MonoBehaviour
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
        if (GameUIManager.Instance.TryGetOrCreate<UIPopUpStatus>(false, UILayer.LEVEL_4, out var ui))
        {
            ui.Show();
            ui.SetStatInfo(_statID);
        }
    }
}
