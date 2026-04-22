using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float jumpDuration = 1.0f;

    private float _gravity;
    private float _velocityY = 0f;
    private CharacterController _controller;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _gravity = -(2f * jumpForce) / jumpDuration;
    }

    private void Update()
    {
        float dx = 0f;
        float dz = 0f;

        if (Input.GetKey(KeyCode.W)) dz += 1f;
        if (Input.GetKey(KeyCode.S)) dz -= 1f;
        if (Input.GetKey(KeyCode.A)) dx -= 1f;
        if (Input.GetKey(KeyCode.D)) dx += 1f;

        if (_controller != null)
        {
            if (_controller.isGrounded)
            {
                _velocityY = -0.5f;
                if (Input.GetKeyDown(KeyCode.Space))
                    _velocityY = jumpForce;
            }

            _velocityY += _gravity * Time.deltaTime;

            Vector3 move = new Vector3(dx, _velocityY, dz) * moveSpeed * Time.deltaTime;
            move.y = _velocityY * Time.deltaTime;
            _controller.Move(move);
        }
        else
        {
            Vector3 move = new Vector3(dx, 0f, dz) * moveSpeed * Time.deltaTime;
            transform.Translate(move, Space.World);
        }
    }
}
