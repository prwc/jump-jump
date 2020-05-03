using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class MainGameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject titleScreenGroup = default;

    [SerializeField]
    private GameObject gameScreenGroup = default;

    [SerializeField]
    private GameObject retryScreenGroup = default;

    [SerializeField]
    private Player player = default;

    [SerializeField]
    private Button startGameButton = default;

    [SerializeField]
    private Button retryGameButton = default;

    [SerializeField]
    private Button rateUsStoreButton = default;

    [SerializeField]
    private TextMeshProUGUI endScoreText = default;

    [SerializeField]
    private TextMeshProUGUI highScoreText = default;

    [SerializeField]
    private PlatformGenerator platformGenerator = default;

    private Vector3 startCameraPositionCache = default;
    private Coroutine adsBannerCoroutine = default;

    public static string GetAdvertisementID() => "3584481";

    private void Start()
    {
        Advertisement.Initialize(GetAdvertisementID());
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);

        startCameraPositionCache = Camera.main.transform.position;

        titleScreenGroup.gameObject.SetActive(true);
        gameScreenGroup.gameObject.SetActive(false);
        retryScreenGroup.gameObject.SetActive(false);

        player.gameObject.SetActive(false);

        startGameButton.onClick.AddListener(StartGame);
        retryGameButton.onClick.AddListener(StartGame);
        rateUsStoreButton.onClick.AddListener(OpenStorePage);

        player.OnDead += OnPlayerDead;
    }

    public void StartGame()
    {
        HideAdsBanner();

        Camera.main.transform.position = startCameraPositionCache;

        titleScreenGroup.gameObject.SetActive(false);
        gameScreenGroup.gameObject.SetActive(true);
        retryScreenGroup.gameObject.SetActive(false);

        player.gameObject.SetActive(true);

        platformGenerator.StartGame();

        player.transform.position = platformGenerator.Floor.transform.position + (Vector3.up * 0.5f);
        player.transform.SetParent(platformGenerator.Floor.transform);
        player.StartGame();
    }

    public void ShowPlayAgain(int score, int highScore)
    {
        endScoreText.text = score.ToString("N0");
        highScoreText.text = Player.GetHighScore().ToString("N0");

        titleScreenGroup.gameObject.SetActive(false);
        gameScreenGroup.gameObject.SetActive(false);
        retryScreenGroup.gameObject.SetActive(true);

        bool isShowRateUsButton = (PlayerPrefs.GetInt(Player.RateUsStorePlayerPref, 0) == 0);
        rateUsStoreButton.gameObject.SetActive(isShowRateUsButton);

        ShowAdsBanner();
    }

    private void ShowAdsBanner()
    {
        HideAdsBanner();
        adsBannerCoroutine = StartCoroutine(ShowBannerWhenReady("retry_banner"));
    }

    private void HideAdsBanner()
    {
        if (adsBannerCoroutine != default)
        {
            StopCoroutine(adsBannerCoroutine);
            adsBannerCoroutine = default;
        }
        Advertisement.Banner.Hide();
    }

    private void OpenStorePage()
    {
        Application.OpenURL("market://details?id=" + Application.identifier);
        PlayerPrefs.SetInt(Player.RateUsStorePlayerPref, 1);
        rateUsStoreButton.gameObject.SetActive(false);
    }

    private void OnPlayerDead(int score)
    {
        if (score > Player.GetHighScore())
        {
            PlayerPrefs.SetInt(Player.HighScorePlayerPref, score);
        }

        player.transform.SetParent(null);
        player.gameObject.SetActive(false);

        ShowPlayAgain(score, Player.GetHighScore());
    }

    private IEnumerator ShowBannerWhenReady(string placementID)
    {
        while (!Advertisement.IsReady(placementID))
        {
            yield return new WaitForSeconds(0.1f);
        }

        if (retryScreenGroup.gameObject.activeSelf)
        {
            Advertisement.Banner.Show(placementID);
        }
    }
}
