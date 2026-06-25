using UnityEngine;

public class DoorInteractable : MonoBehaviour, IInteractable
{
    [Header("Destino")]
    [SerializeField] private string targetScene;
    [Tooltip("spawnId do PlayerSpawnPoint na cena destino. Ex: 'ferraria_entrada', 'porão_escada'")]
    [SerializeField] private string targetSpawnId = "default";

    [Header("Visual (opcional)")]
    [SerializeField] private SpriteRenderer doorRenderer;
    [SerializeField] private Sprite openSprite;
    [SerializeField] private Sprite closedSprite;

    public string InteractionLabel => "Entrar";

    public void Interact()
    {
        SceneLoader.Instance.LoadArea(targetScene, targetSpawnId);
    }
}
