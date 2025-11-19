using UnityEngine;

[CreateAssetMenu(fileName = "NewSpell", menuName = "The Rogue Mage/Spell Data")]
public class SpellSO : ScriptableObject
{
    public string spellName = "Fireball";
    public float baseDamage = 15f;
    public float cooldownTime = 1f; // Cooldown zaklęcia w sekundach
    public float projectileSpeed = 10f;
    public GameObject projectilePrefab; // Prefab pocisku

    // Opcjonalnie: koszt many
    // public int manaCost = 10;
}