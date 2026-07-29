using System.Collections.Generic;
using UnityEngine;

public class UIGroup_InteractionSelection : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private ListItem_Interaction _listItemPrefab;
    [SerializeField] private Transform _listContainer;

    private readonly List<ListItem_Interaction> _items = new();

    public void Show(List<InteractableBase> candidates)
    {
        Hide();
        _panel.SetActive(true);

        for (var i = 0; i < candidates.Count; i++)
        {
            //var item = Instantiate(_listItemPrefab, _listContainer);
            //item.Init(candidates[i].InteractionPrompt, i);
            //_items.Add(item);
        }
    }

    public void Hide()
    {
        foreach (var item in _items)
            Destroy(item.gameObject);
        _items.Clear();
        _panel.SetActive(false);
    }

    public void Refresh(int index)
    {
        for (var i = 0; i < _items.Count; i++)
            _items[i].SetHighlight(i == index);
    }
}
