using System;
using Script.Manager;
using UnityEngine;
using UnityEngine.UI;

public class UIStoryImagePanel : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] public CanvasGroup _canvas;
    public void SetImage(string imgName)
    {
        _image.sprite = GameResourceManager.Instance.GetImage(imgName);
        _canvas.alpha = 0;
    }
}
