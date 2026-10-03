using UnityEngine;

/// <summary>
/// Camara en tercera persona que orbita alrededor del jugador controlada con el mouse,
/// manteniendo siempre la misma distancia (estable). Requisito Nota 4.
///
/// IMPORTANTE sobre el diseno: esta camara NO mira hacia el jugador para decidir su propia
/// rotacion (eso generaba el bug anterior: como el movimiento del jugador depende de hacia
/// donde "mira" la camara, y la camara miraba hacia el jugador, se armaba un circulo vicioso
/// que la hacia saltar al costado cuando el jugador giraba). Aca la rotacion de la camara
/// depende UNICAMENTE del mouse (yaw/pitch propios), y el jugador simplemente lee esa
/// rotacion para moverse relativo a ella. Flujo de una sola direccion: Mouse -> Camara -> Jugador.
/// </summary>
public class ThirdPersonCamera : MonoBehaviour
{
    [Tooltip("Arrastrar el Transform del jugador (W)")]
    public Transform target;

    [Tooltip("Punto alrededor del cual orbita, relativo al jugador (altura de cabeza aprox)")]
    public Vector3 pivotOffset = new Vector3(0f, 1.5f, 0f);

    public float distance = 6f;
    public float mouseSensitivity = 3f;
    public float minPitch = -20f;
    public float maxPitch = 60f;

    private float yaw;
    private float pitch = 20f;

    void Start()
    {
        // Bloquea y oculta el cursor para un control de camara tipo juego.
        // LevelCompleteUI lo vuelve a mostrar cuando aparece el panel de fin de nivel.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (target != null)
            yaw = target.eulerAngles.y;
    }

   void Update()
{
    // Permite destrabar el cursor en cualquier momento para testear (Escape) y re-trabarlo con un click.
    if (Input.GetKeyDown(KeyCode.Escape))
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    else if (Time.timeScale > 0f && Input.GetMouseButtonDown(0) && Cursor.lockState != CursorLockMode.Locked)
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}

    void LateUpdate()
    {
        if (target == null) return;

        if (Cursor.lockState == CursorLockMode.Locked)
        {
            yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
            pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        }

        Quaternion rot = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 pivot = target.position + pivotOffset;
        Vector3 desiredPosition = pivot - (rot * Vector3.forward * distance);

        transform.position = desiredPosition;
        transform.rotation = rot;
    }
}