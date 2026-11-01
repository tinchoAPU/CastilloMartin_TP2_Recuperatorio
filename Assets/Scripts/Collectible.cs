using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Collectible : MonoBehaviour
{   

    [SerializeField] private int valorPuntos = 10;

    void Update() {

        transform.Rotate(new Vector3(0, 3, 0), Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth salud = other.GetComponent<PlayerHealth>();
            if (salud != null)
            {
                salud.SumarPuntos(valorPuntos);
                Destroy(gameObject);
            }
        }
    }
}
