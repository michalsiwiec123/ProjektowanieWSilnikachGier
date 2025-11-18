using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 3f;
    [Tooltip("Szybkość obrotu (wyższa = szybsza reakcja)")]
    public float rotationSpeed = 10f; // <--- NOWA ZMIENNA DO KONTROLI SZYBKOŚCI OBROTU

    private Transform playerTarget;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.velocity = Vector3.zero;
        }

        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            playerTarget = player.transform;
        }
    }

    void FixedUpdate()
    {
        if (playerTarget != null && rb != null)
        {
            // 1. Obliczenie wektora kierunku i normalizacja
            Vector3 directionToPlayer = (playerTarget.position - transform.position);
            directionToPlayer.y = 0;

            // Tylko normalizuj, jeśli kierunek jest niezerowy
            if (directionToPlayer.sqrMagnitude > 0.01f)
            {
                directionToPlayer.Normalize();

                // 2. Ustawienie ruchu
                Vector3 newVelocity = directionToPlayer * moveSpeed;
                newVelocity.y = rb.velocity.y;
                rb.velocity = newVelocity;

                // 3. LOGIKA OBROTU (NOWY BLOK KODU)
                // Obliczanie docelowej rotacji, która patrzy w kierunku ruchu (directionToPlayer)
                Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer, Vector3.up);

                // Płynne obracanie w kierunku celu
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    Time.fixedDeltaTime * rotationSpeed // Użyj FixedDeltaTime dla FixedUpdate
                );
            }
            else
            {
                // Zatrzymanie ruchu, jeśli wróg jest bardzo blisko
                rb.velocity = Vector3.zero;
            }
        }
    }
}