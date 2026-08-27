using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 근접 기반 연결(링크) 퍼즐 노드.
/// 서로 _linkRange 이내에 있는 노드끼리는 자동으로 레이(LineRenderer)로 이어진다.
/// 단, 시각적 레이는 Source로부터 실제로 닿아 있는(reachable) 노드끼리의 구간에서만 표시된다.
/// Source 노드에서 출발해 Relay 노드를 거쳐 Receiver 노드까지 연결이 닿으면,
/// Receiver가 _targetKey로 지정된 문(Door)을 연다. 연결이 끊기면 다시 닫는다.
/// </summary>
public class LinkNode : InteractableBase
{
    public enum LinkRole { Source, Relay, Receiver }

    [Header("Link")]
    [SerializeField] private LinkRole _role;
    [SerializeField] private float _linkRange = 4f;

    [Header("Receiver 전용")]
    [SerializeField] private ObjectKey _targetKey;

    [Header("Visual")]
    [SerializeField] private Material _lineMaterial;
    [SerializeField] private float _lineWidth = 0.05f;
    [SerializeField] private Color _lineColor = Color.cyan;

    public LinkRole Role => _role;
    public override bool CanInteract => false;

    private readonly Dictionary<LinkNode, LineRenderer> _edgeLines = new();
    private List<LinkNode> _edgeRemoveBuffer;
    private bool _isConnected;

    protected override void OnEnable() { base.OnEnable(); LinkNetwork.Register(this); }
    protected override void OnDisable() { base.OnDisable(); LinkNetwork.Unregister(this); ClearAllEdges(); }

    private void Update()
    {
        RefreshEdgeVisuals();
        if (_role == LinkRole.Receiver) { RefreshConnection(); }
    }

    private void RefreshEdgeVisuals()
    {
        bool selfReachable = LinkNetwork.IsReachableFromSource(this);
        var nodes = LinkNetwork.Nodes;
        for (int i = 0; i < nodes.Count; i++)
        {
            var other = nodes[i];
            if (other == null || other == this) continue;
            if (other.GetInstanceID() <= GetInstanceID()) continue;
            bool shouldShow = selfReachable && LinkNetwork.IsReachableFromSource(other) && InRange(this, other);
            if (shouldShow) { DrawEdge(other); }
            else if (_edgeLines.ContainsKey(other)) { RemoveEdge(other); }
        }
        if (_edgeLines.Count == 0) return;
        if (_edgeRemoveBuffer == null) _edgeRemoveBuffer = new List<LinkNode>();
        _edgeRemoveBuffer.Clear();
        foreach (var kv in _edgeLines)
        {
            bool stillValid = kv.Key != null && selfReachable && LinkNetwork.IsReachableFromSource(kv.Key) && InRange(this, kv.Key);
            if (!stillValid) { _edgeRemoveBuffer.Add(kv.Key); }
        }
        for (int i = 0; i < _edgeRemoveBuffer.Count; i++) { RemoveEdge(_edgeRemoveBuffer[i]); }
    }

    public static bool InRange(LinkNode a, LinkNode b)
    {
        float range = Mathf.Min(a._linkRange, b._linkRange);
        return (a.transform.position - b.transform.position).sqrMagnitude <= range * range;
    }

    private void DrawEdge(LinkNode other)
    {
        LineRenderer lr;
        if (!_edgeLines.TryGetValue(other, out lr) || lr == null)
        {
            lr = MakeLineRenderer();
            _edgeLines[other] = lr;
        }
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, other.transform.position);
    }

    private LineRenderer MakeLineRenderer()
    {
        var go = new GameObject("LinkEdge");
        go.transform.SetParent(transform, false);
        var lr = go.AddComponent<LineRenderer>();
        lr.material = _lineMaterial != null ? _lineMaterial : new Material(Shader.Find("Sprites/Default"));
        lr.startWidth = _lineWidth;
        lr.endWidth = _lineWidth;
        lr.startColor = _lineColor;
        lr.endColor = _lineColor;
        lr.positionCount = 2;
        lr.useWorldSpace = true;
        return lr;
    }

    private void RemoveEdge(LinkNode key)
    {
        if (_edgeLines.TryGetValue(key, out var lr) && lr != null)
        {
            UnityEngine.Object.DestroyImmediate(lr.gameObject);
        }
        _edgeLines.Remove(key);
    }

    private void ClearAllEdges()
    {
        foreach (var kv in _edgeLines)
        {
            if (kv.Value != null) { UnityEngine.Object.DestroyImmediate(kv.Value.gameObject); }
        }
        _edgeLines.Clear();
    }

    private void RefreshConnection()
    {
        bool connected = LinkNetwork.IsReachableFromSource(this);
        if (connected == _isConnected) return;
        _isConnected = connected;
        var targets = ObjectRegistry.GetAll(_targetKey);
        for (int i = 0; i < targets.Count; i++)
        {
            var door = targets[i] as Door;
            if (door != null) { door.SetOpen(connected); }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Color c = _role == LinkRole.Source ? Color.green : (_role == LinkRole.Receiver ? Color.red : Color.yellow);
        Gizmos.color = c;
        Gizmos.DrawWireSphere(transform.position, _linkRange);
    }
}
