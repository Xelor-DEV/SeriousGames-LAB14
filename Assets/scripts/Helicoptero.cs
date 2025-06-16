using UnityEngine;

public class HelicopterController : MonoBehaviour
{
    // --- Variables de Física y Control ---
    // Estas variables las podremos ajustar desde el Inspector de Unity.

    [Tooltip("La fuerza que empuja el helicóptero hacia arriba.")]
    public float liftForce = 15f;

    [Tooltip("La fuerza para moverse hacia adelante y atrás.")]
    public float forwardForce = 10f;

    [Tooltip("La fuerza para rotar sobre su propio eje (pedales).")]
    public float yawTorque = 5f;

    [Tooltip("Cuánto se inclina el helicóptero visualmente al moverse.")]
    public float tiltAmount = 25f;

    // Just add these two new lines below the other public variables

    [Tooltip("La velocidad máxima de rotación (en radianes/segundo).")]
    public float maxYawVelocity = 2f;

    [Tooltip("La fuerza del 'freno' de rotación cuando no se pulsa ninguna tecla.")]
    public float yawDamping = 3f;

    // Añade esta línea debajo de las otras variables públicas
    [Tooltip("La fuerza base que mantiene al helicóptero flotando para contrarrestar la gravedad.")]
    public float hoverPower = 10f;

    // Añade esta línea debajo de las otras variables públicas
    [Tooltip("La velocidad con la que el helicóptero se inclina visualmente.")]
    public float tiltSpeed = 5f;

    // --- Componentes ---
    private Rigidbody rb; // Referencia al componente Rigidbody para aplicar físicas.


    // --- Entradas del Teclado ---
    private float verticalInput;
    private float horizontalInput;
    private bool isLifting;


    // Start se llama una vez, al principio del juego.
    void Start()
    {
        // Obtenemos el componente Rigidbody del objeto para poder usarlo.
        rb = GetComponent<Rigidbody>();
    }

    // Update se llama en cada fotograma. Lo usamos para leer las entradas del teclado.
    void Update()
    {
        // Leer si la barra espaciadora está siendo presionada para elevarse.
        isLifting = Input.GetKey(KeyCode.Space);

        // Leer los ejes Vertical (W/S o flechas arriba/abajo) y Horizontal (A/D o flechas izq/der).
        verticalInput = Input.GetAxis("Vertical");
        verticalInput = Mathf.Clamp01(verticalInput);
        horizontalInput = Input.GetAxis("Horizontal");
    }

    // FixedUpdate se llama en intervalos de tiempo fijos. Es el lugar correcto para aplicar físicas.
    void FixedUpdate()
    {
        // --- NUEVA LÓGICA DE FLOTE ---
        // 1. Aplicamos una fuerza de sustentación constante para contrarrestar la gravedad.
        // Esto hace que el helicóptero "flote" en lugar de caer como una piedra.
        rb.AddForce(Vector3.up * hoverPower);


        // 2. Aplicar fuerza de elevación EXTRA (Colectivo)
        // Esto se suma a la fuerza de flote para poder ascender.
        if (isLifting)
        {
            rb.AddForce(Vector3.up * liftForce);
        }

        // 3. Aplicar fuerza de avance (Cíclico)
        // Sigue funcionando como antes, pero ahora "verticalInput" nunca será negativo.
        rb.AddRelativeForce(Vector3.forward * verticalInput * forwardForce);


        // 4. LÓGICA DE ROTACIÓN (Pedales / Yaw) - SIN CAMBIOS
        if (Mathf.Abs(horizontalInput) > 0.1f)
        {
            if (Mathf.Abs(rb.angularVelocity.y) < maxYawVelocity)
            {
                rb.AddTorque(Vector3.up * horizontalInput * yawTorque);
            }
        }
        else
        {
            rb.AddTorque(-rb.angularVelocity * yawDamping);
        }
        // --- 5. NUEVA LÓGICA DE INCLINACIÓN SUAVIZADA (LERP / SLERP) ---

        // Primero, calculamos la rotación a la que queremos llegar (nuestro objetivo).
        Quaternion targetRotation = Quaternion.Euler(
            verticalInput * tiltAmount,
            rb.transform.localRotation.eulerAngles.y,
            -horizontalInput * tiltAmount
        );
        rb.transform.localRotation = Quaternion.Slerp(
        rb.transform.localRotation, // Desde dónde
        targetRotation,             // Hacia dónde
        Time.fixedDeltaTime * tiltSpeed
        );
    }
}