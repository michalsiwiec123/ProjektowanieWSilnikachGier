using UnityEngine;

// Dziedziczy z SpellDamageInit
public class ChainLightning : SpellDamageInit
{
    private int chainCount = 3;
    private float chainRange = 8f;

    void Start()
    {
        // Błyskawica istnieje tylko przez chwilę, zanim zostanie zniszczona przez kolizję
        Destroy(gameObject, 3f);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            StartChain(collision.gameObject);
            // Zniszcz pocisk-matkę
            Destroy(gameObject);
        }
    }

    private void StartChain(GameObject target)
    {
        // Rozpoczynamy łańcuch
        ChainJump(target, 1);
    }

    private void ChainJump(GameObject currentTarget, int currentChain)
    {
        if (currentChain > chainCount) return;

        Debug.Log($"Łańcuch: {currentChain}/{chainCount}. Trafiono {currentTarget.name}. Zadano {damageAmount}.");

        // Zniszcz bieżący cel
        if (currentTarget.CompareTag("Enemy"))
        {
            Destroy(currentTarget);
        }

        if (currentChain == chainCount) return;

        // Szukaj kolejnego celu
        Collider[] hitColliders = Physics.OverlapSphere(currentTarget.transform.position, chainRange);

        GameObject nextTarget = null;
        float minDistance = float.MaxValue;

        foreach (var hit in hitColliders)
        {
            // Znajdź najbliższego, nie trafionego jeszcze wroga
            if (hit.CompareTag("Enemy") && hit.gameObject != currentTarget)
            {
                float distance = Vector3.Distance(currentTarget.transform.position, hit.transform.position);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    nextTarget = hit.gameObject;
                }
            }
        }

        if (nextTarget != null)
        {
            // Opcjonalnie: wizualizacja (Debug.DrawLine)
            ChainJump(nextTarget, currentChain + 1);
        }
    }
}