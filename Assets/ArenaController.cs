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
    public static ArenaController Instance { get; private set; }
    public Action NewArenaGenerated;

    //settings
    
    [SerializeField] float _maxArenaRadius = 40f;
    [SerializeField] int _numberOfFlowers = 20;
    [SerializeField] float _minDistanceBetweenFlowers = 2f;

    [SerializeField] int _enemyHivesToSpawn = 4;
    [SerializeField] float _minDistanceBetweenHives = 10f;

    [SerializeField] float _minDistanceFromHiveToFlower = 10f;

    //state
    GameObject _arena;
    HiveHandler _homeHive;
    public HiveHandler PlayerHive => _homeHive;
    [SerializeField] List<HiveHandler> _enemyHives = new List<HiveHandler>();
    [SerializeField] List<Vector3> _allHivesPoints = new List<Vector3>();


    [SerializeField] List<FlowerHandler> _allFlowers = new List<FlowerHandler>();
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
            GenerateRandomFlower();
        }

        GenerateHomeHive();
        GenerateEnemyHives();

        DestroyFlowersTooCloseToHives();
        NewArenaGenerated?.Invoke();
    }


    private void GenerateHomeHive()
    {
        _homeHive = Instantiate(ArenaObjectLibrary.Instance.GetHive(0)).GetComponent<HiveHandler>();
        _allHivesPoints.Add(_homeHive.transform.position);
        _homeHive.transform.parent = _arena.transform;
    }

    private void GenerateEnemyHives()
    {
        for (int i = 0; i < _enemyHivesToSpawn; i++)
        {
            Vector3 pos = CUR.GetRandomPosWithinArenaAwayFromOtherPoints(Vector3.zero,
                _maxArenaRadius,
                _allHivesPoints, 
                _minDistanceBetweenHives);

            if (pos == Vector3.zero) break;

            HiveHandler newHive = Instantiate(
                ArenaObjectLibrary.Instance.GetHive(i + 1),
                pos, Quaternion.identity).GetComponent<HiveHandler>();

            newHive.transform.parent = _arena.transform;

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

    private void GenerateRandomFlower()
    {
        Vector2 pos = CUR.GetRandomPosWithinArenaAwayFromOtherPoints(
            Vector2.zero,
            _maxArenaRadius,
            _allFlowersPoints,
            _minDistanceBetweenFlowers);

        FlowerHandler go = Instantiate(
            ArenaObjectLibrary.Instance.GetRandomFlower(),
            pos, Quaternion.identity).GetComponent<FlowerHandler>();

        go.transform.parent = _arena.transform;

        _allFlowers.Add(go);
        _allFlowersPoints.Add(go.transform.position);
    }

    private void DestroyFlowersTooCloseToHives()
    {
        foreach (var hive in _allHivesPoints)
        {
            var hits = Physics2D.OverlapCircleAll(hive, _minDistanceFromHiveToFlower);
            for (int i = hits.Length - 1; i >= 0; i--)
            {
                FlowerHandler fh;
                if (hits[i].TryGetComponent<FlowerHandler>(out fh))
                {
                    _allFlowers.Remove(fh);
                    _allFlowersPoints.Remove(fh.transform.position);
                    Destroy(hits[i].gameObject);
                }

            }
        }

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
