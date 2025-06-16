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
        horizontalInput = Input.GetAxis("Horizontal");
    }

    // FixedUpdate se llama en intervalos de tiempo fijos. Es el lugar correcto para aplicar físicas.
    void FixedUpdate()
    {
        // 1. Aplicar fuerza de elevación (Colectivo)
        if (isLifting)
        {
            // Añadimos una fuerza constante hacia arriba mientras se presione Espacio.
            rb.AddForce(Vector3.up * liftForce);
        }

        // 2. Aplicar fuerza de avance/retroceso (Cíclico)
        // Usamos AddRelativeForce para que la fuerza se aplique en la dirección local del helicóptero.
        rb.AddRelativeForce(Vector3.forward * verticalInput * forwardForce);

        // 3. Aplicar rotación (Pedales / Yaw)
        // Usamos AddTorque para hacer girar el helicóptero sobre su eje Y.
        rb.AddTorque(Vector3.up * horizontalInput * yawTorque);

        // 4. Inclinación visual (Opcional, pero da una buena sensación)
        // Inclinamos el modelo del helicóptero para que se vea más realista al moverse.
        // No afecta a la física, solo a la rotación visual del objeto.
        rb.transform.localRotation = Quaternion.Euler(
            verticalInput * tiltAmount,      // Inclinación adelante/atrás
            rb.transform.localRotation.eulerAngles.y, // Mantenemos la rotación actual del yaw
            -horizontalInput * tiltAmount    // Inclinación lateral
        );
    }
}