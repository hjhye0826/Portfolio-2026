using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 씬의 모든 RoomInfo를 스캔해 방 목록을 관리하고, 플레이어를 지정한 방의
/// 스폰 지점으로 순간이동시킨다.
/// </summary>
public class RoomManager
{
    private const string RoomsRootName = "Root_Rooms";

    private readonly List<RoomInfo> _rooms = new();

    public IReadOnlyList<RoomInfo> Rooms => _rooms;

    public void Init()
    {
        _rooms.Clear();

        var root = GameObject.Find(RoomsRootName);
        if (root == null)
        {
            Debug.LogWarning($"RoomManager: '{RoomsRootName}' 오브젝트를 찾을 수 없습니다.");
            return;
        }

        _rooms.AddRange(root.GetComponentsInChildren<RoomInfo>(true).OrderBy(r => r.RoomNumber));
    }

    public void MoveToRoom(RoomInfo room)
    {
        if (room == null) return;

        var player = Manager.Game?.Player;
        if (player == null)
        {
            Debug.LogWarning("RoomManager: Player를 찾을 수 없어 이동할 수 없습니다.");
            return;
        }

        var spawn = room.SpawnPoint;

        // CharacterController가 붙어 있으면 순간이동 중 충돌 보정으로 위치가
        // 밀리는 것을 막기 위해 잠깐 꺼두고 위치를 옮긴다.
        var controller = player.GetComponent<CharacterController>();
        if (controller != null) controller.enabled = false;

        player.SetPositionAndRotation(spawn.position, spawn.rotation);

        if (controller != null) controller.enabled = true;
    }

    public void MoveToRoom(int roomNumber)
    {
        MoveToRoom(_rooms.FirstOrDefault(r => r.RoomNumber == roomNumber));
    }
}
