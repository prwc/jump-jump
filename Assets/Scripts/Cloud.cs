using UnityEngine;

public class Cloud : MonoBehaviour
{
    private Platform platform = default;
    private float moveSpeed = 1f;
    private int targetIndex = 0;

    public void SetPlatfrom(Platform platform)
    {
        this.platform = platform;
        targetIndex = Random.Range(0, platform.CircularWaypoint.Count);
    }

    public void SetMoveSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }

    private void Update()
    {
        if (platform.CircularWaypoint.Count > 0)
        {
            Vector3 diff = platform.CircularWaypoint[targetIndex].position - transform.position;
            if (diff.sqrMagnitude <= 0.1f)
            {
                targetIndex = (targetIndex + 1) % platform.CircularWaypoint.Count;
            }
            else
            {
                Vector3 direction = diff.normalized;
                transform.position += (direction * Time.deltaTime * moveSpeed);
            }
        }
    }
}
