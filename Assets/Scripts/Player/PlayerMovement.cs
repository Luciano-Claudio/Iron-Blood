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
    private bool isLocked; // true durante sono ou transição de cena
    public Vector2 LastMoveDirection { get; private set; } = Vector2.down;

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        GameEvents.OnSceneTransitionStarted += Lock;
        GameEvents.OnSceneTransitionEnded   += Unlock;
    }

    private void OnDisable()
    {
        GameEvents.OnSceneTransitionStarted -= Lock;
        GameEvents.OnSceneTransitionEnded   -= Unlock;
    }

    private void Lock()   { isLocked = true;  moveInput = Vector2.zero; }
    private void Unlock() => isLocked = false;

    public void Teleport(Vector2 position) => transform.position = position;

    public void OnMove(InputValue value)
    {
        if (isLocked || (GameClock.Instance != null && GameClock.Instance.IsSleeping))
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
        if (isLocked || (GameClock.Instance != null && GameClock.Instance.IsSleeping))
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }
        rb.linearVelocity = moveInput * moveSpeed;
    }
}
