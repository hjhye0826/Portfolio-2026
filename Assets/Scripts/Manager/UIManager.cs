using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIManager
{
    private Transform _root;
    private List<UIView> _viewList = new();

    public void Init()
    {
        _root = GameObject.Find("Canvas").transform;
    }

    public T Show<T>() where T : UIView
    {
        var ui = GetOrCreate<T>();
        ui.Show();

        return ui;
    }

    public void Hide<T>() where T : UIView
    {
        var ui = _viewList.OfType<T>().FirstOrDefault();
        ui?.Hide();
    }

    public bool IsVisible<T>() where T : UIView
    {
        var ui = _viewList.OfType<T>().FirstOrDefault();
        return ui != null && ui.IsVisible;
    }

    private T GetOrCreate<T>() where T : UIView
    {
        var ui = _viewList.OfType<T>().FirstOrDefault();
        if (ui != null) return ui;

        var typeName = typeof(T).Name;
        var underscore = typeName.IndexOf('_');
        var path = underscore > 0
            ? $"Prefabs/UI/{typeName.Substring(0, underscore)}/{typeName}"
            : $"Prefabs/UI/{typeName}";
        var prefab = Resources.Load<T>(path);
        var instance = Object.Instantiate(prefab, _root);
        _viewList.Add(instance);

        return instance;
    }
}