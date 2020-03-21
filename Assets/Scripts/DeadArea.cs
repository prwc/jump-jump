using UnityEngine;

public class DeadArea : MonoBehaviour
{
    private int playerLayer;

    // Start is called before the first frame update
    private void Start()
    {
        playerLayer = LayerMask.NameToLayer("Player");
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == playerLayer)
        {
            Player player = other.gameObject.GetComponent<Player>();
            player.RestartGame();
        }
    }
}
