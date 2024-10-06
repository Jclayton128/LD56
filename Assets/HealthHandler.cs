using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HealthHandler : MonoBehaviour
{
    public Action EntityDied;

    [SerializeField] int _hitpoints = 1;

    public virtual void ReduceHitpoints(int hitpoints)
    {
        _hitpoints -= hitpoints;
        if (_hitpoints <= 0)
        {
            EntityDied?.Invoke();
            Destroy(transform.parent.gameObject);
        }
    }
    
}
