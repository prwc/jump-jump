using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public IReadOnlyList<Transform> CircularWaypoint => circularWaypoint;

    public int Score { get; private set; } = 0;

    [SerializeField]
    private List<Transform> circularWaypoint = default;

    [SerializeField]
    private SpriteRenderer spriteRenderer = default;

    [SerializeField]
    private BoxCollider2D boxCollider = default;

    [SerializeField]
    private GameObject highScoreGroup = default;

    private float moveSpeed = 1f;

    private int targetIndex = 0;

    public void SetMoveSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }

    public void SetScore(int score)
    {
        Score = score;
    }

    public void SetHighScoreGroupVisible(bool isVisible)
    {
        highScoreGroup.gameObject.SetActive(isVisible);
    }

    public void SetSize(int size)
    {
        Vector2 newSize = new Vector2(size, spriteRenderer.size.y);
        spriteRenderer.size = newSize;
        boxCollider.size = newSize;
    }

    public void SetTileSprite(Sprite tileSprite)
    {
        spriteRenderer.sprite = tileSprite;
    }

    private void Start()
    {
        targetIndex = Random.Range(0, circularWaypoint.Count);
    }

    private void Update()
    {
        if (circularWaypoint.Count > 0)
        {
            Vector3 diff = circularWaypoint[targetIndex].position - transform.position;
            if (diff.sqrMagnitude <= 0.1f)
            {
                targetIndex = (targetIndex + 1) % circularWaypoint.Count;
            }
            else
            {
                Vector3 direction = diff.normalized;
                transform.position += (direction * Time.deltaTime * moveSpeed);
            }
        }
    }
}
