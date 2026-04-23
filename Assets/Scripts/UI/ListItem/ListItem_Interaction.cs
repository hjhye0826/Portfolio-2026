using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ListItem_Interaction : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI _label;
    [SerializeField] private GameObject _highlight;

    private int _index;
    private Action<int> _onHover;
    private Action<int> _onClick;

    public void Init(string label, int index, Action<int> onHover = null, Action<int> onClick = null)
    {
        _label.text = label;
        _index = index;
        _onHover = onHover;
        _onClick = onClick;
        SetHighlight(false);
    }

    public void SetHighlight(bool on)
    {
        _highlight.SetActive(on);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _onHover?.Invoke(_index);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _onClick?.Invoke(_index);
    }
}
