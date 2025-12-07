using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float tileSize = 0.5f;

    [Header("Push Settings")]
    public LayerMask pushableLayer;

    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 moveInput;
    private float lastMoveX = 1f;

    private SeasonManager seasonManager;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();

        seasonManager = FindObjectOfType<SeasonManager>();
        if (seasonManager == null)
            Debug.LogWarning("No SeasonManager found in scene. Seasonal push rules won't work.");
    }

    void Update()
    {
        moveInput = Vector2.zero;

        // Movement input (new Input System)
        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed) moveInput.x = -1f;
            if (Keyboard.current.dKey.isPressed) moveInput.x = 1f;
            if (Keyboard.current.wKey.isPressed) moveInput.y = 1f;
            if (Keyboard.current.sKey.isPressed) moveInput.y = -1f;
        }

        // Update last horizontal facing
        if (moveInput.x != 0)
            lastMoveX = Mathf.Sign(moveInput.x);

        // Set Blend Tree parameters
        anim.SetFloat("MoveX", moveInput.x != 0 ? moveInput.x : lastMoveX);
        anim.SetFloat("Speed", moveInput.magnitude);

        // Push input
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            anim.SetFloat("PushDirX", lastMoveX);
            anim.SetTrigger("Push");

            TryPushObject();
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveInput.normalized * moveSpeed;
    }

    void TryPushObject()
    {
        Vector2 pushDir = moveInput != Vector2.zero ? moveInput.normalized : new Vector2(lastMoveX, 0f);

        RaycastHit2D hit = Physics2D.Raycast(rb.position, pushDir, tileSize, pushableLayer);
        if (hit.collider != null)
        {
            Pushable pushable = hit.collider.GetComponent<Pushable>();
            if (pushable != null && (seasonManager == null || pushable.IsPushable()))
            {
                Vector3 target = SnapToGrid(hit.collider.transform.position + (Vector3)(pushDir * tileSize));

                // Optional: clamp inside map bounds if needed
                hit.collider.transform.position = target;
            }
        }
    }

    Vector3 SnapToGrid(Vector3 pos)
    {
        float half = tileSize / 2f;
        pos.x = Mathf.Round((pos.x - half) / tileSize) * tileSize + half;
        pos.y = Mathf.Round((pos.y - half) / tileSize) * tileSize + half;
        return pos;
    }
}
