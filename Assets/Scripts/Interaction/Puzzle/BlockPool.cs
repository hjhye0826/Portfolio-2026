using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CarryableBlock 오브젝트 풀. 시작 시 _size 개를 미리 생성해 비활성으로 보관하고,
/// Get()으로 꺼내 활성화, Release()로 되돌린다.
/// 활성 블록이 _size 에 도달하면(모두 사용 중) Get()은 null 을 반환해 무한 적재를 막는다.
/// _passThroughFloor 가 지정되면 각 블록 콜라이더와 해당 바닥의 충돌을 무시해
/// 블록만 그 바닥을 통과하게 한다(플레이어는 CharacterController 로 그대로 충돌).
/// </summary>
public class BlockPool : MonoBehaviour
{
    [SerializeField] private GameObject _blockPrefab;
    [SerializeField] private int _size = 8;
    [SerializeField] private Transform _inactiveParent;   // 비활성 블록 보관 위치(비우면 자기 자신)
    [SerializeField] private Collider _passThroughFloor;  // 블록만 통과시킬 바닥(플레이어는 충돌 유지)

    private readonly Queue<GameObject> _idle = new Queue<GameObject>();

    private void Awake()
    {
        if (_inactiveParent == null) _inactiveParent = transform;
        if (_blockPrefab == null) return;

        for (int i = 0; i < _size; i++)
        {
            var go = Instantiate(_blockPrefab, _inactiveParent);
            go.name = _blockPrefab.name + "_" + i;

            if (_passThroughFloor != null)
            {
                var col = go.GetComponent<Collider>();
                if (col != null) Physics.IgnoreCollision(col, _passThroughFloor, true);
            }

            go.SetActive(false);
            _idle.Enqueue(go);
        }
    }

    /// <summary>사용 가능한 블록이 있으면 활성화해 반환, 없으면 null.</summary>
    public GameObject Get(Vector3 position, Quaternion rotation)
    {
        if (_idle.Count == 0) return null;

        var go = _idle.Dequeue();
        go.transform.SetParent(null, true);
        go.transform.SetPositionAndRotation(position, rotation);

        var rb = go.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        go.SetActive(true);
        return go;
    }

    /// <summary>블록을 비활성화해 풀로 되돌린다.</summary>
    public void Release(GameObject go)
    {
        if (go == null) return;

        var rb = go.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        go.SetActive(false);
        go.transform.SetParent(_inactiveParent, false);

        if (!_idle.Contains(go)) _idle.Enqueue(go);
    }
}
