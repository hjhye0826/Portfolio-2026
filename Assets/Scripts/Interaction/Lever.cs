using UnityEngine;

/// <summary>
/// F 상호작용 시 _targetKey에 해당하는 문(Door)들을 토글하는 레버.
/// 씬 직접 참조 없이 ObjectRegistry로 대상을 해석한다. 반복 사용 가능.
/// </summary>
public class Lever : InteractableBase
{
    [SerializeField] private ObjectKey _targetKey;      // 조작할 대상(문)의 키
    [SerializeField] private Transform _handle;         // 시각 연출용 손잡이(선택)
    [SerializeField] private float _handlePitch = 40f;  // 손잡이 기울기(도)

    private bool _on;

    public override string DisplayName => _on ? "레버 (켜짐)" : "레버 (꺼짐)";

    public override void OnInteract(GameObject interactor)
    {
        _on = !_on;

        var targets = ObjectRegistry.GetAll(_targetKey);
        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i] is Door door)
                door.Toggle();
        }

        if (_handle != null)
            _handle.localRotation = Quaternion.Euler(_on ? _handlePitch : -_handlePitch, 0f, 0f);
    }
}
