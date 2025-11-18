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

    void Start()
    {
        if (castPoint == null)
        {
            GameObject cp = new GameObject("CastPoint");
            cp.transform.SetParent(transform);
            cp.transform.localPosition = new Vector3(0, 1.0f, 0.75f);
            castPoint = cp.transform;
        }

        InitializeCooldowns();
    }

    private void InitializeCooldowns()
    {
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
        if (spell == null) return;
        string spellName = spell.spellName;

        if (!cooldownTimers.ContainsKey(spellName) || cooldownTimers[spellName] > 0)
        {
            return;
        }

        GameObject spellEffectGO = Instantiate(
            spell.projectilePrefab,
            castPoint.position,
            transform.rotation
        );

        Rigidbody rb = spellEffectGO.GetComponent<Rigidbody>();

        // Inicjalizacja obrażeń (bez systemu Health)
        SpellDamageInit spellInit = spellEffectGO.GetComponent<SpellDamageInit>();

        if (rb != null && spell.projectileSpeed > 0.01f)
        {
            rb.velocity = transform.forward * spell.projectileSpeed;
        }

        if (spellInit != null)
        {
            spellInit.Initialize(spell.baseDamage);
        }

        cooldownTimers[spellName] = spell.cooldownTime;
    }
}