using UnityEngine;

public class SlowZone : MonoBehaviour
{
    [Header("Configuración de Barro")]
    [SerializeField][Range(0.1f, 0.9f)] private float factorVelocidad = 0.4f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInteraction player = other.GetComponentInParent<PlayerInteraction>();
            if (player != null)
            {
                player.AplicarDesaceleracion(factorVelocidad);
                Debug.Log("<color=orange>Entraste al barro: velocidad reducida.</color>");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInteraction player = other.GetComponentInParent<PlayerInteraction>();
            if (player != null)
            {
                player.RestaurarVelocidad();
                Debug.Log("<color=green>Saliste del barro: velocidad normal restaurada.</color>");
            }
        }
    }
}