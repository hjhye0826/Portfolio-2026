using System.Collections;
using UnityEngine;

/// <summary>
/// 원격으로 열리는 문.
/// 직접 F 상호작용 대상이 아니며(CanInteract=false), 자신의 ObjectKey로
/// 레지스트리에만 등록되어 레버 등이 키로 찾아 연다.
/// 이 컴포넌트가 붙은 Transform(힌지)을 회전시켜 여닫는다.
/// </summary>
public class Door : InteractableBase, IToggleable
{
    [SerializeField] private float _openAngle = -90f;   // 열렸을 때 힌지 Y 회전량(도)
    [SerializeField] private float _speed = 240f;       // 회전 속도(도/초)
    [SerializeField] private bool _startOpen;

    private Quaternion _closedRot;
    private Quaternion _openRot;
    private bool _isOpen;
    private Coroutine _anim;

    public bool IsOpen => _isOpen;

    // 문은 레버로만 조작 → F 후보에서 제외
    public override bool CanInteract => false;

    protected override void OnAwake()
    {
        _closedRot = transform.localRotation;
        _openRot = _closedRot * Quaternion.Euler(0f, _openAngle, 0f);

        _isOpen = _startOpen;
        transform.localRotation = _isOpen ? _openRot : _closedRot;
    }

    public void Toggle() => SetOpen(!_isOpen);
    public void Open() => SetOpen(true);
    public void Close() => SetOpen(false);

    public void SetOpen(bool open)
    {
        _isOpen = open;

        if (_anim != null) StopCoroutine(_anim);

        if (isActiveAndEnabled)
            _anim = StartCoroutine(Animate(open ? _openRot : _closedRot));
        else
            transform.localRotation = open ? _openRot : _closedRot;
    }

    private IEnumerator Animate(Quaternion target)
    {
        while (Quaternion.Angle(transform.localRotation, target) > 0.05f)
        {
            transform.localRotation = Quaternion.RotateTowards(
                transform.localRotation, target, _speed * Time.deltaTime);
            yield return null;
        }
        transform.localRotation = target;
        _anim = null;
    }
}
