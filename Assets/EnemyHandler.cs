using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyHandler : MonoBehaviour
{
    public Action TargetChanged; 

    //ref
    AllegianceHandler _allegianceHandler;

    //state
    [SerializeField] Transform _targetTransform;
    public Transform TargetTransform => _targetTransform;//(_targetTransform != null) ? _targetTransform : transform;

    private void Awake()
    {
        _allegianceHandler = GetComponent<AllegianceHandler>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (TargetTransform != null) return;
        AllegianceHandler ah;
        if (collision.transform.parent.TryGetComponent<AllegianceHandler>(out ah))
        {
            if (ah.Allegiance != _allegianceHandler.Allegiance)
            {
                _targetTransform = ah.transform;
                TargetChanged?.Invoke();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (TargetTransform == null) return;
        AllegianceHandler ah;
        if (collision.TryGetComponent<AllegianceHandler>(out ah))
        {
            _targetTransform = null;
            TargetChanged?.Invoke();
        }
    }

}
