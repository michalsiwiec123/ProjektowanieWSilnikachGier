using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Referencja do ScriptableObject
    public PlayerStatsSO playerStats; // <--- DODAJ TO

    // Usunęliśmy 'public float moveSpeed = 5f;' na rzecz danych z SO

    private Rigidbody rb;
    private Vector2 movementInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("PlayerMovement wymaga komponentu Rigidbody na tym samym obiekcie.");
        }

        // Zabezpieczenie przed brakiem ScriptableObject
        if (playerStats == null)
        {
            Debug.LogError("PlayerStatsSO nie jest przypisany do PlayerMovement!");
        }
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        movementInput = new Vector2(moveX, moveY).normalized;
    }

    void FixedUpdate()
    {
        // Używamy moveSpeed z obiektu ScriptableObject!
        float currentMoveSpeed = playerStats != null ? playerStats.baseMoveSpeed : 5f; // Użyjemy domyślnej 5f jako fallback

        Vector3 newPosition = rb.position + new Vector3(movementInput.x, 0f, movementInput.y) * currentMoveSpeed * Time.fixedDeltaTime;

        rb.MovePosition(newPosition);
    }
}