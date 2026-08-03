using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ObjectKey -> InteractableBase 인스턴스들 해석용 런타임 레지스트리.
/// 하나의 키를 여러 인스턴스가 공유할 수 있다(카테고리/역할 의미).
/// 등록/해제는 InteractableBase의 OnEnable/OnDisable에서 수행된다.
/// </summary>
public static class ObjectRegistry
{
    private static readonly Dictionary<ObjectKey, List<InteractableBase>> _map = new();
    private static readonly List<InteractableBase> _empty = new();

    public static void Register(ObjectKey key, InteractableBase target)
    {
        if (key == null || target == null) return;
        if (!_map.TryGetValue(key, out var list))
        {
            list = new List<InteractableBase>();
            _map[key] = list;
        }
        if (!list.Contains(target)) list.Add(target);
    }

    public static void Unregister(ObjectKey key, InteractableBase target)
    {
        if (key == null) return;
        if (_map.TryGetValue(key, out var list))
        {
            list.Remove(target);
            if (list.Count == 0) _map.Remove(key);
        }
    }

    public static IReadOnlyList<InteractableBase> GetAll(ObjectKey key)
    {
        return key != null && _map.TryGetValue(key, out var list) ? list : _empty;
    }

    public static InteractableBase Get(ObjectKey key)
    {
        if (key != null && _map.TryGetValue(key, out var list) && list.Count > 0) return list[0];
        return null;
    }

    public static InteractableBase GetNearest(ObjectKey key, Vector3 position)
    {
        if (key == null || !_map.TryGetValue(key, out var list)) return null;

        InteractableBase nearest = null;
        float best = float.MaxValue;
        for (int i = 0; i < list.Count; i++)
        {
            var t = list[i];
            if (t == null) continue;
            float d = (t.transform.position - position).sqrMagnitude;
            if (d < best) { best = d; nearest = t; }
        }
        return nearest;
    }
}
