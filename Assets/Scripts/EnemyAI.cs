using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Configuración de Referencias")]
    [SerializeField] private Transform jugador;
    [SerializeField] private Transform[] puntosPatrulla;
    private NavMeshAgent agente;
    private int indicePuntoActual = 0;

    [Header("Configuración de Rangos")]
    [SerializeField] private float radioDeteccion = 10f;
    [SerializeField] private float radioAtaque = 2.5f;
    void Start()
    {
        // Iniciar patrullaje yendo al primer punto
        agente = GetComponent<NavMeshAgent>();

        if (puntosPatrulla.Length > 0)
        {
            agente.SetDestination(puntosPatrulla[0].position);
        }
    }

    void Update()
    {
        float distanciaAlJugador = Vector3.Distance(transform.position, jugador.position);

        if (distanciaAlJugador <= radioAtaque)
        {
            Atacar();
        }
        else if (distanciaAlJugador <= radioDeteccion)
        {
            Perseguir();
        }
        else
        {
            Patrullar();
        }
    }


    void Patrullar()
    {
        if (puntosPatrulla.Length == 0) return;

        if (agente.remainingDistance < 0.5f && !agente.pathPending)
        {
            indicePuntoActual = (indicePuntoActual + 1) % puntosPatrulla.Length;
            agente.SetDestination(puntosPatrulla[indicePuntoActual].position);
        }
    }


    void Perseguir()
    {
        agente.SetDestination(jugador.position);
    }

    void Atacar()
    {
        agente.SetDestination(transform.position);
        Debug.Log("El centinela te alcanzó y está atacando.");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radioDeteccion);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioAtaque);
    }
}
