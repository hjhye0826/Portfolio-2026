using System;
using UnityEngine;

public class PressurePlate : InteractableBase
{
    [SerializeField] private float requiredHoldDuration;

    public event Action<bool> OnStateChanged;

    private bool _isActivated;
    private float _currentHoldTime;

    private void Update()
    {
        if (false == _isActivated || IsCompleted) return;

        _currentHoldTime += Time.deltaTime;

        if (_currentHoldTime >= requiredHoldDuration)
            OnComplete();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (false == other.CompareTag("Player")) return;

        Activate();
    }

    private void OnTriggerExit(Collider other)
    {
        if (false == other.CompareTag("Player")) return;
        if (IsCompleted) return;

        _currentHoldTime = 0f;
        Deactivate();
    }

    private void Activate()
    {
        if (_isActivated) return;

        _isActivated = true;
        OnStateChanged?.Invoke(_isActivated);
    }

    private void Deactivate()
    {
        if (false == _isActivated) return;

        _isActivated = false;
        OnStateChanged?.Invoke(_isActivated);
    }

    protected override void OnComplete()
    {
        base.OnComplete();
        
        // ���� ���� ���� 
    }
}