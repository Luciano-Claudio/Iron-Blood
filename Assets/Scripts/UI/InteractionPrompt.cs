using UnityEngine;

// Exibe o ícone da tecla [E] sobre o objeto interagível em world space.
// Usa RectTransformUtility para converter corretamente com Canvas Scaler ativo.
public class InteractionPrompt : MonoBehaviour
{
    [SerializeField] private RectTransform promptPanel;
    [SerializeField] private Vector3       worldOffset = new Vector3(0f, 1f, 0f);

    private Transform targetTransform;
    private Camera    mainCamera;
    private Canvas    rootCanvas;

    private void Awake()
    {
        mainCamera = Camera.main;
        rootCanvas = GetComponentInParent<Canvas>();
        promptPanel.gameObject.SetActive(false);
    }

    private void Start()
    {
        PlayerInteractor.Instance.OnInteractableEntered += Show;
        PlayerInteractor.Instance.OnInteractableExited  += Hide;
    }

    private void OnDestroy()
    {
        if (PlayerInteractor.Instance == null) return;
        PlayerInteractor.Instance.OnInteractableEntered -= Show;
        PlayerInteractor.Instance.OnInteractableExited  -= Hide;
    }

    private void Show(IInteractable interactable)
    {
        if (interactable is MonoBehaviour mb)
            targetTransform = mb.transform;

        promptPanel.gameObject.SetActive(true);
    }

    private void Hide()
    {
        promptPanel.gameObject.SetActive(false);
        targetTransform = null;
    }

    private void LateUpdate()
    {
        if (targetTransform == null) return;

        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(
            mainCamera, targetTransform.position + worldOffset);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rootCanvas.transform as RectTransform,
            screenPos,
            rootCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : rootCanvas.worldCamera,
            out Vector2 localPos);

        ((RectTransform)transform).localPosition = localPos;
    }
}
