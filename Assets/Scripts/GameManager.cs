using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private TMP_Text survivalTimeText;
    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private GameObject startMenuCanvas;
    [SerializeField] private Button startButton;

    public bool IsGameOver { get; private set; }
    public bool HasStarted { get; private set; }
    public float SurvivalTime => survivalTime;

    private float survivalTime;

    private void Awake()
    {
        Instance = this;

        if (startButton != null)
        {
            startButton.onClick.AddListener(StartGame);
        }
    }

    private void Update()
    {
        if (!HasStarted) return;

        if (IsGameOver)
        {
            if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
            {
                Restart();
            }
            return;
        }

        survivalTime += Time.deltaTime;
        survivalTimeText.text = survivalTime.ToString("F1") + "s";
    }

    public void StartGame()
    {
        if (HasStarted) return;

        HasStarted = true;

        if (startMenuCanvas != null)
        {
            startMenuCanvas.SetActive(false);
        }
    }

    public void GameOver()
    {
        if (IsGameOver) return;

        IsGameOver = true;
        Debug.Log("Game Over");

        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true);
        }
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
