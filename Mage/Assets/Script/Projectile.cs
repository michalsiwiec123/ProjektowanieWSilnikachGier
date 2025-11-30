using UnityEngine;

public class ProjectileBehaviour : SpellDamageInit
{
    void Start()
    {
        // Pocisk znika po 3 sekundach, jeśli nic nie trafi
        Destroy(gameObject, 3f);
    }

    // Używamy OnTriggerEnter, ponieważ wróg jest teraz Triggerem
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // 1. Zwiększenie licznika zabójstw w GameManagerze
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddKill();
            }

            Debug.Log($"Fireball trafia! Zadano {damageAmount} obrażeń. Wróg ginie natychmiast.");

            // 2. Niszcz wroga
            Destroy(other.gameObject);

            // 3. Niszcz pocisk
            Destroy(gameObject);
        }
    }
}