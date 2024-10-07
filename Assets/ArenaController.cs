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
    [SerializeField] int _enemiesToSpawn = 8;
    [SerializeField] float _minDistanceBetweenHives = 10f;

    [SerializeField] float _minDistanceFromHiveToObject = 10f;
    [SerializeField] float _minDistanceBetweenEnemies = 7f;

    //state
    GameObject _arena;
    HiveHandler _homeHive;
    public HiveHandler PlayerHive => _homeHive;
    [SerializeField] List<HiveHandler> _enemyHives = new List<HiveHandler>();
    [SerializeField] List<Vector3> _allHivesPoints = new List<Vector3>();


    [SerializeField] List<FlowerHandler> _allFlowers = new List<FlowerHandler>();
    [SerializeField] List<Vector3> _allFlowersPoints = new List<Vector3>();

    [SerializeField] List<EnemyHandler> _allEnemies = new List<EnemyHandler>();
    [SerializeField] List<Vector3> _allEnemiesPoints = new List<Vector3>();

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
        
        for (int i = 0; i < _enemiesToSpawn; i++)
        {
            GenerateRandomEnemy();
        }

        DestroyFlowersTooCloseToHives();
        DestroyEnemiesTooCloseToHives();
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

    public void GenerateRandomEnemy()
    {
        Vector2 pos = CUR.GetRandomPosWithinArenaAwayFromOtherPoints(
            Vector2.zero,
            _maxArenaRadius,
            _allEnemiesPoints,
            _minDistanceBetweenEnemies);

        EnemyHandler go = Instantiate(
            ArenaObjectLibrary.Instance.GetRandomEnemy(),
            pos, Quaternion.identity).GetComponent<EnemyHandler>();

        //go.transform.parent = _arena.transform; //Unneccesary and confusing to have them under Arena GO

        _allEnemies.Add(go);
        _allEnemiesPoints.Add(go.transform.position);
    }

    private void DestroyFlowersTooCloseToHives()
    {
        foreach (var hive in _allHivesPoints)
        {
            var hits = Physics2D.OverlapCircleAll(hive, _minDistanceFromHiveToObject);
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
    }

    private void DestroyEnemiesTooCloseToHives()
    {
        foreach (var hive in _allHivesPoints)
        {
            var hits = Physics2D.OverlapCircleAll(hive, _minDistanceFromHiveToObject);
            for (int i = hits.Length - 1; i >= 0; i--)
            {
                EnemyHandler fh;
                if (hits[i].TryGetComponent<EnemyHandler>(out fh))
                {
                    _allEnemies.Remove(fh);
                    _allEnemiesPoints.Remove(fh.transform.position);
                    Destroy(hits[i].gameObject);
                }

            }
        }
    }

    public void FreezeAllEnemies()
    {
        if (GameController.Instance.GameMode != GameController.GameModes.Flying) return;

        PlayerController.Instance.Player.GetComponent<MovementHandler>().enabled = false;

        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            enemy.GetComponent<MovementHandler>().enabled = false;
        }
    }

    public void UnfreezeAllEnemies()
    {
        if (GameController.Instance.GameMode != GameController.GameModes.Flying) return;

        PlayerController.Instance.Player.GetComponent<MovementHandler>().enabled = true;
        
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            enemy.GetComponent<MovementHandler>().enabled = true;
        }
    }

}
