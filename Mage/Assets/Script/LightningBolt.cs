using UnityEngine;

public class ChainLightning : SpellDamageInit
{
    private int chainCount = 3;
    private float chainRange = 8f;

    void Start()
    {
        // Błyskawica znika po 3 sekundach, jeśli nie trafi w cel
        Destroy(gameObject, 3f);
    }

    // Używamy OnTriggerEnter, ponieważ wróg jest teraz Triggerem
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            StartChain(other.gameObject);

            // Zniszcz pocisk-matkę, gdy tylko trafi w cel
            Destroy(gameObject);
        }
    }

    private void StartChain(GameObject target)
    {
        ChainJump(target, 1);
    }

    private void ChainJump(GameObject currentTarget, int currentChain)
    {
        if (currentChain > chainCount) return;

        Debug.Log($"Łańcuch: {currentChain}/{chainCount}. Trafiono {currentTarget.name}. Zadano {damageAmount}. Wróg ginie natychmiast.");

        // Zniszczenie bieżącego celu i inkrementacja licznika
        if (currentTarget.CompareTag("Enemy"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddKill();
            }
            Destroy(currentTarget);
        }

        if (currentChain == chainCount) return;

        // Szukaj kolejnego celu w zasięgu
        Collider[] hitColliders = Physics.OverlapSphere(currentTarget.transform.position, chainRange);

        GameObject nextTarget = null;
        float minDistance = float.MaxValue;

        foreach (var hit in hitColliders)
        {
            // Znajdź najbliższego wroga, który nie jest obecnym celem
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

        // Przeskocz do następnego celu
        if (nextTarget != null)
        {
            ChainJump(nextTarget, currentChain + 1);
        }
    }
}