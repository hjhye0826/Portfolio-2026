using UnityEngine;

public class SimpleInteractable : InteractableBase
{
    [SerializeField] private string _label = "SimpleInteractable";

    private int _interactCount;

    public override void OnInteract(GameObject interactor)
    {
        _interactCount++;
        Debug.Log($"[Interact] '{_label}' interacted by '{interactor.name}' (count={_interactCount})");
    }
}
