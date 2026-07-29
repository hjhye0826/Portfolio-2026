using UnityEngine;

public abstract class UIView : MonoBehaviour
{
    public bool IsVisible { get; private set; }

    public virtual void Show()
    {
        IsVisible = true;
        gameObject.SetActive(true);

        OnShow();
    }

    public virtual void Hide()
    {
        IsVisible = false;
        gameObject.SetActive(false);

        OnHide();
    }

    protected virtual void OnShow() { }
    protected virtual void OnHide() { }
}