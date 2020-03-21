using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject platformGroupPrefab = default;

    private float offsetHeight = 2f;

    private const int maxBoxSize = 3;

    private void Start()
    {
        for (int i = 0; i < 100; ++i)
        {
            GameObject go = GameObject.Instantiate(platformGroupPrefab, Vector3.up * (i + 1) * offsetHeight, Quaternion.identity, transform);
            Platform platform = go.GetComponentInChildren<Platform>();
            platform.SetScore(i + 1);
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
