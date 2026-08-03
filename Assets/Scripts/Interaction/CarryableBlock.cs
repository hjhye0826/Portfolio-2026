using UnityEngine;

/// <summary>
/// F로 들어올리고 다시 F로 내려놓는 블럭. 드는 동안 머리 위에 고정된다.
/// 내려놓아 발판 위에 두면 그 발판을 눌러준다.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class CarryableBlock : InteractableBase
{
    [SerializeField] private Vector3 _holdLocalOffset = new Vector3(0f, 2.0f, 0f);
    [SerializeField] private float _dropForward = 1.2f;

    public bool IsCarried { get; private set; }

    private Rigidbody _rb;
    private Transform _carrier;

    public override string DisplayName => IsCarried ? "block (Carried)" : "block";

    protected override void OnAwake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    public override void OnInteract(GameObject interactor)
    {
        if (!IsCarried) PickUp(interactor);
        else Drop();
    }

    private void PickUp(GameObject interactor)
    {
        IsCarried = true;
        _carrier = interactor.transform;

        _rb.isKinematic = true;
        transform.SetParent(_carrier, false);
        transform.localPosition = _holdLocalOffset;
        transform.localRotation = Quaternion.identity;
    }

    private void Drop()
    {
        IsCarried = false;
        transform.SetParent(null, true);

        Vector3 pos = _carrier.position + _carrier.forward * _dropForward;
        pos.y = 1.0f;   // 발판 윗면(0.2)보다 높은 위치에서 낙하시켜 관통/박힘 방지
        transform.position = pos;
        transform.rotation = Quaternion.identity;

        _rb.isKinematic = false;
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;

        _carrier = null;
    }
}
