using UnityEngine;

/// <summary>
/// _plateGroupKey로 묶인 발판들을 감시해, (_requireAll 기준) 조건을 만족하는 동안만 _targetKey의 문을 연다.
/// 래치 없음: 발판 위에 물체나 플레이어가 있으면 문을 열고, 모두 벗어나면 문을 다시 닫는다.
/// </summary>
public class PlateGate : MonoBehaviour
{
    [SerializeField] private ObjectKey _plateGroupKey;
    [SerializeField] private ObjectKey _targetKey;
    [SerializeField] private bool _requireAll = true;

    private bool _isOpen;
    private bool _initialized;

    private void Update()
    {
        var plates = ObjectRegistry.GetAll(_plateGroupKey);
        int total = 0, pressed = 0;
        for (int i = 0; i < plates.Count; i++)
        {
            if (plates[i] is PressurePlate p)
            {
                total++;
                if (p.IsPressed) pressed++;
            }
        }
        if (total == 0) return;

        bool ok = _requireAll ? pressed == total : pressed > 0;

        // 상태가 바뀔 때만 문에 명령을 보내 애니메이션이 매 프레임 재시작되는 것을 막는다.
        if (_initialized && ok == _isOpen) return;
        _initialized = true;
        _isOpen = ok;

        var doors = ObjectRegistry.GetAll(_targetKey);
        for (int i = 0; i < doors.Count; i++)
            if (doors[i] is Door door)
                door.SetOpen(ok);
    }
}
