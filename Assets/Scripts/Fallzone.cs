using UnityEngine;

/// <summary>
/// Zona de caida: un Collider marcado como "Is Trigger" debajo del nivel. Al entrar el jugador,
/// se lo reenvia al ultimo checkpoint (o inicio del nivel). Requisito Nota 4.
/// </summary>
public class FallZone : MonoBehaviour
{
    public string playerTag = "Player";

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            Rigidbody playerRb = other.attachedRigidbody;
            RespawnManager.Instance.Respawn(playerRb);
        }
    }
}