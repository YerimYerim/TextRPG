using UnityEngine;

public class UIStroyTextPanel : MonoBehaviour
{
    [SerializeField] private TextTyper _textTyper;
    [SerializeField] public CanvasGroup _canvas;
    public void SetText(string outputString, int canvasAlpha = 0)
    {
        _textTyper.TypeText(outputString);
        _canvas.alpha = canvasAlpha;
    }
    public LTDescr SetTween(float delay, float tweenTime)
    {
        return LeanTween.alphaCanvas(_canvas, 1.0f, tweenTime).setDelay(delay).setEase(LeanTweenType.animationCurve).setOnComplete(
            () =>
            {
                _canvas.alpha = 1;
            });
    }
}
