using UnityEngine;
using System.Collections.Generic;

public class BlastWave : SpellDamageInit
{
    // Lista już trafionych, aby uniknąć wielokrotnych obrażeń w tym samym kadrze
    private List<GameObject> hitEnemies = new List<GameObject>();

    void Start()
    {
        // Fala natychmiastowo znika (np. po 0.1 sekundy)
        Destroy(gameObject, 0.1f);
    }

    // Używamy OnTriggerEnter, ponieważ wróg jest teraz Triggerem
    void OnTriggerEnter(Collider other)
    {
        // Sprawdź, czy trafiliśmy we wroga i czy nie był już trafiony 
        if (other.CompareTag("Enemy") && !hitEnemies.Contains(other.gameObject))
        {
            Debug.Log($"Blast Wave AOE trafia {other.gameObject.name}. Zadano {damageAmount}. Wróg ginie natychmiast.");

            // 1. Zwiększenie licznika zabójstw
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddKill();
            }

            // 2. Zniszcz wroga
            Destroy(other.gameObject);
            hitEnemies.Add(other.gameObject);
        }
    }
}