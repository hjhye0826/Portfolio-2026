using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGroup_InteractionSelection : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private ListItem_Interaction _listItemPrefab;
    [SerializeField] private Transform _listContainer;
    [SerializeField] private ScrollRect _scrollRect;

    private readonly List<ListItem_Interaction> _items = new();

    public void Show(List<InteractableBase> candidates)
    {
        Hide();
        _panel.SetActive(true);

        for (var i = 0; i < candidates.Count; i++)
        {
            var item = Instantiate(_listItemPrefab, _listContainer);
            item.Init(candidates[i].DisplayName, i);
            _items.Add(item);
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

        EnsureVisible(index);
    }

    private void EnsureVisible(int index)
    {
        if (_scrollRect == null || _items.Count <= 1) return;
        if (index < 0 || index >= _items.Count) return;

        Canvas.ForceUpdateCanvases();

        var content = _scrollRect.content;
        var viewport = _scrollRect.viewport;
        if (content == null || viewport == null) return;

        // 모든 항목이 뷰포트 안에 들어오면 스크롤 불필요
        if (content.rect.height <= viewport.rect.height) return;

        // 인덱스를 세로 정규화 위치로 매핑 (0번=최상단, 마지막=최하단)
        var t = (float)index / (_items.Count - 1);
        _scrollRect.verticalNormalizedPosition = Mathf.Clamp01(1f - t);
    }
}
