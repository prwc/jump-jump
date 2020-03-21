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
            int boxSize = maxBoxSize;

            if (Random.Range(0, 100) < i)
            {
                if (Random.Range(0, 100) < i)
                {
                    boxSize = 1;
                }
                else
                {
                    boxSize = 2;
                }
            }

            platform.SetMoveSpeed(1f + Random.Range(0, (i / 20f)));

            platform.SetSize(boxSize);
            platform.transform.localPosition = Vector2.right * Random.Range(-2f, 2f);
        }
    }
}
