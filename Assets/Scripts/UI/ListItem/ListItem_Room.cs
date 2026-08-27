using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ListItem_Room : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI _numberLabel;
    [SerializeField] private TextMeshProUGUI _nameLabel;

    private RoomInfo _room;
    private Action<RoomInfo> _onClick;

    public void Init(RoomInfo room, Action<RoomInfo> onClick)
    {
        _room = room;
        _onClick = onClick;

        if (_numberLabel != null) _numberLabel.text = room.RoomNumber.ToString();
        if (_nameLabel != null) _nameLabel.text = room.DisplayName;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _onClick?.Invoke(_room);
    }
}
