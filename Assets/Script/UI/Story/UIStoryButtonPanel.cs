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
            
            _ChoiceButton.button.onClick.AddListener(()=>OnClickButton(scenarioDataBase));
            _ChoiceButton.text.text = scenarioDataBase.output_txt;
            _canvas.alpha = 0;
        }

        private void OnClickButton(ScenarioData scenarioDataBase)
        {
            GamePageManager.Instance.NextDataEnqueue(scenarioDataBase);
            _onClickAction?.Invoke();
        }
    }
}
