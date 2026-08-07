using UnityEngine;

/// <summary>
/// 한 번에 블록 1개만 유지한다. 스폰한 블록이 사라져(풀로 회수돼 비활성) 있을 때만,
/// _interval 만큼 기다린 뒤 새 블록을 떨어뜨린다.
/// 구멍이 닫혀 블록이 덮개 위에 남아 있으면(활성 유지) 새로 스폰하지 않는다.
/// </summary>
public class BlockSpawner : MonoBehaviour
{
    [SerializeField] private BlockPool _pool;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _interval = 1f;   // 사라진 뒤 다음 블록까지 대기(초)

    private GameObject _current;
    private float _timer;

    private void Update()
    {
        if (_pool == null || _spawnPoint == null) return;

        // 현재 블록이 살아있으면(낙하 중이거나 덮개/발판 위) 아무것도 안 함
        if (_current != null && _current.activeInHierarchy)
        {
            _timer = 0f;
            return;
        }

        _timer += Time.deltaTime;
        if (_timer < _interval) return;

        _timer = 0f;
        _current = _pool.Get(_spawnPoint.position, Quaternion.identity);
    }
}
