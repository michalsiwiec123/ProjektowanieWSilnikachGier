using UnityEngine;

[CreateAssetMenu(fileName = "NewSpell", menuName = "The Rogue Mage/Spell Data")]
public class SpellSO : ScriptableObject
{
    public string spellName = "Fireball";
    public float baseDamage = 15f;
    public float cooldownTime = 1f;
    public float projectileSpeed = 15f;
    public GameObject projectilePrefab;

    // --- DODAJ TEN WIERSZ ---
    [Header("Koszt")]
    public float manaCost = 10f; // Koszt many, który brakował
    // -------------------------
}