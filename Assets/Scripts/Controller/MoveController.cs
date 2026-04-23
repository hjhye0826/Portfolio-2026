using UnityEngine;

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

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _gravity = -(GravityMultiplier * jumpForce) / jumpDuration;

        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    private void Update()
    {
        var dx = 0f;
        var dz = 0f;

        if (Input.GetKey(KeyCode.W)) dz += 1f;
        if (Input.GetKey(KeyCode.S)) dz -= 1f;
        if (Input.GetKey(KeyCode.A)) dx -= 1f;
        if (Input.GetKey(KeyCode.D)) dx += 1f;

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
                if (Input.GetKeyDown(KeyCode.Space))
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
    }
}
