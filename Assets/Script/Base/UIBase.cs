using UnityEngine;

public class UIBase : MonoBehaviour
{
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
    }
}
