using UnityEngine;

/// <summary>
/// Movimiento del personaje en el plano XZ aplicando velocidad directamente sobre el Rigidbody,
/// conservando la gravedad del motor. Incluye salto (con doble salto opcional) mediante
/// deteccion de piso con OnCollisionStay/OnCollisionEnter. Requisito Nota 4.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 6f;
    public float rotationSpeed = 10f;

    [Header("Ajuste visual del modelo")]
    [Tooltip("Si el personaje queda mirando para un costado en vez de hacia donde se mueve, " +
             "es porque el modelo 3D no esta orientado hacia +Z. Probar 90, -90 o 180 hasta que quede alineado.")]
    public float modelForwardOffset = 0f;

    [Header("Salto")]
    public float jumpForce = 7f;
    [Tooltip("1 = salto simple, 2 = doble salto")]
    public int maxJumps = 2;
    public string groundTag = "Ground";

    [Header("Referencias")]
    [Tooltip("Arrastrar la Main Camera (o el pivote de la camara en tercera persona)")]
    public Transform cameraTransform;

    private Rigidbody rb;
    private Vector3 inputDir;
    private bool isGrounded;
    private int jumpsUsed;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // evita que el Rigidbody vuelque al chocar contra paredes/plataformas

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        // --- Lectura de input (Input Manager clasico) ---
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        inputDir = new Vector3(h, 0f, v);
        if (inputDir.sqrMagnitude > 1f) inputDir.Normalize();

        // --- Salto / doble salto: se permite mientras no se hayan gastado los saltos disponibles ---
        if (Input.GetButtonDown("Jump") && jumpsUsed < maxJumps)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
            jumpsUsed++;
        }
    }

    void FixedUpdate()
    {
        // Movimiento relativo a la camara: "adelante" siempre es hacia donde mira la camara,
        // pero proyectado sobre el plano XZ para no moverse hacia arriba/abajo.
        Vector3 camForward = cameraTransform
            ? Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized
            : Vector3.forward;
        Vector3 camRight = cameraTransform
            ? Vector3.ProjectOnPlane(cameraTransform.right, Vector3.up).normalized
            : Vector3.right;

        Vector3 moveDir = camForward * inputDir.z + camRight * inputDir.x;
        Vector3 horizontalVelocity = moveDir * moveSpeed;

        // Se aplica la velocidad directamente sobre el Rigidbody, conservando la gravedad (eje Y intacto)
        rb.linearVelocity = new Vector3(horizontalVelocity.x, rb.linearVelocity.y, horizontalVelocity.z);

        // Rotar el personaje para que mire hacia donde se mueve (mejora la sensacion de control).
        // Se suma modelForwardOffset por si el modelo no esta modelado mirando hacia +Z.
        if (moveDir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir, Vector3.up) * Quaternion.Euler(0f, modelForwardOffset, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime);
        }
    }

    // --- Deteccion de piso mediante colisiones estandar (requisito explicito del TP) ---
    void OnCollisionEnter(Collision collision)
    {
        // Se resetean los saltos disponibles apenas se vuelve a tocar el piso (aterrizaje)
        if (collision.gameObject.CompareTag(groundTag))
        {
            jumpsUsed = 0;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag(groundTag))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag(groundTag))
        {
            isGrounded = false;
        }
    }
}