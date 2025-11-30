using UnityEngine;

public class PlayerAiming : MonoBehaviour
{
    private Camera mainCamera;

    void Awake()
    {
        // 1. Pobierz referencję do głównej kamery
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Brak głównej kamery (MainCamera). Upewnij się, że kamera ma tag 'MainCamera'.");
        }
    }

    void Update()
    {
        // 2. Pobierz pozycję kursora myszy w przestrzeni ekranu
        Vector3 mouseScreenPosition = Input.mousePosition;

        // Używamy Raycastingu, aby znaleźć punkt na płaszczyźnie gry

        // Tworzymy promień wychodzący z kamery przez kursor myszy
        Ray ray = mainCamera.ScreenPointToRay(mouseScreenPosition);

        // Definiujemy płaszczyznę "ziemi", na której stoi gracz (płaszczyzna XZ, normalna to Vector3.up)
        // Używamy transform.position.y, aby płaszczyzna była na wysokości gracza
        Plane groundPlane = new Plane(Vector3.up, transform.position.y);

        float rayDistance;
        Vector3 pointToLookAt = transform.position;

        // Sprawdź, czy promień przecina płaszczyznę
        if (groundPlane.Raycast(ray, out rayDistance))
        {
            // 3. Oblicz punkt przecięcia (target w świecie gry)
            pointToLookAt = ray.GetPoint(rayDistance);
        }

        // 4. Oblicz wektor kierunku od gracza do kursora
        Vector3 direction = pointToLookAt - transform.position;
        direction.y = 0; // Upewnij się, że obrót jest tylko wokół osi Y

        // 5. Zastosuj obrót
        if (direction != Vector3.zero)
        {
            // Quaternion.LookRotation tworzy rotację, która patrzy w danym kierunku (direction)
            // Utrzymując "górę" obiektu w kierunku Vector3.up (czyli oś Y)
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

            // Płynny obrót (opcjonalny, ale ładniejszy)
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);

            // Aby obrót był natychmiastowy, użyj:
            // transform.rotation = targetRotation;
        }
    }
}