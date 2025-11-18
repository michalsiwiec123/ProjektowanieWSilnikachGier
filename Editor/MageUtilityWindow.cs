using UnityEditor;
using UnityEngine;

public class MageUtilityWindow : EditorWindow
{
    // Zmienna do przechowywania referencji do naszego ScriptableObject
    private PlayerStatsSO playerStats;

    // 1. Definicja menu, pod którym okno będzie dostępne
    [MenuItem("The Rogue Mage/Mage Utility Window")]
    public static void ShowWindow()
    {
        // Tworzenie i wyświetlanie okna
        GetWindow<MageUtilityWindow>("Mage Utility");
    }

    // 2. Metoda wywoływana, gdy okno jest rysowane
    private void OnGUI()
    {
        GUILayout.Label("Player Stats Quick Editor", EditorStyles.boldLabel);

        EditorGUILayout.Space();

        // Pole do przeciągania i upuszczania ScriptableObject
        playerStats = (PlayerStatsSO)EditorGUILayout.ObjectField(
            "Stats Asset",
            playerStats,
            typeof(PlayerStatsSO),
            false
        );

        // Jeśli mamy przypisany PlayerStatsSO, wyświetlamy jego pola
        if (playerStats != null)
        {
            EditorGUILayout.Space();
            GUILayout.Label("Quick Stats Adjustment", EditorStyles.helpBox);

            // Wyświetlanie i edytowanie pól
            playerStats.baseMoveSpeed = EditorGUILayout.FloatField("Base Move Speed", playerStats.baseMoveSpeed);
            playerStats.maxHealth = EditorGUILayout.IntField("Max Health", playerStats.maxHealth);
            playerStats.maxMana = EditorGUILayout.IntField("Max Mana", playerStats.maxMana);

            EditorGUILayout.Space();

            // Przycisk do zapisywania zmian na dysku
            if (GUILayout.Button("Save Changes"))
            {
                // Wymuś zapisanie zmian w ScriptableObject na dysku
                EditorUtility.SetDirty(playerStats);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                Debug.Log("Player Stats zostały zapisane.");
            }
        }
        else
        {
            EditorGUILayout.HelpBox("Przeciągnij tutaj swój PlayerStatsSO asset, aby rozpocząć edycję.", MessageType.Info);
        }
    }
}