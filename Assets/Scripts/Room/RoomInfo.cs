using UnityEngine;

/// <summary>
/// 방(Room) 하나의 정보를 나타낸다.
/// Root_Rooms 하위 각 Room_N 오브젝트에 부착하며, RoomManager가 씬을 스캔해
/// 방 이동 목록(번호/이름/스폰 지점)을 구성하는 데 사용한다.
/// </summary>
public class RoomInfo : MonoBehaviour
{
    [SerializeField] private int _roomNumber = 1;
    [Tooltip("비워두면 오브젝트 이름을 표시 이름으로 사용한다.")]
    [SerializeField] private string _displayName;
    [Tooltip("방 이동 시 플레이어가 위치할 지점(문 앞). 비워두면 이 오브젝트의 위치를 사용한다.")]
    [SerializeField] private Transform _spawnPoint;

    public int RoomNumber => _roomNumber;
    public string DisplayName => string.IsNullOrEmpty(_displayName) ? gameObject.name : _displayName;
    public Transform SpawnPoint => _spawnPoint != null ? _spawnPoint : transform;
}
