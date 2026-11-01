using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Salud")]
    [SerializeField] private int saludMaxima = 100;
    private int saludActual;

    [Header("Inmunidad (Cooldown)")]
    [SerializeField] private float tiempoInmunidad = 1.5f;
    private float temporizadorInmunidad;
    private bool esInmune;

    [Header("Puntuación")]
    private int puntuacionTotal = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        saludActual = saludMaxima;
    }

    // Update is called once per frame
    void Update()
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

        saludActual -= cantidad;
        Debug.Log("Daño recibido: " + cantidad + ". Salud restante: " + saludActual);

        esInmune = true;
        temporizadorInmunidad = tiempoInmunidad;

        if (saludActual <= 0)
        {
            Debug.Log("El jugador ha sido derrotado.");
        }
    }

    public void SumarPuntos(int puntos)
    {
        puntuacionTotal += puntos;
        Debug.Log("Ítem recogido. Puntuación: " + puntuacionTotal);
    }
}
