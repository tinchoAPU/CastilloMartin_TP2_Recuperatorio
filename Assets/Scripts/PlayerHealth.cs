using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Salud")]
    [SerializeField] private int saludMaxima = 100;
    private int saludActual;

    public int SaludActual => saludActual;
    public int SaludMaxima => saludMaxima;

    [Header("Inmunidad (Cooldown)")]
    [SerializeField] private float tiempoInmunidad = 1.5f;
    private float temporizadorInmunidad;
    private bool esInmune;

    public event Action<int, int> OnHealthChanged; // (saludActual, saludMaxima)

    private Rigidbody rb;
    private Collider col;
    private MeshRenderer mesh;

    public bool EsInvisible { get; private set; }
    public event Action<bool> OnInvisibilityChanged;

    private Coroutine corrutinaInvisibilidad;

    public void ActivarInvisibilidad(float duracion)
    {
        if (corrutinaInvisibilidad != null)
        {
            StopCoroutine(corrutinaInvisibilidad);
        }
        corrutinaInvisibilidad = StartCoroutine(ProcesoInvisibilidad(duracion));
    }

    private IEnumerator ProcesoInvisibilidad(float duracion)
    {
        EsInvisible = true;
        OnInvisibilityChanged?.Invoke(true);
        Debug.Log($"<color=cyan>¡Invisibilidad activada por {duracion} segundos!</color>");

        yield return new WaitForSeconds(duracion);

        EsInvisible = false;
        corrutinaInvisibilidad = null;
        OnInvisibilityChanged?.Invoke(false);
        Debug.Log("<color=yellow>La invisibilidad ha terminado.</color>");
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        mesh = GetComponentInChildren<MeshRenderer>();
    }

    private void Start()
    {
        saludActual = saludMaxima;
        OnHealthChanged?.Invoke(saludActual, saludMaxima);
    }

    private void Update()
    {
        if (esInmune)
        {
            temporizadorInmunidad -= Time.deltaTime;
            if (temporizadorInmunidad <= 0)
            {
                esInmune = false;
            }
        }
    }

    public void RestarVida(int cantidad)
    {
        if (esInmune || saludActual <= 0) return;

        saludActual = Mathf.Max(saludActual - cantidad, 0);
        Debug.Log($"Daño recibido: {cantidad}. Salud restante: {saludActual}");

        OnHealthChanged?.Invoke(saludActual, saludMaxima);

        esInmune = true;
        temporizadorInmunidad = tiempoInmunidad;

        if (saludActual <= 0)
        {
            Debug.Log("El jugador ha sido derrotado.");
            Dead();
        }
    }

    public bool EstaHerido()
    {
        return saludActual < saludMaxima;
    }

    public void Curar(int cantidad)
    {
        if (saludActual <= 0) return;

        saludActual = Mathf.Min(saludActual + cantidad, saludMaxima);
        Debug.Log($"<color=green>Curado +{cantidad} HP. Salud actual: {saludActual}</color>");

        OnHealthChanged?.Invoke(saludActual, saludMaxima);
    }

    private void Dead()
    {
        if (gameObject.CompareTag("Player"))
        {
            GameManager.Instance?.NotifyPlayerDead();
        }

        // StartCoroutine(DestroyTransition());
    }

    private IEnumerator DestroyTransition()
    {
        if (col != null) col.enabled = false;
        if (rb != null) rb.isKinematic = true;
        if (mesh != null) mesh.enabled = false;

        yield return new WaitForSecondsRealtime(5f);

        Destroy(gameObject);
    }
}