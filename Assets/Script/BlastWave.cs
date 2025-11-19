using UnityEngine;
using System.Collections.Generic;

// Dziedziczy z SpellDamageInit
public class BlastWave : SpellDamageInit
{
    private List<GameObject> hitEnemies = new List<GameObject>();

    void Start()
    {
        // Fala natychmiastowo znika (np. po 0.1 sekundy)
        Destroy(gameObject, 0.1f);
    }

    // Używamy OnTriggerEnter, ponieważ Blast Wave jest dużym, natychmiastowym Triggerem
    void OnTriggerEnter(Collider other)
    {
        // Sprawdź, czy trafiliśmy we wroga i czy nie był już trafiony 
        if (other.CompareTag("Enemy") && !hitEnemies.Contains(other.gameObject))
        {
            Debug.Log($"Blast Wave AOE trafia {other.gameObject.name}. Zadano {damageAmount}.");

            // Zniszcz wroga
            Destroy(other.gameObject);
            hitEnemies.Add(other.gameObject);
        }
    }
}