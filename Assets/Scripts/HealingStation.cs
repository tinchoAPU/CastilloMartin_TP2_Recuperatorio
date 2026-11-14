using System;
using System.Collections;
using UnityEngine;

public class HealingStation : MonoBehaviour
{
    [Header("Configuración de Curación")]
    [SerializeField] private int curacionPorTick = 10;
    [SerializeField] private float intervaloCuracion = 1.5f;
    [SerializeField] private int cargaMaxima = 50;

    [Header("Configuración de Recarga (Cooldown)")]
    [SerializeField] private float tiempoRecarga = 8f;

    [Header("Retroalimentación Visual")]
    [SerializeField] private Light luzEstacion;
    [SerializeField] private ParticleSystem particulasEstacion;

    public event Action<bool> OnEstadoEstacionCambiado;

    private int cargaActual;
    private bool estaDisponible = true;
    private Coroutine corrutinaCuracion;

    private void Start()
    {
        cargaActual = cargaMaxima;
        ActualizarVisuales(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!estaDisponible) return;

        if (other.CompareTag("Player"))
        {
            PlayerHealth salud = other.GetComponent<PlayerHealth>();
            if (salud != null && corrutinaCuracion == null)
            {
                corrutinaCuracion = StartCoroutine(ProcesoCuracion(salud));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && corrutinaCuracion != null)
        {
            StopCoroutine(corrutinaCuracion);
            corrutinaCuracion = null;
        }
    }

    private IEnumerator ProcesoCuracion(PlayerHealth salud)
    {
        while (estaDisponible && cargaActual > 0)
        {
            if (salud.EstaHerido())
            {
                int cantidadACurar = Mathf.Min(curacionPorTick, cargaActual);
                salud.Curar(cantidadACurar);
                cargaActual -= cantidadACurar;

                Debug.Log($"<color=green>Estación curando: +{cantidadACurar} HP. Carga restante de estación: {cargaActual}/{cargaMaxima}</color>");

                if (cargaActual <= 0)
                {
                    corrutinaCuracion = null;
                    StartCoroutine(RecargarEstacion());
                    yield break;
                }
            }

            yield return new WaitForSeconds(intervaloCuracion);
        }

        corrutinaCuracion = null;
    }

    private IEnumerator RecargarEstacion()
    {
        estaDisponible = false;
        ActualizarVisuales(false);
        OnEstadoEstacionCambiado?.Invoke(false);

        Debug.Log($"<color=orange>Estación en Recarga... Disponible nuevamente en {tiempoRecarga} segundos.</color>");

        yield return new WaitForSeconds(tiempoRecarga);

        cargaActual = cargaMaxima;
        estaDisponible = true;
        ActualizarVisuales(true);
        OnEstadoEstacionCambiado?.Invoke(true);

        Debug.Log("<color=cyan>Estación de Curación lista nuevamente.</color>");
    }

    private void ActualizarVisuales(bool activa)
    {
        if (luzEstacion != null)
        {
            luzEstacion.enabled = activa;
        }

        if (particulasEstacion != null)
        {
            if (activa) particulasEstacion.Play();
            else particulasEstacion.Stop();
        }
    }
}