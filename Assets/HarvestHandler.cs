using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HarvestHandler : MonoBehaviour
{
    public Action PollenFactorChanged;

    //settings
    [SerializeField] float _pollenCapacity_Starting = 10f;
    [SerializeField] BeeUIDriver _beeUIDriver = null;


    //state
    List<GameObject> _flowersInRange = new List<GameObject>();
    float _pollenCapacity_Current;
    float _pollenLoad;
    public float PollenLoad => _pollenLoad;
    public float PollenFactor => _pollenLoad / _pollenCapacity_Current;

    private void Awake()
    {
        _pollenCapacity_Current = _pollenCapacity_Starting;
    }

    private void Start()
    {
        UIController.Instance.SetContextText(" ");
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        UIController.Instance.SetContextText("Space to Harvest");
        _flowersInRange.Add(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _flowersInRange.Remove(collision.gameObject);
        if (_flowersInRange.Count == 0)
        {
            UIController.Instance.SetContextText(" ");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _flowersInRange.Count > 0)
        {
            HarvestPollen();
        }
    }

    public void HarvestPollen()
    {
        foreach (var flower in _flowersInRange)
        {
            //flower.HarvestPollen()
            _pollenLoad += 1f;
        }
        PollenFactorChanged?.Invoke();
    }


}
