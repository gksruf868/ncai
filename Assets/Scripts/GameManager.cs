using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private TMP_Text survivalTimeText;
    [SerializeField] private GameObject gameOverCanvas;

    public bool IsGameOver { get; private set; }
    public float SurvivalTime => survivalTime;

    private float survivalTime;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
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
