using UnityEngine;
using UnityEngine.InputSystem;

public class MoveController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float jumpDuration = 1.0f;
    [SerializeField] private float rotationLerpSpeed = 10f;
    [SerializeField] private Camera mainCamera;

    private const float GroundedVelocity = -0.5f;
    private const float MoveDirSqrThreshold = 0.01f;
    private const float GravityMultiplier = 2f;

    private float _gravity;
    private float _velocityY = 0f;
    private CharacterController _controller;

    // PlayerInput (Send Messages) 에서 채워주는 입력 상태
    private Vector2 _moveInput;
    private bool _jumpPressed;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _gravity = -(GravityMultiplier * jumpForce) / jumpDuration;

        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    // PlayerInput 메시지 콜백: Player 액션맵의 "Move" 액션
    private void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }

    // PlayerInput 메시지 콜백: Player 액션맵의 "Jump" 액션
    private void OnJump(InputValue value)
    {
        if (value.isPressed)
            _jumpPressed = true;
    }

    private void Update()
    {
        var dx = _moveInput.x;
        var dz = _moveInput.y;

        // 카메라 기준 수평 방향 벡터 (Y축 제거)
        var camForward = mainCamera.transform.forward;
        var camRight   = mainCamera.transform.right;
        camForward.y = 0f;
        camRight.y   = 0f;
        camForward.Normalize();
        camRight.Normalize();

        var moveDir = camForward * dz + camRight * dx;

        if (_controller != null)
        {
            if (_controller.isGrounded)
            {
                _velocityY = GroundedVelocity;
                if (_jumpPressed)
                    _velocityY = jumpForce;
            }

            _velocityY += _gravity * Time.deltaTime;

            var move = moveDir * moveSpeed * Time.deltaTime;
            move.y = _velocityY * Time.deltaTime;
            _controller.Move(move);
        }
        else
        {
            var move = moveDir * moveSpeed * Time.deltaTime;
            transform.Translate(move, Space.World);
        }

        // 이동 방향으로 플레이어 회전
        if (moveDir.sqrMagnitude > MoveDirSqrThreshold)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDir), rotationLerpSpeed * Time.deltaTime);

        // jump는 한 프레임에만 소비
        _jumpPressed = false;
    }
}
