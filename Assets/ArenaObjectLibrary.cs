using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaObjectLibrary : MonoBehaviour
{
    public static ArenaObjectLibrary Instance { get; private set; }

    [SerializeField] GameObject[] _flowers = null;

    [Tooltip("Hive 0 is starting player. Others are for enemy factions.")]
    [SerializeField] GameObject[] _hives = null;

    private void Awake()
    {
        Instance = this;
    }

    public GameObject GetRandomFlower()
    {
        int rand = UnityEngine.Random.Range(0, _flowers.Length);
        return _flowers[rand];
    }

    public GameObject GetHive(int hiveType)
    {
        return _hives[hiveType];
    }
}
