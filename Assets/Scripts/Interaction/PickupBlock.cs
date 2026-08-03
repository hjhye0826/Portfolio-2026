using UnityEngine;

/// <summary>
/// F로 집어 들고 다니다 다시 F로 내려놓는 블럭.
/// 들고 있는 동안 콜라이더를 트리거로 바꿔 이동을 막지 않으면서 F 감지는 유지.
/// 내려놓으면 물리로 떨어져 발판 위에 얹힌다.
/// </summary>
public class PickupBlock : InteractableBase
{
    [SerializeField] private float _holdForward = 1.2f;
    [SerializeField] private float _holdHeight = 1.0f;

    public bool IsHeld { get; private set; }

    private Rigidbody _rb;
    private Collider _col;
    private Transform _origParent;

    protected override void OnAwake()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<Collider>();
        _origParent = transform.parent;
    }

    public override void OnInteract(GameObject interactor)
    {
        if (!IsHeld) PickUp(interactor);
        else Drop();
    }

    private void PickUp(GameObject interactor)
    {
        IsHeld = true;
        if (_rb != null) _rb.isKinematic = true;
        if (_col != null) _col.isTrigger = true;

        var hold = interactor.transform.Find("HoldPoint");
        if (hold != null)
        {
            transform.SetParent(hold, true);
            transform.localPosition = Vector3.zero;
        }
        else
        {
            transform.SetParent(interactor.transform, true);
            transform.localPosition = new Vector3(0f, _holdHeight, _holdForward);
        }
        transform.localRotation = Quaternion.identity;
    }

    private void Drop()
    {
        IsHeld = false;
        transform.SetParent(_origParent, true);
        if (_col != null) _col.isTrigger = false;
        if (_rb != null) _rb.isKinematic = false;
    }
}
