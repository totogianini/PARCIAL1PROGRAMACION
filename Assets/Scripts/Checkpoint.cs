using UnityEngine;

/// <summary>
/// Punto de control: un Collider "Is Trigger" en el nivel. Al tocarlo el jugador, actualiza
/// el punto de reaparicion en el RespawnManager. Requisito Nota 7.
/// </summary>
public class Checkpoint : MonoBehaviour
{
    public string playerTag = "Player";

    [Tooltip("Opcional: objeto visual que se activa para marcar que el checkpoint ya fue tocado (ej: una bandera o luz)")]
    public GameObject activatedVisual;

    private bool activated = false;

    void OnTriggerEnter(Collider other)
    {
        if (!activated && other.CompareTag(playerTag))
        {
            activated = true;
            Debug.Log("Checkpoint tocado en: " + transform.position);
            RespawnManager.Instance.SetCheckpoint(transform.position);

            if (activatedVisual != null)
                activatedVisual.SetActive(true);
        }
    }
}