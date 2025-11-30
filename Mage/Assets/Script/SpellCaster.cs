using UnityEngine;
using System.Collections.Generic;

public class SpellCaster : MonoBehaviour
{
    [Header("Ustawienia Zaklęć")]
    public SpellSO fireballSpell;       // Klawisz 1
    public SpellSO lightningSpell;      // Klawisz 2
    public SpellSO blastWaveSpell;      // Klawisz 3

    public Transform castPoint;

    private Dictionary<string, float> cooldownTimers = new Dictionary<string, float>();
    private PlayerStatus playerStatus;

    void Start()
    {
        // 1. Sprawdzenie zależności
        playerStatus = GetComponent<PlayerStatus>();
        if (playerStatus == null)
        {
            Debug.LogError("SpellCaster wymaga komponentu PlayerStatus na tym samym obiekcie! Wyłączam skrypt.");
            enabled = false;
            return;
        }

        // 2. Utworzenie CastPoint, jeśli go brakuje
        if (castPoint == null)
        {
            GameObject cp = new GameObject("CastPoint");
            cp.transform.SetParent(transform);
            cp.transform.localPosition = new Vector3(0, 1.0f, 0.75f);
            castPoint = cp.transform;
        }

        // 3. Inicjalizacja cooldownów
        InitializeCooldowns();
    }

    private void InitializeCooldowns()
    {
        // Dodajemy tylko te zaklęcia, które zostały przypisane w Inspektorze
        if (fireballSpell != null) cooldownTimers[fireballSpell.spellName] = 0f;
        if (lightningSpell != null) cooldownTimers[lightningSpell.spellName] = 0f;
        if (blastWaveSpell != null) cooldownTimers[blastWaveSpell.spellName] = 0f;
    }

    void Update()
    {
        UpdateCooldowns();

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TryCastSpell(fireballSpell);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            TryCastSpell(lightningSpell);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            TryCastSpell(blastWaveSpell);
        }
    }

    private void UpdateCooldowns()
    {
        // Zmniejszanie czasu cooldownu
        List<string> keys = new List<string>(cooldownTimers.Keys);
        foreach (string spellName in keys)
        {
            if (cooldownTimers[spellName] > 0)
            {
                cooldownTimers[spellName] -= Time.deltaTime;
            }
        }
    }

    private void TryCastSpell(SpellSO spell)
    {
        if (spell == null || playerStatus == null) return;
        string spellName = spell.spellName;

        // 1. Sprawdzenie Cooldownu
        if (!cooldownTimers.ContainsKey(spellName) || cooldownTimers[spellName] > 0)
        {
            Debug.Log($"Zaklęcie {spellName} na cooldownie.");
            return;
        }

        // 2. Sprawdzenie i Konsumpcja Many
        if (!playerStatus.ConsumeMana(spell.manaCost))
        {
            Debug.Log($"Za mało Many, aby rzucić {spellName}!");
            return;
        }

        // 3. Rzucenie Zaklęcia
        GameObject spellEffectGO = Instantiate(
            spell.projectilePrefab,
            castPoint.position,
            transform.rotation
        );

        Rigidbody rb = spellEffectGO.GetComponent<Rigidbody>();
        SpellDamageInit spellInit = spellEffectGO.GetComponent<SpellDamageInit>();

        // Nadanie prędkości
        if (rb != null && spell.projectileSpeed > 0.01f)
        {
            rb.linearVelocity = transform.forward * spell.projectileSpeed;
        }

        // Inicjalizacja obrażeń (przekazanie wartości do skryptów kolizji)
        if (spellInit != null)
        {
            spellInit.Initialize(spell.baseDamage);
        }

        // 4. Ustawienie Cooldownu
        cooldownTimers[spellName] = spell.cooldownTime;
        Debug.Log($"Rzucono: {spellName}! Koszt: {spell.manaCost} Many.");
    }
}