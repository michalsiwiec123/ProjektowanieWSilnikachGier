using UnityEngine;
using UnityEditor;
using System.Linq;

public class SpellEditorWindow : EditorWindow
{
    private SpellSO selectedSpell;

    // Lista wszystkich assetów SpellSO znalezionych w projekcie
    private string[] allSpellAssetPaths;
    private string[] allSpellNames;
    private int selectedIndex = 0;

    [MenuItem("The Rogue Mage/Spell Manager")]
    public static void ShowWindow()
    {
        GetWindow<SpellEditorWindow>("Spell Manager");
    }

    private void OnEnable()
    {
        // Odśwież listę za każdym razem, gdy okno jest włączane
        RefreshSpellList();
    }

    private void RefreshSpellList()
    {
        // 1. Znajdź GUIDy wszystkich assetów typu SpellSO
        string[] guids = AssetDatabase.FindAssets("t:SpellSO");

        // 2. Zamień GUIDy na ścieżki i nazwy
        allSpellAssetPaths = guids.Select(AssetDatabase.GUIDToAssetPath).ToArray();
        allSpellNames = allSpellAssetPaths.Select(path => System.IO.Path.GetFileNameWithoutExtension(path)).ToArray();

        // Dodaj opcję 'Utwórz nowy' na górę listy
        allSpellNames = new[] { "--- Utwórz nowy ---" }.Concat(allSpellNames).ToArray();
        selectedIndex = 0;
        selectedSpell = null;
    }


    private void OnGUI()
    {
        GUILayout.Label("Zarządzanie Zaklęciami", EditorStyles.boldLabel);

        // 1. Przycisk odświeżania listy
        if (GUILayout.Button("Odśwież listę zaklęć"))
        {
            RefreshSpellList();
        }

        // 2. Menu rozwijane z nazwami wszystkich znalezionych SpellSO
        selectedIndex = EditorGUILayout.Popup("Wybierz zaklęcie:", selectedIndex, allSpellNames);

        // Akcja po wybraniu z listy
        if (selectedIndex == 0)
        {
            selectedSpell = null;
        }
        else
        {
            // Ładowanie wybranego assetu
            string path = allSpellAssetPaths[selectedIndex - 1];
            selectedSpell = AssetDatabase.LoadAssetAtPath<SpellSO>(path);
        }

        // 3. Przycisk do tworzenia nowego SpellSO (gdy wybrano 'Utwórz nowy')
        if (selectedIndex == 0 && GUILayout.Button("Utwórz nowy SpellSO"))
        {
            CreateNewSpellAsset();
        }

        EditorGUILayout.Space();

        // 4. Edycja właściwości wybranego zaklęcia
        if (selectedSpell != null)
        {
            GUILayout.Label("Edycja: " + selectedSpell.name, EditorStyles.boldLabel);

            SerializedObject serializedObject = new SerializedObject(selectedSpell);

            // Rysowanie pól
            SerializedProperty property = serializedObject.GetIterator();
            while (property.NextVisible(true))
            {
                EditorGUILayout.PropertyField(property, true);
            }

            serializedObject.ApplyModifiedProperties();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(selectedSpell);
            }
        }
    }

    private void CreateNewSpellAsset()
    {
        SpellSO newSpell = CreateInstance<SpellSO>();

        string uniquePath = AssetDatabase.GenerateUniqueAssetPath("Assets/NewSpellData.asset");
        AssetDatabase.CreateAsset(newSpell, uniquePath);
        AssetDatabase.SaveAssets();

        RefreshSpellList();

        // Wybór nowego zaklęcia na liście
        selectedIndex = allSpellNames.Length - 1;
        selectedSpell = newSpell;

        EditorGUIUtility.PingObject(newSpell);
    }
}