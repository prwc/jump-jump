using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimation : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> sprites = default;

    private int index = 0;
    private int spriteCount = 0;
    private float spritePerSecond = 10;

    private void Start()
    {
        index = 0;  
        spriteCount = sprites.Count;
    }

    private void Update()
    {

    }
}
