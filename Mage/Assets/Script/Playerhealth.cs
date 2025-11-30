using UnityEngine;
using TMPro;

public class PlayerStatus : MonoBehaviour
{
    // Używamy PlayerStatsSO dla wartości bazowych (maxHealth z SO musi być 100)
    public PlayerStatsSO playerStats;

    [Header("UI Status")]
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI manaText;

    private float currentHealth;
    private float currentMana;

    [Header("Ustawienia Many")]
    public float manaRegenRate = 5f; // Mana regenerowana na sekundę

    [Header("Ustawienia Zdrowia")]
    public float damageOnContact = 50f; // 50 obrażeń = dwa hity
    public float healthRegenAmount = 1f; // Ilość HP regenerowana
    public float healthRegenInterval = 2f; // Co ile sekund regeneracja
    public float invulnerabilityTime = 1f; // Nietykalność po trafieniu

    private float invulnerabilityTimer;
    private float healthRegenTimer;

    void Start()
    {
        if (playerStats == null)
        {
            Debug.LogError("PlayerStatsSO nie przypisany! Ustaw w Inspektorze.");
            return;
        }

        // Zapewnienie, że maxHealth w SO jest ustawione na 100
        playerStats.maxHealth = 100;

        currentHealth = playerStats.maxHealth;
        currentMana = playerStats.maxMana;

        UpdateUI();
    }

    void Update()
    {
        // 1. Regeneracja Many
        if (currentMana < playerStats.maxMana)
        {
            currentMana += manaRegenRate * Time.deltaTime;
            currentMana = Mathf.Min(currentMana, playerStats.maxMana);
            UpdateUI();
        }

        // 2. Regeneracja Zdrowia
        if (currentHealth < playerStats.maxHealth)
        {
            healthRegenTimer -= Time.deltaTime;
            if (healthRegenTimer <= 0)
            {
                currentHealth += healthRegenAmount;
                currentHealth = Mathf.Min(currentHealth, playerStats.maxHealth);
                healthRegenTimer = healthRegenInterval; // Reset timera
                UpdateUI();
            }
        }

        // 3. Aktualizacja Timera Nietykalności
        if (invulnerabilityTimer > 0)
        {
            invulnerabilityTimer -= Time.deltaTime;
        }
    }

    // Metoda zużycia many (wywoływana przez SpellCaster)
    public bool ConsumeMana(float amount)
    {
        if (currentMana >= amount)
        {
            currentMana -= amount;
            UpdateUI();
            return true;
        }
        return false;
    }

    // Otrzymywanie obrażeń (wywoływane przez Trigger z wrogiem)
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            TakeDamage(damageOnContact);
        }
    }

    public void TakeDamage(float amount)
    {
        if (invulnerabilityTimer > 0)
        {
            return; // Gracz jest nietykalny
        }

        currentHealth -= amount;
        invulnerabilityTimer = invulnerabilityTime; // Ustaw timer nietykalności

        Debug.Log($"Gracz otrzymał {amount} obrażeń. Zostało {currentHealth} HP.");
        UpdateUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("KONIEC GRY! Mag został pokonany.");

        if (GameManager.Instance != null)
        {
            GameManager.Instance.EndGame();
        }

        Destroy(gameObject);
    }

    private void UpdateUI()
    {
        if (healthText != null)
        {
            // Wyświetlanie aktualnego HP na 100
            healthText.text = $"HP: {(int)currentHealth}/{playerStats.maxHealth}";
        }
        if (manaText != null)
        {
            manaText.text = $"Mana: {(int)currentMana}/{playerStats.maxMana}";
        }
    }
}