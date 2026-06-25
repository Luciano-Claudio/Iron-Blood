using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public class PlayerInteractor : MonoBehaviour
{
    public static PlayerInteractor Instance;

    private IInteractable currentInteractable;

    public IInteractable CurrentInteractable => currentInteractable;

    public event System.Action<IInteractable> OnInteractableEntered;
    public event System.Action                OnInteractableExited;

    private void Awake() => Instance = this;

    // Chamado pelo PlayerInput via Send Messages — action "Interact" mapeada para E
    public void OnInteract(InputValue value)
    {
        if (!value.isPressed) return;
        if (GameStateManager.Instance != null && GameStateManager.Instance.IsPlayerLocked) return;
        currentInteractable?.Interact();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<IInteractable>(out var interactable))
        {
            currentInteractable = interactable;
            OnInteractableEntered?.Invoke(interactable);
            Debug.Log($"[PlayerInteractor] Entrou em range: {interactable.InteractionLabel}");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<IInteractable>(out var interactable)
            && interactable == currentInteractable)
        {
            currentInteractable = null;
            OnInteractableExited?.Invoke();
            Debug.Log("[PlayerInteractor] Saiu do range.");
        }
    }
}
