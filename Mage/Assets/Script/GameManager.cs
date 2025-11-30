using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Dodaj, jeśli używasz TextMeshPro do UI

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject gameOverPanel;
    private int sceneToReloadIndex = 0;

    // --- NOWY BLOK KODU ---
    [Header("Statystyki Gry")]
    public TextMeshProUGUI killCountText; // Przypisz pole tekstowe z UI
    private int killCount = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Upewnij się, że czas jest wznowiony na początku nowej gry
        Time.timeScale = 1f;
        UpdateKillCountUI(); // Wyświetlenie 0 na start
    }

    // Metoda publiczna wywoływana przez pociski/zaklęcia
    public void AddKill()
    {
        killCount++;
        UpdateKillCountUI();
    }

    private void UpdateKillCountUI()
    {
        if (killCountText != null)
        {
            killCountText.text = "Zabójstwa: " + killCount;
        }
    }

    // --- KONIEC NOWEGO BLOKU KODU ---

    public void EndGame()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        // Opcjonalnie: Zatrzymanie czasu
        Time.timeScale = 0f;

        // Możesz tutaj wyświetlić końcowy KillCount na ekranie Game Over
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        // Reset licznika jest automatyczny, ponieważ obiekt GameManager jest niszczony 
        // i tworzony od nowa wraz z wczytaniem sceny.
        SceneManager.LoadScene(sceneToReloadIndex);
    }
}