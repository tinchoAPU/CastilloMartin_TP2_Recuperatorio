using UnityEngine;

public class InvisibilityPowerUp : MonoBehaviour
{
    [Header("Configuración del Power-Up")]
    [SerializeField] private float duracionInvisibilidad = 6f;
    [SerializeField] private float velocidadRotacion = 180f;

    private void Update()
    {
        transform.Rotate(new Vector3(0, velocidadRotacion * Time.deltaTime, 0), Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth salud = other.GetComponentInParent<PlayerHealth>();
            if (salud != null)
            {
                salud.ActivarInvisibilidad(duracionInvisibilidad);
                Destroy(gameObject);
            }
        }
    }
}