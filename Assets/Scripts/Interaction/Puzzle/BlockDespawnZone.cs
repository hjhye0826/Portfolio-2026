using UnityEngine;

/// <summary>
/// 구멍 아래에 두는 트리거 볼륨. 떨어진 블록이 들어오면 풀로 반환한다.
/// 들고 있는 블록(IsCarried)은 무시한다. Collider 는 IsTrigger 여야 한다.
/// </summary>
[RequireComponent(typeof(Collider))]
public class BlockDespawnZone : MonoBehaviour
{
    [SerializeField] private BlockPool _pool;

    private void OnTriggerEnter(Collider other)
    {
        if (_pool == null) return;

        var block = other.GetComponentInParent<CarryableBlock>();
        if (block == null) return;
        if (block.IsCarried) return;

        _pool.Release(block.gameObject);
    }
}
