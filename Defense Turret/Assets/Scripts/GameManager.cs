using UnityEngine;
using UnityEngine.SceneManagement; // Avem nevoie de asta pentru Restart (opțional)

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton ca să îl putem accesa de oriunde

    [Header("Setări Joc")]
    public int lives = 7; // Numărul de inamici care pot trece
    public GameObject gameOverUI; // Aici tragi Panoul creat la Pasul 1

    private bool gameIsOver = false;

    void Awake()
    {
        // Asigurăm că există doar un singur GameManager
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        // Ne asigurăm că ecranul de Game Over e ascuns la început
        if (gameOverUI != null)
            gameOverUI.SetActive(false);
    }

    public void ReduceLives()
    {
        if (gameIsOver) return;

        lives--; // Scădem o viață
        Debug.Log("Vieti ramase: " + lives);

        if (lives <= 0)
        {
            EndGame();
        }
    }

    void EndGame()
    {
        gameIsOver = true;
        Debug.Log("GAME OVER!");

        // 1. Activăm ecranul de final
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }

        // 2. Oprim timpul (Jocul ia pauză totală)
        Time.timeScale = 0f;
    }
    public void RestartGame()
    {
        // 1. FOARTE IMPORTANT: Repornim timpul!
        // Deoarece l-am oprit cu Time.timeScale = 0 la Game Over, trebuie să îl facem 1 la loc.
        Time.timeScale = 1f;

        // 2. Reîncărcăm scena curentă
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}