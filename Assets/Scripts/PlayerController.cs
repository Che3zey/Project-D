using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public LayerMask pushableLayer;
    public float tileSize = 0.5f; // tilemap grid size (0.5 units)
    
    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 moveInput;
    private float lastMoveX = 1f; // horizontal facing

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        moveInput = Vector2.zero;

        // Keyboard movement input
        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed) moveInput.x = -1f;
            if (Keyboard.current.dKey.isPressed) moveInput.x = 1f;
            if (Keyboard.current.wKey.isPressed) moveInput.y = 1f;
            if (Keyboard.current.sKey.isPressed) moveInput.y = -1f;
        }

        // Update last horizontal facing
        if (moveInput.x != 0) lastMoveX = moveInput.x;

        // Update Blend Tree parameters
        anim.SetFloat("MoveX", moveInput.x != 0 ? moveInput.x : lastMoveX);
        anim.SetFloat("Speed", moveInput.magnitude);

        // Left mouse click triggers push animation and tries to push object
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            anim.SetTrigger("Push"); // Trigger in PushLayer
            TryPushObject();
        }
    }

    void FixedUpdate()
    {
        // Move player
        rb.linearVelocity = moveInput.normalized * moveSpeed;
    }

    void TryPushObject()
    {
        Vector2 pushDir;

        // Determine push direction: current input or last horizontal facing if standing still
        if (moveInput != Vector2.zero)
            pushDir = moveInput.normalized;
        else
            pushDir = new Vector2(lastMoveX, 0);

        // Raycast to see if there is a pushable object
        RaycastHit2D hit = Physics2D.Raycast(rb.position, pushDir, tileSize, pushableLayer);
        if (hit.collider != null)
        {
            // Move object one tile and snap to grid
            Vector3 targetPos = SnapToGrid(hit.collider.transform.position + (Vector3)(pushDir * tileSize));
            hit.collider.transform.position = targetPos;
        }
    }

    Vector3 SnapToGrid(Vector3 pos)
    {
        float halfTile = tileSize / 2f;

        pos.x = Mathf.Round((pos.x - halfTile) / tileSize) * tileSize + halfTile;
        pos.y = Mathf.Round((pos.y - halfTile) / tileSize) * tileSize + halfTile;

        return pos;
    }
}
