using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Zoom")]
    [SerializeField] private float zoomSpeed = 2f;
    [SerializeField] private float minDistance = 2f;
    [SerializeField] private float maxDistance = 20f;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 180f;
    [SerializeField] private float minPitch = -20f;
    [SerializeField] private float maxPitch = 70f;

    private Transform _player;
    private float _yaw;
    private float _pitch;
    private float _distance;
    private float _orbitCenterOffsetY; // X, Z는 항상 플레이어 위치 고정

    public float Yaw => _yaw;

    private void Start()
    {
        var playerObj = GameObject.FindWithTag("Player");
        if (playerObj == null)
        {
            Debug.LogError("Player 태그를 가진 오브젝트를 찾을 수 없습니다.");
            enabled = false;
            return;
        }
        _player = playerObj.transform;

        _yaw = transform.eulerAngles.y;
        var rawPitch = transform.eulerAngles.x;
        _pitch = rawPitch > 180f ? rawPitch - 360f : rawPitch;

        var toPlayer = _player.position - transform.position;
        _distance = Mathf.Max(Vector3.Dot(toPlayer, transform.forward), 0.1f);

        var orbitCenter = transform.position + transform.forward * _distance;
        _orbitCenterOffsetY = orbitCenter.y - _player.position.y;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void LateUpdate()
    {
        if (_player == null) return;

        HandleZoom();
        HandleRotation();
        ApplyTransform();
    }

    private void HandleZoom()
    {
        var scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll == 0f) return;

        _distance = Mathf.Clamp(_distance - scroll * zoomSpeed, minDistance, maxDistance);
    }

    private void HandleRotation()
    {
        if (!Input.GetMouseButton(1)) return;

        _yaw += Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        _pitch -= Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
        _pitch = Mathf.Clamp(_pitch, minPitch, maxPitch);
    }

    private void ApplyTransform()
    {
        var rotation = Quaternion.Euler(_pitch, _yaw, 0f);
        var orbitCenter = new Vector3(_player.position.x, _player.position.y + _orbitCenterOffsetY, _player.position.z);

        transform.position = orbitCenter + rotation * Vector3.back * _distance;
        transform.rotation = rotation;
    }
}