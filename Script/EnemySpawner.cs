using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Ustawienia Spawnu")]
    public GameObject enemyPrefab;
    public float spawnInterval = 5f;
    public float startDelay = 2f;

    [Header("Lokalizacja Spawnu")]
    [Tooltip("Zakres X (Min/Max) dla pozycji spawnu.")]
    public Vector2 spawnRangeX = new Vector2(-10f, 10f);

    [Tooltip("Zakres Z (Min/Max) dla pozycji spawnu.")]
    public Vector2 spawnRangeZ = new Vector2(-10f, 10f);

    [Header("Detekcja Podłogi")]
    [Tooltip("Warstwa, na której znajduje się podłoga (np. 'Ground').")]
    public LayerMask groundLayer;

    // Wysokość, z której rzucamy promień w dół (musi być wyżej niż najwyższy punkt podłogi)
    private const float RaycastHeight = 20f;

    void Start()
    {
        InvokeRepeating("SpawnEnemy", startDelay, spawnInterval);
    }

    void SpawnEnemy()
    {
        // 1. Losowanie pozycji X i Z
        float randomX = Random.Range(spawnRangeX.x, spawnRangeX.y);
        float randomZ = Random.Range(spawnRangeZ.x, spawnRangeZ.y);

        // Ustawienie punktu początkowego promienia (wysoko nad mapą)
        Vector3 rayStartPoint = new Vector3(randomX, RaycastHeight, randomZ);

        // Obiekt do przechowywania informacji o trafieniu
        RaycastHit hit;

        // 2. Raycasting: rzucamy promień w dół (-Vector3.up)
        if (Physics.Raycast(rayStartPoint, -Vector3.up, out hit, RaycastHeight * 2, groundLayer))
        {
            // Promień TRAFIŁ w obiekt na warstwie 'groundLayer'

            // Pozycja spawnu to pozycja trafienia, ale podniesiona o połowę wysokości wroga
            // (zakładając, że wróg ma wysokość 1 jednostki, a pivot jest na środku)
            float enemyHeightOffset = 0.5f; // Zmień, jeśli Twój wróg ma inną skalę/pivot

            Vector3 spawnPosition = hit.point + Vector3.up * enemyHeightOffset;

            // 3. Spawnowanie wroga
            if (enemyPrefab != null)
            {
                Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            }
        }
        else
        {
            // Opcjonalnie: Jeśli nie znaleziono podłogi, możemy próbować ponownie
            // Debug.LogWarning("Nie znaleziono podłogi na wylosowanej pozycji. Pomijam spawn.");
        }
    }
}