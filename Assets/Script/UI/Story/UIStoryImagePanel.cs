using System;
using Script.Manager;
using UnityEngine;
using UnityEngine.UI;

public class UIStoryImagePanel : MonoBehaviour
{
    [SerializeField] private Image _image;
    
    public void SetImage()
    {
        _image.sprite = GameResourceManager.Instance.GetImage("SampleSprite");
    }
}
