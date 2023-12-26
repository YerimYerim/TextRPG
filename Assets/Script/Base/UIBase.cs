using System;
using UnityEngine;

public class UIBase : MonoBehaviour
{
    public event Action onEventHide;
    protected virtual void OnShow(params object[] param)
    {
        
    }

    public virtual void Show()
    {
        transform.gameObject.SetActive(true);
        OnShow();
    }

    protected virtual void OnHide(params object[] param)
    {
        
    }

    public virtual void Hide()
    {
        transform.gameObject.SetActive(false);
        OnHide();
        onEventHide?.Invoke();
    }
}
