using Script.DataClass;
using Script.Manager;
using UnityEngine;

namespace Script.UI.Story
{
    public class UIStoryButtonPanel : MonoBehaviour
    {
        [SerializeField] private UIStoryButton[] _ChoiceButton;

        private void SetButton(ScenarioDataBase scenarioDataBase, int index)
        {
            if (index < _ChoiceButton.Length)
            {
                _ChoiceButton[index].button.onClick.RemoveAllListeners();
            }
        }

        public void SetUI()
        {
        }
    }
}
