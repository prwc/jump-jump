using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public int Score { get; private set; } = 0;

    [SerializeField]
    private List<Transform> circularWaypoint = default;

    [SerializeField]
    private float moveSpeed = 1f;

    private int targetIndex = 0;

    public void SetScore(int score)
    {
        Score = score;
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
