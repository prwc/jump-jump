using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI timerText = default;

    private float timer = 0f;

    private void Start()
    {
        timer = 0f;
    }

    // Update is called once per frame
    public void UpdateTime(float deltatime)
    {
        timer += deltatime;
        System.TimeSpan t = System.TimeSpan.FromSeconds(timer);
        timerText.text = string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
    }
}
