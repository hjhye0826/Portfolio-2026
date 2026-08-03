using UnityEngine;

public abstract class InteractableBase : MonoBehaviour
{
    [SerializeField] private ObjectKey _key;

    protected bool IsCompleted;

    public ObjectKey Key => _key;
    public virtual bool CanInteract => !IsCompleted;
    public virtual string DisplayName => gameObject.name;

    private void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("Interactable");
        OnAwake();
    }

    protected virtual void OnEnable() => ObjectRegistry.Register(_key, this);
    protected virtual void OnDisable() => ObjectRegistry.Unregister(_key, this);

    public virtual void OnInteract(GameObject interactor) { }

    protected virtual void OnAwake() { }
    protected virtual void OnComplete() { IsCompleted = true; }
}
