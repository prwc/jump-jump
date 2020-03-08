using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField]
    private List<Transform> circularWaypoint = default;

    [SerializeField]
    private float moveSpeed = 1f;

    private int targetIndex = 0;

    private void Start()
    {
        transform.localPosition = Vector2.right * Random.Range(-2f, 2f);
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
