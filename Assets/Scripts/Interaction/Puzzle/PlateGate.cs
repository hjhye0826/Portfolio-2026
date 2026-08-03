using UnityEngine;

/// <summary>
/// _plateGroupKey로 묶인 발판들을 감시해, (기본) 전부 눌리면 _targetKey의 문을 연다.
/// 한 번 열리면 유지(래치)라 발판에서 내려와도 다음 방으로 넘어갈 수 있다.
/// </summary>
public class PlateGate : MonoBehaviour
{
    [SerializeField] private ObjectKey _plateGroupKey;
    [SerializeField] private ObjectKey _targetKey;
    [SerializeField] private bool _requireAll = true;

    private bool _opened;

    private void Update()
    {
        if (_opened) return;

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
        if (!ok) return;

        _opened = true;
        var doors = ObjectRegistry.GetAll(_targetKey);
        for (int i = 0; i < doors.Count; i++)
            if (doors[i] is Door door)
                door.Open();
    }
}
