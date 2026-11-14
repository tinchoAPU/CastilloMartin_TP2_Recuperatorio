using System.Collections;
using UnityEngine;

public class ToxicGasZone : MonoBehaviour
{
    [Header("Configuración de Gas")]
    [SerializeField] private int danoPorTick = 10;
    [SerializeField] private float intervaloDano = 1.5f;

    private Coroutine corrutinaVeneno;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth salud = other.GetComponent<PlayerHealth>();
            if (salud != null && corrutinaVeneno == null)
            {
                Debug.Log("entro por aca");
                corrutinaVeneno = StartCoroutine(AplicarVeneno(salud));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && corrutinaVeneno != null)
        {
            StopCoroutine(corrutinaVeneno);
            corrutinaVeneno = null;
        }
    }

    private IEnumerator AplicarVeneno(PlayerHealth salud)
    {
        while (salud != null && salud.SaludActual > 0)
        {
            salud.RestarVida(danoPorTick);
            Debug.Log($"<color=magenta>Gas Tóxico: -{danoPorTick} HP</color>");
            yield return new WaitForSeconds(intervaloDano);
        }
        corrutinaVeneno = null;
    }
}