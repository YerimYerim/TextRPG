using System.Collections;
using Lofle.Tween;
using Script.Manager;
using Script.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIToastMsg : UIBase
{
    [SerializeField] private DTButton shorCutBtn;
    [SerializeField] private TextMeshProUGUI txtTitle;
    [SerializeField] private TextMeshProUGUI txtDesc;
    [SerializeField] private Image imgIcon;
    
    [SerializeField] private UITweenPosition uiTweenPosition;
    [SerializeField] private UITweenColorAlpha _tweenColorAlpha;
    
    private Coroutine _coroutineShowPopup;
    
    protected override void OnShow(params object[] param)
    {
        base.OnShow(param);
        shorCutBtn.interactable = false;
        
        uiTweenPosition.Play(true);
        _tweenColorAlpha.Play(true, WaitTweenStart);
    }

    public void SetUI(CONTENT_TYPE contentType, string iconName, string title, string desc)
    {
        shorCutBtn.onClick.RemoveAllListeners();
        shorCutBtn.onClick.AddListener(()=>GameShortCutManager.Instance.GoToContent(contentType));
        
        imgIcon.sprite = GameResourceManager.Instance.GetImage(iconName);
        txtTitle.text = title;
        txtDesc.text = desc;
    }


    void WaitTweenStart()
    {
        if (_coroutineShowPopup != null)
        {
            StopCoroutine(_coroutineShowPopup);    
        }
        _coroutineShowPopup = StartCoroutine(WaitTween());
    }
    IEnumerator WaitTween()
    {
        shorCutBtn.interactable = true;
        yield return new WaitForSeconds(2.0f);
        uiTweenPosition.Play(false);
        _tweenColorAlpha.Play(false, Hide);
    }
}
