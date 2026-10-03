using UnityEngine;

/// <summary>
///respawnovich
/// </summary>
public class RespawnManager : MonoBehaviour
{
    public static RespawnManager Instance { get; private set; }

    private Vector3 currentCheckpoint;
    private bool hasCheckpoint = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Si todavia no se activo ningun checkpoint, usamos la posicion inicial del jugador.
        if (!hasCheckpoint)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                currentCheckpoint = player.transform.position;
        }
    }

    public void SetCheckpoint(Vector3 position)
    {
        Debug.Log("SetCheckpoint llamado con: " + position);
        currentCheckpoint = position;
        hasCheckpoint = true;
    }

    public void Respawn(Rigidbody playerRb)
    {
        Debug.Log("Respawn ejecutado hacia: " + currentCheckpoint);

        if (playerRb == null) return;

        playerRb.linearVelocity = Vector3.zero;
        playerRb.angularVelocity = Vector3.zero;
        playerRb.transform.position = currentCheckpoint;
    }
}