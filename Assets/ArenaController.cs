using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls quantity/location/type of things spawned in the world, including 
/// flowers, bee hives, and predator starting locations
/// </summary>
public class ArenaController : MonoBehaviour
{
    public ArenaController Instance { get; private set; }

    //settings
    [SerializeField] float _maxArenaRadius = 40f;
    [SerializeField] int _numberOfFlowers = 20;
    [SerializeField] float _minDistanceBetweenFlowers = 2f;

    [SerializeField] int _totalHives = 4;
    [SerializeField] float _minDistanceBetweenHives = 10f;

    [SerializeField] float _minDistanceFromHiveToFlower = 10f;


    //state
    GameObject _arena;
    GameObject _homeHive;
    [SerializeField] List<GameObject> _enemyHives = new List<GameObject>();
    [SerializeField] List<Vector3> _allHivesPoints = new List<Vector3>();


    [SerializeField] List<GameObject> _allFlowers = new List<GameObject>();
    [SerializeField] List<Vector3> _allFlowersPoints = new List<Vector3>();

    private void Awake()
    {
        Instance = this;

    }

    private void Start()
    {
        GameController.Instance.NewGameStarted += HandleNewGameStarted;
    }

    private void HandleNewGameStarted()
    {
        ClearExistingArena();


        _arena = Instantiate(new GameObject());
        _arena.name = "Arena";

        for (int i = 0; i < _numberOfFlowers; i++)
        {
            GenerateRandomArenaObject();
        }

        GenerateHomeHive();
        GenerateEnemyHives();

        DestroyFlowersTooCloseToHives();
    }


    private void GenerateHomeHive()
    {
        _homeHive = Instantiate(ArenaObjectLibrary.Instance.GetHive(0));
        _allHivesPoints.Add(_homeHive.transform.position);
        _homeHive.transform.parent = transform.parent;
    }

    private void GenerateEnemyHives()
    {
        for (int i = 0; i < _totalHives; i++)
        {
            Vector3 pos = CUR.GetRandomPosWithinArenaAwayFromOtherPoints(Vector3.zero,
                _maxArenaRadius,
                _allHivesPoints, 
                _minDistanceBetweenHives);

            if (pos == Vector3.zero) break;

            GameObject newHive = Instantiate(
                ArenaObjectLibrary.Instance.GetHive(i + 1),
                pos, Quaternion.identity);

            newHive.transform.parent = transform.parent;

            _enemyHives.Add(newHive);
            _allHivesPoints.Add(newHive.transform.position);
        }
    }

    private void ClearExistingArena()
    {
        if (_arena) Destroy(_arena);
        _allHivesPoints.Clear();
        _allFlowersPoints.Clear();
        _allFlowers.Clear();
        _homeHive = null;
    }

    private void GenerateRandomArenaObject()
    {
        Vector2 pos = CUR.GetRandomPosWithinArenaAwayFromOtherPoints(
            Vector2.zero,
            _maxArenaRadius,
            _allFlowersPoints,
            _minDistanceBetweenFlowers);

        GameObject go = Instantiate(
            ArenaObjectLibrary.Instance.GetRandomFlower(),
            pos, Quaternion.identity);

        go.transform.parent = _arena.transform;

        _allFlowers.Add(go);
        _allFlowersPoints.Add(go.transform.position);
    }

    private void DestroyFlowersTooCloseToHives()
    {
        //float dist;
        //for (int i = _allFlowers.Count - 1; i >= 0; i--)
        //{
        //    for (int j = 0; j < _allHivesPoints.Count; i++)
        //    {
        //        dist = (_allFlowers[i].transform.position - _allHivesPoints[j]).magnitude;
        //        if (dist <= _minDistanceFromHiveToFlower)
        //        {
        //            Destroy(_allFlowers[i].gameObject);
        //            break;
        //        }
        //    }

        //}
    }

}
