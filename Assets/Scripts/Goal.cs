using UnityEngine;

/// <summary>
/// Meta final del nivel: un Collider "Is Trigger". Al llegar el jugador, muestra la UI de
/// nivel completado (con el boton para cargar el siguiente nivel). Requisito Nota 4.
/// </summary>
public class Goal : MonoBehaviour
{
    public string playerTag = "Player";

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            LevelCompleteUI.Instance.ShowLevelComplete();
        }
    }
}