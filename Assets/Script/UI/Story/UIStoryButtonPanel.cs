using System;
using Script.DataClass;
using UnityEngine;

namespace Script.UI.Story
{
    public class UIStoryButtonPanel : MonoBehaviour
    {
        [SerializeField] private UIStoryButton _ChoiceButton;
        private Action _onClickAction;
        public void SetButton(ScenarioData scenarioDataBase, Action onClickAction)
        {
            _ChoiceButton.button.onClick.RemoveAllListeners();
            _onClickAction = onClickAction;
            
            _ChoiceButton.button.onClick.AddListener(()=>OnClickButton(scenarioDataBase));
            _ChoiceButton.text.text = scenarioDataBase.output_txt;
        }

        private void OnClickButton(ScenarioData scenarioDataBase)
        {
            _onClickAction?.Invoke();
        }
    }
}
