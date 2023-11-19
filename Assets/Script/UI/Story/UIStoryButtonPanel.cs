using System;
using Script.DataClass;
using Script.Manager;
using UnityEngine;

namespace Script.UI.Story
{
    public class UIStoryButtonPanel : MonoBehaviour
    {
        [SerializeField] private UIStoryButton _ChoiceButton;
        [SerializeField] public CanvasGroup _canvas;
        private Action _onClickAction;
        public void SetButton(ScenarioData scenarioDataBase, Action onClickAction)
        {
            _ChoiceButton.button.onClick.RemoveAllListeners();
            _onClickAction = onClickAction;
            
            _ChoiceButton.button.onClick.AddListener(OnClickButton);
            _ChoiceButton.text.text = scenarioDataBase.output_txt;
            _canvas.alpha = 0;
        }
        public void SetButton(string text, Action onClickAction)
        {
            _ChoiceButton.button.onClick.RemoveAllListeners();
            _onClickAction = onClickAction;
            
            _ChoiceButton.button.onClick.AddListener(OnClickButton);
            _ChoiceButton.text.text = text;
            _canvas.alpha = 0;
        }
        private void OnClickButton()
        {
            _onClickAction?.Invoke();
        }
        public LTDescr SetTween(int delay, float tweenTime)
        {
            return LeanTween.alphaCanvas(_canvas, 1.0f, tweenTime).setDelay(delay).setEase(LeanTweenType.animationCurve).setOnComplete(
                () =>
                {
                    _canvas.alpha = 1;
                });
        }
    }
}
