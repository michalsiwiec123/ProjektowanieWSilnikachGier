using UnityEngine;

// Dziedziczy z SpellDamageInit, aby uzyskać dostęp do damageAmount
public class ProjectileBehaviour : SpellDamageInit
{
    void Start()
    {
        // Niszczy pocisk po 3 sekundach
        Destroy(gameObject, 3f);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log($"Fireball trafia! Zadano {damageAmount} obrażeń.");
            // Niszcz wroga od razu, brak Health
            Destroy(collision.gameObject);
            // Niszcz pocisk
            Destroy(gameObject);
        }
    }
}