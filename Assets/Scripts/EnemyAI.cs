using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Configuración de Referencias")]
    [SerializeField] private Transform jugador;
    [SerializeField] private Transform[] puntosPatrulla;
    [SerializeField] private short ataque = 10;

    private NavMeshAgent agente;
    private PlayerHealth saludJugador;
    private int indicePuntoActual = 0;

    [Header("Configuración de Rangos")]
    [SerializeField] private float radioDeteccion = 10f;
    [SerializeField] private float radioAtaque = 2.5f;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();

        if (jugador != null)
        {
            saludJugador = jugador.GetComponentInParent<PlayerHealth>();
        }

        if (puntosPatrulla.Length > 0)
        {
            agente.SetDestination(puntosPatrulla[0].position);
        }
    }

    void Update()
    {
        if (jugador == null) return;

        bool jugadorVisible = (saludJugador == null || !saludJugador.EsInvisible);
        float distanciaAlJugador = Vector3.Distance(transform.position, jugador.position);

        if (jugadorVisible && distanciaAlJugador <= radioAtaque)
        {
            Atacar();
        }
        else if (jugadorVisible && distanciaAlJugador <= radioDeteccion)
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

        if (!agente.pathPending && agente.remainingDistance < 0.5f)
        {
            indicePuntoActual = (indicePuntoActual + 1) % puntosPatrulla.Length;
        }

        agente.SetDestination(puntosPatrulla[indicePuntoActual].position);
    }

    void Perseguir()
    {
        agente.SetDestination(jugador.position);
    }

    void Atacar()
    {
        agente.SetDestination(transform.position);

        if (saludJugador != null)
        {
            saludJugador.RestarVida(ataque);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radioDeteccion);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioAtaque);
    }
}