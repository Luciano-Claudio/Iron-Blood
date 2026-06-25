using UnityEngine;

// Marca o ponto onde o player aparece ao entrar nesta cena.
// Cada porta de destino tem um spawnId correspondente a um PlayerSpawnPoint na cena destino.
// Ex: porta "ferraria_entrada" → PlayerSpawnPoint com spawnId "ferraria_entrada"
public class PlayerSpawnPoint : MonoBehaviour
{
    [Tooltip("ID único nesta cena. Deve corresponder ao targetSpawnId da porta de origem.")]
    public string spawnId = "default";

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.3f);
        Gizmos.DrawIcon(transform.position, "sv_icon_dot3_pix16_gizmo", true);
    }
}
