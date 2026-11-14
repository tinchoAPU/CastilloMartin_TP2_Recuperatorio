using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private int valorPuntos = 10;
    [SerializeField] private float velocidadRotacion = 180f;

    private void Update()
    {
        transform.Rotate(new Vector3(0, velocidadRotacion * Time.deltaTime, 0), Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.NotifyItemCollected(1, valorPuntos);
            }
            Destroy(gameObject);
        }
    }
}