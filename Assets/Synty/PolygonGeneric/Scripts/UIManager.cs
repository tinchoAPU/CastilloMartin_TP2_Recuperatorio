using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Referencias de Lógica")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private HealingStation healingStation;

    [Header("Textos del HUD")]
    [SerializeField] private TextMeshProUGUI textoSalud;
    [SerializeField] private TextMeshProUGUI textoMonedas;
    [SerializeField] private TextMeshProUGUI textoPuntos;
    [SerializeField] private TextMeshProUGUI textoEstacion;

    [Header("Paneles de Estado")]
    [SerializeField] private GameObject panelPausa;
    [SerializeField] private GameObject panelVictoria;
    [SerializeField] private GameObject panelGameOver;

    private void Start()
    {
        if (playerHealth != null)
        {
            ActualizarSalud(playerHealth.SaludActual, playerHealth.SaludMaxima);
            playerHealth.OnHealthChanged += ActualizarSalud;
        }

        if (GameManager.Instance != null)
        {
            ActualizarMonedas(GameManager.Instance.ItemsCollected, GameManager.Instance.ItemsToWin);
            ActualizarPuntos(GameManager.Instance.CurrentScore);
            ActualizarPanelesPorEstado(GameManager.Instance.CurrentState);

            GameManager.Instance.OnItemsChange += ActualizarMonedas;
            GameManager.Instance.OnScoreChange += ActualizarPuntos;
            GameManager.Instance.OnChangeState += ActualizarPanelesPorEstado;
        }

        if (healingStation != null)
        {
            ActualizarEstadoEstacion(true);
            healingStation.OnEstadoEstacionCambiado += ActualizarEstadoEstacion;
        }
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
            playerHealth.OnHealthChanged -= ActualizarSalud;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnItemsChange -= ActualizarMonedas;
            GameManager.Instance.OnScoreChange -= ActualizarPuntos;
            GameManager.Instance.OnChangeState -= ActualizarPanelesPorEstado;
        }

        if (healingStation != null)
            healingStation.OnEstadoEstacionCambiado -= ActualizarEstadoEstacion;
    }

    private void ActualizarSalud(int actual, int maxima)
    {
        if (textoSalud != null)
            textoSalud.text = $"Salud: {actual} / {maxima}";
    }

    private void ActualizarMonedas(int actuales, int objetivo)
    {
        if (textoMonedas != null)
            textoMonedas.text = $"Monedas: {actuales} / {objetivo}";
    }

    private void ActualizarPuntos(int puntos)
    {
        if (textoPuntos != null)
            textoPuntos.text = $"Puntos: {puntos}";
    }

    private void ActualizarEstadoEstacion(bool disponible)
    {
        if (textoEstacion != null)
        {
            textoEstacion.text = disponible ? "Estación: Lista" : "Estación en Recarga";
            textoEstacion.color = disponible ? Color.green : new Color(1f, 0.5f, 0f);
        }
    }

    private void ActualizarPanelesPorEstado(GameState estado)
    {
        if (panelPausa != null) panelPausa.SetActive(estado == GameState.Paused);
        if (panelVictoria != null) panelVictoria.SetActive(estado == GameState.Victory);
        if (panelGameOver != null) panelGameOver.SetActive(estado == GameState.GameOver);

        // Libera el cursor en menús para poder hacer clic en los botones
        bool mostrarCursor = (estado == GameState.Paused || estado == GameState.Victory || estado == GameState.GameOver);
        Cursor.visible = mostrarCursor;
        Cursor.lockState = mostrarCursor ? CursorLockMode.None : CursorLockMode.Locked;
    }

    // Métodos públicos para conectar en el evento OnClick() de los botones
    public void BotonReanudar()
    {
        GameManager.Instance?.Paused();
    }

    public void BotonReiniciarNivel()
    {
        Time.timeScale = 1f;
        if (GameManager.Instance != null)
        {
            Destroy(GameManager.Instance.gameObject);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}