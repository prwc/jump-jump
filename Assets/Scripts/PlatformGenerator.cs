﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject platformGroupPrefab = default;

    private float offsetHeight = 2f;

    private void Start()
    {
        for (int i = 0; i < 100; ++i)
        {
            GameObject go = GameObject.Instantiate(platformGroupPrefab, Vector3.up * (i + 1) * offsetHeight, Quaternion.identity, transform);
        }
    }
}