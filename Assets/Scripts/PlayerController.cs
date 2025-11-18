using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public LayerMask pushableLayer;
    public float tileSize = 0.5f;

    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 moveInput;
    private float lastMoveX = 1f; 

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        moveInput = Vector2.zero;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed) moveInput.x = -1f;
            if (Keyboard.current.dKey.isPressed) moveInput.x = 1f;
            if (Keyboard.current.wKey.isPressed) moveInput.y = 1f;
            if (Keyboard.current.sKey.isPressed) moveInput.y = -1f;
        }

        if (moveInput.x != 0)
            lastMoveX = Mathf.Sign(moveInput.x);

        anim.SetFloat("MoveX", moveInput.x != 0 ? moveInput.x : lastMoveX);
        anim.SetFloat("Speed", moveInput.magnitude);

        // PUSH INPUT
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            // Set direction for the blend tree
            anim.SetFloat("PushDirX", lastMoveX);

            // Fire the animation
            anim.SetTrigger("Push");

            // Try to push object mechanically
            TryPushObject();
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveInput.normalized * moveSpeed;
    }

    void TryPushObject()
    {
        Vector2 pushDir;

        if (moveInput != Vector2.zero)
            pushDir = moveInput.normalized;
        else
            pushDir = new Vector2(lastMoveX, 0);

        RaycastHit2D hit = Physics2D.Raycast(rb.position, pushDir, tileSize, pushableLayer);
        if (hit.collider != null)
        {
            Vector3 target = SnapToGrid(hit.collider.transform.position + (Vector3)(pushDir * tileSize));
            hit.collider.transform.position = target;
        }
    }

    Vector3 SnapToGrid(Vector3 pos)
    {
        float h = tileSize / 2f;
        pos.x = Mathf.Round((pos.x - h) / tileSize) * tileSize + h;
        pos.y = Mathf.Round((pos.y - h) / tileSize) * tileSize + h;
        return pos;
    }
}
