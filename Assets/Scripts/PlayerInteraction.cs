using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{

    [Header("Movimiento y Físicas")]
    [SerializeField] private float velocidad = 5f;
    [SerializeField] private float fuerzaSalto = 6f;
    private Rigidbody rb;

    [Header("Configuración de Capas")]
    [SerializeField] private LayerMask capaSuelo;
    [SerializeField] private LayerMask capaInteractuable;

    [Header("Configuración de Rayos")]
    [SerializeField] private float distanciaInteraccion = 3f;
    [SerializeField] private float distanciaSuelo = 1.1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        MoverJugador();
        ChequeoDeSalto();
        InteraccionADistancia();
    }

    void MoverJugador()
    {
        float movX = Input.GetAxis("Horizontal");
        float movZ = Input.GetAxis("Vertical");

        Vector3 movimiento = transform.right * movX + transform.forward * movZ;

        Vector3 nuevaPosicion = rb.position + movimiento * velocidad * Time.deltaTime;
        rb.MovePosition(nuevaPosicion);
    }


    void ChequeoDeSalto()
    {
        bool enSuelo = Physics.Raycast(transform.position, Vector3.down, distanciaSuelo, capaSuelo);

        if (Input.GetKeyDown(KeyCode.Space) && enSuelo)
        {
            rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
            Debug.Log("Salto limpio validado por Raycast.");
        }
    }


    void InteraccionADistancia()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, distanciaInteraccion, capaInteractuable))
            {
                Debug.Log("Interactuando a distancia con: " + hit.collider.gameObject.name);
            }
        }
    }
}
