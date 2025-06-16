using UnityEngine;

public class HelicopterController : MonoBehaviour
{
    // --- Variables de F�sica y Control ---
    // Estas variables las podremos ajustar desde el Inspector de Unity.

    [Tooltip("La fuerza que empuja el helic�ptero hacia arriba.")]
    public float liftForce = 15f;

    [Tooltip("La fuerza para moverse hacia adelante y atr�s.")]
    public float forwardForce = 10f;

    [Tooltip("La fuerza para rotar sobre su propio eje (pedales).")]
    public float yawTorque = 5f;

    [Tooltip("Cu�nto se inclina el helic�ptero visualmente al moverse.")]
    public float tiltAmount = 25f;


    // --- Componentes ---
    private Rigidbody rb; // Referencia al componente Rigidbody para aplicar f�sicas.


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
        // Leer si la barra espaciadora est� siendo presionada para elevarse.
        isLifting = Input.GetKey(KeyCode.Space);

        // Leer los ejes Vertical (W/S o flechas arriba/abajo) y Horizontal (A/D o flechas izq/der).
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
    }

    // FixedUpdate se llama en intervalos de tiempo fijos. Es el lugar correcto para aplicar f�sicas.
    void FixedUpdate()
    {
        // 1. Aplicar fuerza de elevaci�n (Colectivo)
        if (isLifting)
        {
            // A�adimos una fuerza constante hacia arriba mientras se presione Espacio.
            rb.AddForce(Vector3.up * liftForce);
        }

        // 2. Aplicar fuerza de avance/retroceso (C�clico)
        // Usamos AddRelativeForce para que la fuerza se aplique en la direcci�n local del helic�ptero.
        rb.AddRelativeForce(Vector3.forward * verticalInput * forwardForce);

        // 3. Aplicar rotaci�n (Pedales / Yaw)
        // Usamos AddTorque para hacer girar el helic�ptero sobre su eje Y.
        rb.AddTorque(Vector3.up * horizontalInput * yawTorque);

        // 4. Inclinaci�n visual (Opcional, pero da una buena sensaci�n)
        // Inclinamos el modelo del helic�ptero para que se vea m�s realista al moverse.
        // No afecta a la f�sica, solo a la rotaci�n visual del objeto.
        rb.transform.localRotation = Quaternion.Euler(
            verticalInput * tiltAmount,      // Inclinaci�n adelante/atr�s
            rb.transform.localRotation.eulerAngles.y, // Mantenemos la rotaci�n actual del yaw
            -horizontalInput * tiltAmount    // Inclinaci�n lateral
        );
    }
}