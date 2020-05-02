using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlatformGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject platformGroupPrefab = default;

    [SerializeField]
    private Sprite[] tileSprites = default;

    [SerializeField]
    private Sprite[] treeSprites = default;

    [SerializeField]
    private Sprite[] backgroundSprites = default;

    [SerializeField]
    private SpriteRenderer floor = default;

    [SerializeField]
    private SpriteRenderer background = default;

    [SerializeField]
    private GameObject titleScreenGroup = default;

    [SerializeField]
    private GameObject gameScreenGroup = default;

    [SerializeField]
    private GameObject retryScreenGroup = default;

    [SerializeField]
    private SpriteRenderer[] trees = default;

    [SerializeField]
    private Player player = default;

    [SerializeField]
    private Button startGameButton = default;

    [SerializeField]
    private Button retryGameButton = default;

    [SerializeField]
    private TextMeshProUGUI endScoreText = default;

    [SerializeField]
    private TextMeshProUGUI highScoreText = default;

    private List<Platform> platforms = default;

    private float offsetHeight = 2f;

    private const int maxBoxSize = 3;
    private Vector3 startCameraPositionCache = default;

    private void Start()
    {
        startCameraPositionCache = Camera.main.transform.position;

        titleScreenGroup.gameObject.SetActive(true);
        gameScreenGroup.gameObject.SetActive(false);
        retryScreenGroup.gameObject.SetActive(false);

        player.gameObject.SetActive(false);
        floor.gameObject.SetActive(false);

        startGameButton.onClick.AddListener(StartGame);
        retryGameButton.onClick.AddListener(StartGame);

        player.OnDead += OnPlayerDead;
    }

    private void OnPlayerDead(int score)
    {
        if (score > Player.GetHighScore())
        {
            PlayerPrefs.SetInt(Player.HighScorePlayerPref, score);
        }

        player.transform.SetParent(null);
        player.gameObject.SetActive(false);

        endScoreText.text = score.ToString("N0");
        highScoreText.text = Player.GetHighScore().ToString("N0");

        titleScreenGroup.gameObject.SetActive(false);
        gameScreenGroup.gameObject.SetActive(false);
        retryScreenGroup.gameObject.SetActive(true);
    }

    public void StartGame()
    {
        Camera.main.transform.position = startCameraPositionCache;

        titleScreenGroup.gameObject.SetActive(false);
        gameScreenGroup.gameObject.SetActive(true);
        retryScreenGroup.gameObject.SetActive(false);

        player.gameObject.SetActive(true);
        floor.gameObject.SetActive(true);

        player.transform.position = floor.transform.position + (Vector3.up * 0.5f);
        player.StartGame();

        int highScore = Player.GetHighScore();
        Sprite backgroundSprite = backgroundSprites[UnityEngine.Random.Range(0, backgroundSprites.Count())];
        int spriteRandomIndex = UnityEngine.Random.Range(0, tileSprites.Count());
        Sprite tileSprite = tileSprites[spriteRandomIndex];
        Sprite treeSprite = treeSprites[spriteRandomIndex];

        foreach (var tree in trees)
        {
            tree.sprite = treeSprite;
        }

        InitGame();

        floor.sprite = tileSprite;

        for (int i = 0; i < 100; ++i)
        {
            Platform platform = platforms.ElementAt(i);
            int score = i + 1;
            platform.SetTileSprite(tileSprite);
            platform.SetScore(score);
            platform.SetHighScoreGroupVisible(score == highScore);
            platform.SetMoveSpeed(CalculateMoveSpeed(i));
            platform.SetSize(CalculateSize(i));
            platform.transform.localPosition = Vector2.right * UnityEngine.Random.Range(-2f, 2f);
        }
    }

    public void ShowPlayAgain()
    {

    }

    private void InitGame()
    {
        if (platforms == null)
        {
            platforms = new List<Platform>();
            for (int i = 0; i < 100; ++i)
            {
                GameObject go = GameObject.Instantiate(platformGroupPrefab, Vector3.up * (i + 1) * offsetHeight, Quaternion.identity, transform);
                Platform platform = go.GetComponentInChildren<Platform>();
                platforms.Add(platform);
            }
        }
    }

    private float CalculateMoveSpeed(int index)
    {
        return (1f + UnityEngine.Random.Range(0, (index / 20f)));
    }

    private int CalculateSize(int index)
    {
        if (UnityEngine.Random.Range(0, 100) < index)
        {
            if (UnityEngine.Random.Range(0, 100) < index)
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
