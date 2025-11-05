using UnityEngine;

// Ten atrybut pozwala na łatwe tworzenie obiektu ScriptableObject
// w menu Assetów (kliknięcie prawym przyciskiem myszy w oknie Projektu)
[CreateAssetMenu(fileName = "NewPlayerStats", menuName = "The Rogue Mage/Player Stats")]
public class PlayerStatsSO : ScriptableObject
{
    // Statystyki początkowe/bazowe gracza
    public float baseMoveSpeed = 5f;
    public int maxHealth = 100;
    public int maxMana = 100;

    // Możesz tu dodawać więcej statystyk, np. bazowe obrażenia zaklęć.
    // public float baseFireballDamage = 10f;
}