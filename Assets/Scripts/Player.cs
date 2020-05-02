using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public const string HighScorePlayerPref = "highscore_alpha";

    [SerializeField]
    private SpriteRenderer spriteRenderer = default;

    [SerializeField]
    private Rigidbody2D playerRigidbody = default;

    [SerializeField]
    private Collider2D playerCollider = default;

    [SerializeField]
    private TextMeshProUGUI scoreText = default;

    [SerializeField]
    private AudioSource jumpSound = default;

    [SerializeField]
    private AudioSource stageTransitionSound = default;

    [SerializeField]
    private Timer timer = default;

    [SerializeField]
    private float jumpSpeed = 300f;

    private bool isGrounded = false;

    private int platformLayer;
    private int playerLayer;
    private int score = 0;

    private Coroutine stageTransitionRoutine = default;

    public static int GetHighScore()
    {
        return PlayerPrefs.GetInt(HighScorePlayerPref, 0);
    }

    // Start is called before the first frame update
    void Start()
    {
        playerLayer = LayerMask.NameToLayer("Player");
        platformLayer = LayerMask.NameToLayer("Platform");

        UpdateScore(0);
    }

    public void RestartGame()
    {
        if (score > GetHighScore())
        {
            PlayerPrefs.SetInt(HighScorePlayerPref, score);
        }
        SceneManager.LoadScene(0);
    }

    private void UpdateScore(int score)
    {
        this.score = score;
        scoreText.text = $"{score.ToString("N0")}";
    }

    private void Update()
    {
        if (isGrounded && isTriggerJump())
        {
            transform.SetParent(null);
            Physics2D.IgnoreLayerCollision(playerLayer, platformLayer, true);
            playerRigidbody.AddForce(Vector2.up * jumpSpeed);
            jumpSound.Play();
        }

        if (!isGrounded && playerRigidbody.velocity.y <= 0f)
        {
            Physics2D.IgnoreLayerCollision(playerLayer, platformLayer, false);
        }

        if (stageTransitionRoutine == default)
        {
            timer.UpdateTime(Time.deltaTime);
        }

        UpdateState();
    }

    private bool isTriggerJump()
    {
        if (stageTransitionRoutine != default)
        {
            return false;
        }

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

        if (Camera.main.WorldToScreenPoint(transform.position).y > Screen.height * 0.8f)
        {
            Vector3 cameraPosition = Camera.main.transform.position;
            cameraPosition.y = transform.position.y + 4f;
            StageTransition(cameraPosition, 2f);
        }

        if (other.gameObject.GetComponent<Platform>() is Platform platform)
        {
            if (platform.Score > score)
            {
                UpdateScore(platform.Score);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        isGrounded = false;
    }

    private void StageTransition(Vector3 cameraTargetPosition, float time)
    {
        stageTransitionRoutine = StartCoroutine(StageTransitionRoutine(cameraTargetPosition, time));
    }

    private IEnumerator StageTransitionRoutine(Vector3 cameraTargetPosition, float time)
    {
        float startTime = Time.time;
        Vector3 startPosition = Camera.main.transform.position;
        float ratio = 0f;

        var platforms = GameObject.FindObjectsOfType<Platform>();
        foreach (var platform in platforms)
        {
            platform.enabled = false;
        }

        stageTransitionSound.Play();

        do
        {
            ratio = Mathf.Clamp01((Time.time - startTime) / time);
            Camera.main.transform.position = Vector3.Lerp(startPosition, cameraTargetPosition, ratio);
            yield return null;
        } while (ratio < 1f);

        foreach (var platform in platforms)
        {
            platform.enabled = true;
        }

        stageTransitionRoutine = default;

        yield return null;
    }
}
