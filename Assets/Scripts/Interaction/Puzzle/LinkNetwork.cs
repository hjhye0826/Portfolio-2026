using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 씬에 존재하는 모든 LinkNode를 추적하는 런타임 레지스트리.
/// ObjectRegistry(키 기반 조회)와 달리 "현재 활성화된 노드 전체 목록"을 제공하고,
/// Source로부터 실제로 닿아 있는(reachable) 노드 집합을 프레임당 한 번만 계산해 캐싱한다.
/// 이 reachable 집합은 LinkNode의 문 개폐 판정과 레이 시각화 둘 다에 공유되어,
/// "Source에서부터 실제로 이어진 구간만" 레이가 표시되도록 보장한다.
/// 등록/해제는 LinkNode의 OnEnable/OnDisable에서 수행된다.
/// </summary>
public static class LinkNetwork
{
    private static readonly List<LinkNode> _nodes = new();
    private static readonly HashSet<LinkNode> _reachable = new();
    private static int _reachableComputedFrame = -1;

    public static IReadOnlyList<LinkNode> Nodes => _nodes;

    public static void Register(LinkNode node)
    {
        if (node != null && !_nodes.Contains(node))
            _nodes.Add(node);
    }

    public static void Unregister(LinkNode node)
    {
        _nodes.Remove(node);
    }

    /// <summary>Source로부터 실제로 연결이 닿는 노드인지(Source 자신 포함).</summary>
    public static bool IsReachableFromSource(LinkNode node)
    {
        EnsureReachableComputed();
        return node != null && _reachable.Contains(node);
    }

    private static void EnsureReachableComputed()
    {
        int frame = Time.frameCount;
        if (frame == _reachableComputedFrame) return;
        _reachableComputedFrame = frame;

        _reachable.Clear();
        var queue = new Queue<LinkNode>();

        for (int i = 0; i < _nodes.Count; i++)
        {
            var n = _nodes[i];
            if (n != null && n.Role == LinkNode.LinkRole.Source && _reachable.Add(n))
                queue.Enqueue(n);
        }

        while (queue.Count > 0)
        {
            var cur = queue.Dequeue();
            for (int i = 0; i < _nodes.Count; i++)
            {
                var other = _nodes[i];
                if (other == null || _reachable.Contains(other)) continue;
                if (!LinkNode.InRange(cur, other)) continue;

                _reachable.Add(other);
                queue.Enqueue(other);
            }
        }
    }
}
