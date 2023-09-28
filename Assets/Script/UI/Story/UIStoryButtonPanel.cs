using Script.DataClass;
using UnityEngine;

namespace Script.UI.Story
{
    public class UIStoryButtonPanel : MonoBehaviour
    {
        [SerializeField] private UIStoryButton[] _ChoiceButton;

        private void SetButton(ScenarioData scenarioDataBase, int index)
        {
            if (index < _ChoiceButton.Length)
            {
                _ChoiceButton[index].button.onClick.RemoveAllListeners();
                _ChoiceButton[index].text.text = scenarioDataBase.output_txt;
            }
        }

        public void SetButtonPanel(ScenarioData[] scenarioDataBase)
        {
            foreach (var buttons in _ChoiceButton)
            {
                buttons.gameObject.SetActive(false);
            }

            for (int i = 0; i < scenarioDataBase.Length; ++i)
            {
                SetButton(scenarioDataBase[i], i);
            }
        }
    }
}
