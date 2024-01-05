using System.Collections;
using System.Globalization;
using Lofle.Tween;
using Script.Manager;
using Script.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGameStart : UIBase
{

    [SerializeField] private DTButton startButton;
    [SerializeField] private Slider loadBar;
    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField] private UITweenColorAlpha _logoTween;
    [SerializeField] private Transform _logoParent;
    private Coroutine _loadCoroutine;


    private void Awake()
    {
        startButton.onClick.AddListener(OnClickStartButton);
        StartCoroutine(CoroutineHideLogo());
    }
    
    private void StartLoadScene()
    {
        if (_loadCoroutine != null)
        {
            StopCoroutine(_loadCoroutine);
        }
        _loadCoroutine = StartCoroutine(GameSceneManager.Instance.LoadScene("GameScene", OnEventProgressAction, OnEventFinishAction));
    }
    private void OnClickStartButton()
    {
        loadBar.maxValue = 100f;
        startButton.interactable = false;
        StartLoadScene();
    }

    private void OnEventFinishAction()
    {
        if (GameUIManager.Instance.TryGetOrCreate<UIPopUpStroy>(false, UILayer.LEVEL_1, out var ui))
        {
            ui.Show();
        }
        Hide();
    }

    private void OnEventProgressAction(float progress)
    {
        loadBar.value = progress;
        progressText.text = $"{progress.ToString(CultureInfo.InvariantCulture)}/{100.ToString()}";
    }

    private IEnumerator CoroutineHideLogo()
    {
        _logoTween.Play(true);
        yield return new WaitForSeconds(_logoTween.TotalTime);
        _logoParent.gameObject.SetActive(false);
    }
}
