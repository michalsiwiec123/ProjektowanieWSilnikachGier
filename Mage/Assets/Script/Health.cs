using UnityEngine;

public class Health : MonoBehaviour
{
    // Maksymalne HP (można zmienić w Inspektorze)
    public float maxHealth = 30f;
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        // Upewnij się, że collider Goblina jest ustawiony jako ZWYKŁY COLLIDER (Is Trigger: ODZNACZONE)
    }

    // Metoda publiczna wywoływana przez pocisk do zadawania obrażeń
    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        Debug.Log(gameObject.name + $" otrzymał {damageAmount} obrażeń. Pozostało HP: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Używamy OnTriggerEnter, aby Goblin mógł dotknąć Gracza (jeśli jest potrzebna logika kontaktu)
    // Ale główna logika otrzymywania obrażeń jest w TakeDamage()

    private void Die()
    {
        Debug.Log(gameObject.name + " został zniszczony!");
        // Zniszcz obiekt wroga
        Destroy(gameObject);
    }
}