using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer = default;

    [SerializeField]
    private Rigidbody2D playerRigidbody = default;

    [SerializeField]
    private Collider2D playerCollider = default;

    [SerializeField]
    private float jumpSpeed = 300f;

    private bool isGrounded = false;

    private int platformLayer;
    private int playerLayer;

    // Start is called before the first frame update
    void Start()
    {
        playerLayer = LayerMask.NameToLayer("Player");
        platformLayer = LayerMask.NameToLayer("Platform");
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrounded && isTriggerJump())
        {
            transform.SetParent(null);
            Physics2D.IgnoreLayerCollision(playerLayer, platformLayer, true);
            playerRigidbody.AddForce(Vector2.up * jumpSpeed);
        }

        if (!isGrounded && playerRigidbody.velocity.y <= 0f)
        {
            Physics2D.IgnoreLayerCollision(playerLayer, platformLayer, false);
        }

        UpdateState();
    }

    private bool isTriggerJump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            return true;
        }

        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended)
            {
                return true;
            }
        }

        return false;
    }

    private void UpdateState()
    {
        if (!isGrounded)
        {
            spriteRenderer.color = Color.red;
        }
        else
        {
            spriteRenderer.color = Color.white;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == platformLayer)
        {
            transform.SetParent(other.transform);
        }
        isGrounded = true;

        if (Camera.main.WorldToScreenPoint(transform.position).y > Screen.height * 0.75f)
        {
            Vector3 cameraPosition = Camera.main.transform.position;
            cameraPosition.y = transform.position.y + 3f;
            Camera.main.transform.position = cameraPosition;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        isGrounded = false;
    }
}
