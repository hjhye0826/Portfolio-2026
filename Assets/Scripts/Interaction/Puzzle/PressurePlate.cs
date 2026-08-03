using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어 또는 (들려있지 않은) 블럭이 위에 있으면 눌리는 발판.
/// _latch=true  : requiredHoldDuration 유지 후 OnComplete로 _targetKey 문을 열고 잠금(단독형).
/// _latch=false : 올라가 있는 동안만 눌림(순간형). 여러 발판을 PlateGate로 묶을 때 사용.
/// 상태 변화는 OnStateChanged로 통지. 트리거 기반이라 F 대상은 아니다(CanInteract=false).
/// </summary>
public class PressurePlate : InteractableBase
{
    [SerializeField] private float requiredHoldDuration;
    [SerializeField] private ObjectKey _targetKey;   // (_latch 전용) 열 대상 문의 키
    [SerializeField] private bool _latch = true;     // true=단독 래치형, false=순간형(그룹용)

    [Header("Press Visual")]
    [SerializeField] private float _pressDepth = 0.07f;   // 밟았을 때 로컬 Y로 내려가는 깊이
    [SerializeField] private float _pressSpeed = 0.5f;    // 눌림/복귀 속도(유닛/초)

    private Vector3 _restLocalPos;

    public event Action<bool> OnStateChanged;

    public bool IsPressed => _occupants.Count > 0;

    private readonly HashSet<Collider> _occupants = new HashSet<Collider>();
    private bool _isActivated;
    private float _currentHoldTime;

    public override bool CanInteract => false;

    protected override void OnAwake()
    {
        _restLocalPos = transform.localPosition;
    }

    private void Update()
    {
        UpdatePressVisual();

        if (!_latch) return;
        if (!_isActivated || IsCompleted) return;

        _currentHoldTime += Time.deltaTime;
        if (_currentHoldTime >= requiredHoldDuration)
            OnComplete();
    }

    // 밟혀 있으면 _pressDepth 만큼 내려가고, 벗어나면 원위치로 복귀
    private void UpdatePressVisual()
    {
        float targetY = _restLocalPos.y - (IsPressed ? _pressDepth : 0f);
        var lp = transform.localPosition;
        lp.y = Mathf.MoveTowards(lp.y, targetY, _pressSpeed * Time.deltaTime);
        transform.localPosition = lp;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsWeight(other)) return;
        if (!_occupants.Add(other)) return;
        if (_occupants.Count == 1) Activate();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!_occupants.Remove(other)) return;
        if (_occupants.Count > 0) return;

        if (_latch && IsCompleted) return; // 래치 완료 시 유지
        _currentHoldTime = 0f;
        Deactivate();
    }

    // 유효 무게 = 플레이어 또는 (들려있지 않은) 블럭
    private bool IsWeight(Collider other)
    {
        if (other.CompareTag("Player")) return true;
        var block = other.GetComponentInParent<CarryableBlock>();
        return block != null && !block.IsCarried;
    }

    private void Activate()
    {
        if (_isActivated) return;
        _isActivated = true;
        OnStateChanged?.Invoke(true);
    }

    private void Deactivate()
    {
        if (!_isActivated) return;
        _isActivated = false;
        OnStateChanged?.Invoke(false);
    }

    protected override void OnComplete()
    {
        base.OnComplete();
        OpenTargets();
    }

    private void OpenTargets()
    {
        var targets = ObjectRegistry.GetAll(_targetKey);
        for (int i = 0; i < targets.Count; i++)
            if (targets[i] is Door door)
                door.Open();
    }
}
