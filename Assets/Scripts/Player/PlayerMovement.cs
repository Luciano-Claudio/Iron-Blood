using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Configuração")]
    [SerializeField] private float moveSpeed = 4f;

    public static PlayerMovement Instance;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    public Vector2 LastMoveDirection { get; private set; } = Vector2.down;

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
    }

    public void Teleport(Vector2 position) => transform.position = position;

    public void OnMove(InputValue value)
    {
        if (GameStateManager.Instance != null && GameStateManager.Instance.IsPlayerLocked)
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
        if (GameStateManager.Instance != null && GameStateManager.Instance.IsPlayerLocked)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }
        rb.linearVelocity = moveInput * moveSpeed;
    }
}
