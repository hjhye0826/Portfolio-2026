using UnityEngine;

public abstract class InteractableBase : MonoBehaviour
{
    protected bool IsCompleted;

    private void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("InteractionBase");
        OnAwake();
    }

    protected virtual void OnAwake() { }
    protected virtual void OnComplete() { IsCompleted = true; }

}
