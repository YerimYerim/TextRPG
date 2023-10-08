using UnityEngine;

public class UIStroyTextPanel : MonoBehaviour
{
    [SerializeField] private TextTyper _textTyper;
    [SerializeField] public CanvasGroup _canvas;
    public void SetText(string outputString)
    {
        _textTyper.TypeText(outputString);
        _canvas.alpha = 0;
    }
}
