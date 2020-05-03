using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    public SpriteRenderer Floor => floor;

    public const int MaxLevel = 3;

    [SerializeField]
    private GameObject platformGroupPrefab = default;

    [SerializeField]
    private GameObject cloudPrefab = default;

    [SerializeField]
    private Sprite[] tileSprites = default;

    [SerializeField]
    private Sprite[] treeSprites = default;

    [SerializeField]
    private SpriteRenderer floor = default;

    [SerializeField]
    private SpriteRenderer[] trees = default;

    private List<Platform> platforms = default;
    private List<Cloud> clouds = default;

    private float offsetHeight = 2f;
    private const int maxBoxSize = 3;

    private void Start()
    {
        floor.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        floor.gameObject.SetActive(true);
        int spriteRandomIndex = UnityEngine.Random.Range(0, tileSprites.Count());

        InitGame();

        Sprite tileSprite = tileSprites[spriteRandomIndex];
        Sprite treeSprite = treeSprites[spriteRandomIndex];
        foreach (var tree in trees)
        {
            tree.sprite = treeSprite;
        }
        floor.sprite = tileSprite;

        int highScore = Player.GetHighScore();

        for (int i = 0; i < MaxLevel; ++i)
        {
            Platform platform = platforms.ElementAt(i);
            int score = i + 1;
            platform.SetTileSprite(tileSprite);
            platform.SetScore(score);

            Cloud cloud = clouds.ElementAt(i);
            cloud.SetMoveSpeed(CalculateMoveSpeed(i));
            cloud.enabled = true;

            if (i == MaxLevel - 1)
            {
                platform.SetHighScoreGroupVisible(false);
                platform.SetEndGameGroupVisible(true);
                platform.SetSize(3);
                cloud.gameObject.SetActive(false);
            }
            else
            {
                platform.SetHighScoreGroupVisible(score == highScore);
                platform.SetEndGameGroupVisible(false);
                platform.SetSize((score == highScore) ? 3 : CalculateSize(i));
                cloud.gameObject.SetActive(CalculateShowCloud(i));
            }

            platform.SetMoveSpeed(CalculateMoveSpeed(i));
            platform.transform.localPosition = Vector2.right * UnityEngine.Random.Range(-2f, 2f);
            platform.enabled = true;
        }
    }

    public void StartMoving()
    {
        foreach (var platform in platforms)
        {
            platform.enabled = true;
        }

        foreach (var cloud in clouds)
        {
            cloud.enabled = true;
        }
    }

    public void StopMoving()
    {
        foreach (var platform in platforms)
        {
            platform.enabled = false;
        }

        foreach (var cloud in clouds)
        {
            cloud.enabled = false;
        }
    }

    private void InitGame()
    {
        if (platforms == null)
        {
            platforms = new List<Platform>();
            clouds = new List<Cloud>();
            for (int i = 0; i < MaxLevel; ++i)
            {
                GameObject go = GameObject.Instantiate(platformGroupPrefab, Vector3.up * (i + 1) * offsetHeight, Quaternion.identity, transform);
                Platform platform = go.GetComponentInChildren<Platform>();
                platforms.Add(platform);

                GameObject cloudGo = GameObject.Instantiate(cloudPrefab, Vector3.up * (i + 1) * offsetHeight, Quaternion.identity, transform);
                Cloud cloud = cloudGo.GetComponentInChildren<Cloud>();
                cloud.SetPlatfrom(platform);
                clouds.Add(cloud);
            }
        }
    }

    private float CalculateMoveSpeed(int index)
    {
        return (1f + UnityEngine.Random.Range(0, (index / 20f)));
    }

    private bool CalculateShowCloud(int index)
    {
        return (UnityEngine.Random.Range(0, 10) == 0);
    }

    private int CalculateSize(int index)
    {
        if (UnityEngine.Random.Range(0, MaxLevel) < index)
        {
            if (UnityEngine.Random.Range(0, MaxLevel) < index)
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
