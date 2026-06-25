using UnityEngine;

// Marca o ponto onde o player aparece ao entrar nesta cena.
// Cada cena de área deve ter exatamente um PlayerSpawnPoint.
public class PlayerSpawnPoint : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.3f);
        Gizmos.DrawIcon(transform.position, "sv_icon_dot3_pix16_gizmo", true);
    }
}
