using UnityEngine;
using UnityEngine.InputSystem;

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

    [Header("Terrain Collision")]
    [Tooltip("terrain 표면 위로 유지할 최소 높이")]
    [SerializeField] private float groundClearance = 0.5f;

    private Transform _player;
    private float _yaw;
    private float _pitch;
    private float _distance;
    private float _orbitCenterOffsetY; // X, Z는 항상 플레이어 위치 기준

    // PlayerInput (Send Messages) 에서 채워주는 입력 상태
    private Vector2 _lookDelta;       // 마우스 델타 (프레임당 누적)
    private float _zoomInput;         // 휠 (프레임당 누적)

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

    // PlayerInput 메시지 콜백: Camera 액션맵의 "Look" 액션 (마우스 델타)
    private void OnLook(InputValue value)
    {
        _lookDelta += value.Get<Vector2>();
    }

    // PlayerInput 메시지 콜백: Camera 액션맵의 "Zoom" 액션 (휠)
    private void OnZoom(InputValue value)
    {
        _zoomInput += value.Get<float>();
    }

    private void LateUpdate()
    {
        if (_player == null) return;

        HandleZoom();
        HandleRotation();
        ApplyTransform();

        // 누적 입력은 프레임 끝에 초기화
        _lookDelta = Vector2.zero;
        _zoomInput = 0f;
    }

    private void HandleZoom()
    {
        if (_zoomInput == 0f) return;

        // 기존 코드 명세 일치: GetAxis("Mouse ScrollWheel")는 몇 단위이므로 축소 계수 보정
        var scroll = _zoomInput * 0.01f;
        _distance = Mathf.Clamp(_distance - scroll * zoomSpeed, minDistance, maxDistance);
    }

    private void HandleRotation()
    {
        _yaw   += _lookDelta.x * rotationSpeed * Time.deltaTime;
        _pitch -= _lookDelta.y * rotationSpeed * Time.deltaTime;
        _pitch = Mathf.Clamp(_pitch, minPitch, maxPitch);
    }

    private void ApplyTransform()
    {
        var rotation = Quaternion.Euler(_pitch, _yaw, 0f);
        var orbitCenter = new Vector3(_player.position.x, _player.position.y + _orbitCenterOffsetY, _player.position.z);

        var desiredPosition = orbitCenter + rotation * Vector3.back * _distance;

        // 회전/줌으로 카메라가 terrain 표면 아래로 파고드는 것을 방지
        var terrain = Terrain.activeTerrain;
        if (terrain != null)
        {
            var groundY = terrain.transform.position.y + terrain.SampleHeight(desiredPosition);
            var minY = groundY + groundClearance;
            if (desiredPosition.y < minY)
                desiredPosition.y = minY;
        }

        transform.position = desiredPosition;
        transform.rotation = rotation;
    }
}