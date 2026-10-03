using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controla el panel de UI que aparece al llegar a la meta, con el boton que carga el
/// siguiente nivel. Requisito Nota 4.
/// </summary>
public class LevelCompleteUI : MonoBehaviour
{
    public static LevelCompleteUI Instance { get; private set; }

    [Tooltip("Arrastrar aqui el panel de UI (inicialmente desactivado en la escena)")]
    public GameObject panel;

    [Tooltip("Nombre EXACTO de la escena a cargar (debe estar agregada en Build Settings)")]
    public string nextSceneName = "Level2";

    void Awake()
    {
        Instance = this;
        if (panel != null) panel.SetActive(false);
    }

    public void ShowLevelComplete()
    {
        if (panel != null) panel.SetActive(true);
        Time.timeScale = 0f; // pausa el juego mientras se muestra la UI (opcional)

        // Libera el cursor para poder clickear el boton (la camara lo bloquea durante el juego)
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    /// <summary>
    /// Enganchar esta funcion al evento OnClick() del boton, desde el Inspector.
    /// </summary>
    public void LoadNextLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nextSceneName);
    }
}