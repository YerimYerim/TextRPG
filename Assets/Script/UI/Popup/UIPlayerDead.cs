using System.Collections;
using System.Globalization;
using Lofle.Tween;
using Script.Manager;
using Script.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerDead : UIBase
{
    [SerializeField] private DTButton restartButton;
    [SerializeField] private UITweenColorAlpha _logoTween;
    private Coroutine _loadCoroutine;


    protected override void OnShow(params object[] param)
    {
        base.OnShow(param);
        restartButton.onClick.RemoveAllListeners();
        restartButton.onClick.AddListener(OnClickStartButton);
        
        StartCoroutine(CoroutineShowLogo());
    }

    private void StartLoadScene()
    {
        if (_loadCoroutine != null)
        {
            StopCoroutine(_loadCoroutine);
        }
        _loadCoroutine = StartCoroutine(GameSceneManager.Instance.LoadScene("GameStartScene", null, OnEventFinishAction));
    }
    private void OnClickStartButton()
    {
        StartLoadScene();
    }

    private void OnEventFinishAction()
    {
        GameUIManager.Instance.Clear();
        if (GameUIManager.Instance.TryGetOrCreate<UIGameStart>(false, UILayer.LEVEL_1, out var ui))
        {
            ui.Show();
        }
    }
    private IEnumerator CoroutineShowLogo()
    {
        _logoTween.Play(true);
        restartButton.interactable = false;
        yield return new WaitForSeconds(_logoTween.TotalTime);
        restartButton.interactable = true;
    }
    
}
