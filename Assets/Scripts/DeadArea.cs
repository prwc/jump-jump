using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            SceneManager.LoadScene(0);
        }
    }
}
