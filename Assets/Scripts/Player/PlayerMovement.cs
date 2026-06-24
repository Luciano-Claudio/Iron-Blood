using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Configuração")]
    [SerializeField] private float moveSpeed = 4f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    public Vector2 LastMoveDirection { get; private set; } = Vector2.down;

    private void Awake()
        => rb = GetComponent<Rigidbody2D>();

    public void OnMove(InputValue value)
    {
        if (GameClock.Instance != null && GameClock.Instance.IsSleeping)
        {
            moveInput = Vector2.zero;
            return;
        }
        moveInput = value.Get<Vector2>();
        if (moveInput != Vector2.zero)
            LastMoveDirection = moveInput.normalized;
    }

    private void FixedUpdate()
    {
        if (GameClock.Instance != null && GameClock.Instance.IsSleeping)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }
        rb.linearVelocity = moveInput * moveSpeed;
    }
}
