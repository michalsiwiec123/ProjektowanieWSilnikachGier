using UnityEngine;

// Skrypt bazowy, który służy tylko do przekazania obrażeń ze SpellCastera
public class SpellDamageInit : MonoBehaviour
{
    protected float damageAmount;

    public void Initialize(float damage)
    {
        damageAmount = damage;
    }
}