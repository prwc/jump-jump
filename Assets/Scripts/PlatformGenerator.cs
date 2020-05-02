using System.Linq;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject platformGroupPrefab = default;

    [SerializeField]
    private Sprite[] tileSprites = default;

    [SerializeField]
    private Sprite[] backgroundSprites = default;

    [SerializeField]
    private SpriteRenderer floor = default;

    [SerializeField]
    private SpriteRenderer background = default;

    private float offsetHeight = 2f;

    private const int maxBoxSize = 3;

    private void Start()
    {
        int highScore = Player.GetHighScore();
        Sprite backgroundSprite = backgroundSprites[Random.Range(0, backgroundSprites.Count())];
        Sprite tileSprite = tileSprites[Random.Range(0, tileSprites.Count())];

        background.sprite = backgroundSprite;
        floor.sprite = tileSprite;

        for (int i = 0; i < 100; ++i)
        {
            GameObject go = GameObject.Instantiate(platformGroupPrefab, Vector3.up * (i + 1) * offsetHeight, Quaternion.identity, transform);
            Platform platform = go.GetComponentInChildren<Platform>();
            int score = i + 1;
            platform.SetTileSprite(tileSprite);
            platform.SetScore(score);
            platform.SetHighScoreGroupVisible(score == highScore);
            platform.SetMoveSpeed(CalculateMoveSpeed(i));
            platform.SetSize(CalculateSize(i));
            platform.transform.localPosition = Vector2.right * Random.Range(-2f, 2f);
        }
    }

    private float CalculateMoveSpeed(int index)
    {
        return (1f + Random.Range(0, (index / 20f)));
    }

    private int CalculateSize(int index)
    {
        if (Random.Range(0, 100) < index)
        {
            if (Random.Range(0, 100) < index)
            {
                return 1;
            }
            else
            {
                return 2;
            }
        }
        return 3;
    }
}
