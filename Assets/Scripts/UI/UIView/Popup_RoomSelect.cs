using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 우측 상단 아이콘으로 여는 방 이동 팝업.
/// 스크롤 가능한 방 목록을 보여주고, 항목을 클릭하면 해당 방의 스폰 지점으로
/// 플레이어를 순간이동시킨 뒤 팝업을 닫는다.
/// </summary>
public class Popup_RoomSelect : UIView
{
    [SerializeField] private ListItem_Room _listItemPrefab;
    [SerializeField] private Transform _listContainer;
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private Button _closeButton;

    private readonly List<ListItem_Room> _items = new();

    private void Awake()
    {
        if (_closeButton != null)
            _closeButton.onClick.AddListener(Hide);
    }

    protected override void OnShow()
    {
        PopulateRoomList();
        Manager.Game.LockMovement();
    }

    protected override void OnHide()
    {
        ClearRoomList();
        Manager.Game.UnlockMovement();
    }

    private void PopulateRoomList()
    {
        ClearRoomList();

        var rooms = Manager.Room.Rooms;
        for (var i = 0; i < rooms.Count; i++)
        {
            var item = Instantiate(_listItemPrefab, _listContainer);
            item.Init(rooms[i], OnRoomClicked);
            _items.Add(item);
        }

        if (_scrollRect != null)
            _scrollRect.verticalNormalizedPosition = 1f;
    }

    private void ClearRoomList()
    {
        foreach (var item in _items)
            if (item != null) Destroy(item.gameObject);
        _items.Clear();
    }

    private void OnRoomClicked(RoomInfo room)
    {
        Hide();
        Manager.Room.MoveToRoom(room);
    }
}
