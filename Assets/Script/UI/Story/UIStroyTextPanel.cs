using UnityEngine;

public class UIStroyTextPanel : MonoBehaviour
{
    [SerializeField] private TextTyper _textTyper;

    public void SetText(string outputString)
    {
        _textTyper.TypeText(outputString);
    }
}
