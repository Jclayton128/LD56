using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeeGame;
public class EnemyDetector : MonoBehaviour
{
    [SerializeField] List<AllegianceHandler> _enemies = new List<AllegianceHandler>();

    ContextHandler _beeContext;
    [SerializeField] Followers _followers = null;

    private void Start()
    {
        _beeContext = GetComponentInParent<ContextHandler>();
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        AllegianceHandler ah;
        if (collision.transform.parent.TryGetComponent<AllegianceHandler>(out ah))
        {
            if (ah.Allegiance != AllegianceHandler.Allegiances.BeePlayer)
            {

                if (_followers.NumberOfUsableFollowers > 0)
                {
                    _enemies.Add(ah);
                    _beeContext.AddAvailableContext(ContextHandler.BeeContexts.Attack);
                }

            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        AllegianceHandler ah;
        if (collision.transform.parent.TryGetComponent<AllegianceHandler>(out ah))
        {
            if (ah.Allegiance != AllegianceHandler.Allegiances.BeePlayer)
            {
                _enemies.Remove(ah);
                if (_enemies.Count == 0)
                {
                    _beeContext.RemoveAvailableContext(ContextHandler.BeeContexts.Attack);
                }
            }
        }

    }

    public HealthHandler GetRandomEnemy()
    {
        int rand = UnityEngine.Random.Range(0, _enemies.Count);
        return _enemies[rand].GetComponentInChildren<HealthHandler>();
    }
}
