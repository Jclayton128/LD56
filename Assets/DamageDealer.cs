using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] int _damageToInflict = 1;
    [SerializeField] bool _attackingKillsSelf = false;

    HealthHandler _hh;
    private void Start()
    {
        _hh = GetComponent<HealthHandler>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HealthHandler hh;
        if (collision.TryGetComponent<HealthHandler>(out hh))
        {
            hh.ReduceHitpoints(_damageToInflict);
            if (_attackingKillsSelf)
            {
                _hh.ReduceHitpoints(999);
            }
        }
    }
}
