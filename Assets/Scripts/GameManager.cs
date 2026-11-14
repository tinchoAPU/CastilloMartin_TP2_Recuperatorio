using System;
using UnityEngine;

public enum GameState
{
    Start,
    Playing,
    Paused,
    Victory,
    GameOver,
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("State Settings")]
    [SerializeField] private GameState currentState;
    public GameState CurrentState => currentState;

    [Header("Victory & Score Settings")]
    [SerializeField] private int itemsToWin = 10;
    [SerializeField] private int itemsCollected = 0;
    [SerializeField] private int currentScore = 0;

    public int ItemsToWin => itemsToWin;
    public int ItemsCollected => itemsCollected;
    public int CurrentScore => currentScore;

    // Eventos listos para conectar el HUD (UIManager) más adelante
    public event Action<GameState> OnChangeState;
    public event Action<int, int> OnItemsChange; // (actuales, objetivo)
    public event Action<int> OnScoreChange;      // (puntajeTotal)

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        itemsCollected = 0;
        currentScore = 0;

        OnItemsChange?.Invoke(itemsCollected, itemsToWin);
        OnScoreChange?.Invoke(currentScore);
        ChangeState(GameState.Playing);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Paused();
        }
    }

    public void ChangeState(GameState newState)
    {
        if (currentState == newState && newState != GameState.Playing) return;
        currentState = newState;

        switch (currentState)
        {
            case GameState.Start:
                Time.timeScale = 1f;
                Debug.Log("Inicio el juego");
                break;
            case GameState.Playing:
                Time.timeScale = 1f;
                Debug.Log("<color=green>ESTADO: Jugando</color>");
                break;
            case GameState.Paused:
                Time.timeScale = 0f;
                Debug.Log("<color=yellow>ESTADO: Pausa</color>");
                break;
            case GameState.Victory:
                Time.timeScale = 0f;
                Debug.Log("<color=cyan>ESTADO: Victoria - ¡Recolectaste todos los objetos!</color>");
                break;
            case GameState.GameOver:
                Time.timeScale = 0f;
                Debug.Log("<color=red>ESTADO: Game Over</color>");
                break;
        }

        OnChangeState?.Invoke(currentState);
    }

    public void Paused()
    {
        if (currentState == GameState.Playing)
        {
            ChangeState(GameState.Paused);
        }
        else if (currentState == GameState.Paused)
        {
            ChangeState(GameState.Playing);
        }
    }

    // Notificaciones
    public void NotifyPlayerDead()
    {
        if (currentState != GameState.Playing) return;

        ChangeState(GameState.GameOver);
    }

    public void NotifyItemCollected(int cantidad = 1, int puntos = 10)
    {
        if (currentState != GameState.Playing) return;

        itemsCollected += cantidad;
        currentScore += puntos;

        Debug.Log($"Monedas: {itemsCollected}/{itemsToWin} | Puntos: {currentScore}");

        OnItemsChange?.Invoke(itemsCollected, itemsToWin);
        OnScoreChange?.Invoke(currentScore);

        if (itemsCollected >= itemsToWin)
        {
            ChangeState(GameState.Victory);
        }
    }
}