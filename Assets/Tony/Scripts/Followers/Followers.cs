using System.Collections.Generic;
using UnityEngine;

namespace BeeGame
{

    public class Followers : MonoBehaviour
    {

        [SerializeField] private FollowerBeeController followerBeePrefab;

        private const float SpawnDistance = 2;

        private List<GameObject> instances = new List<GameObject>();

        [SerializeField] private List<FollowerBeeController> _unusedFollowerBees = new List<FollowerBeeController>();  
        public int NumberOfUsableFollowers => _unusedFollowerBees.Count;


        private void Start()
        {
            if (GameController.Instance == null)
            {
                Debug.LogError("No GameController. Not spawning followers.", this);
                return;
            }
            RespawnFollowers();
        }

        [ContextMenu("Debug_CreateFollowers")]
        public void Debug_CreateFollowers()
        {
            PollenRunController.Instance.FollowerToSpawnOnNextPollenRun = 3;
            RespawnFollowers();
        }

        public void RespawnFollowers()
        { 
            instances.ForEach(instance => Destroy(instance));
            instances.Clear();
            _unusedFollowerBees.Clear();
            for (int i = 0; i < PollenRunController.Instance.FollowerToSpawnOnNextPollenRun; i++)
            {
                var offset = new Vector3(
                    Random.Range(-SpawnDistance, SpawnDistance), 
                    Random.Range(-SpawnDistance, SpawnDistance), 0);
                var instance = Instantiate(followerBeePrefab, transform.position + offset, Quaternion.identity);
                instances.Add(instance.gameObject);
                FollowerBeeController fbc = instance.GetComponent<FollowerBeeController>();
                fbc.ConnectBeeToHandler(this);
                _unusedFollowerBees.Add(fbc);
            }
        }

        public void LaunchUnusedFollowerAtTarget(HealthHandler target)
        {
            if (_unusedFollowerBees.Count == 0)
            {
                Debug.LogWarning("No bees left to attack with!", this);
                return;
            }
            FollowerBeeController fbc = _unusedFollowerBees[0];
            _unusedFollowerBees.RemoveAt(0);
            fbc.SetTarget(target);
        }

        public void CheckInCompletedAttackBee(FollowerBeeController bee)
        {
            _unusedFollowerBees.Add(bee);
        }

        public void KillFollowerBeeUponPlayerBeeDamaged()
        {
            _unusedFollowerBees.RemoveAt(0);
            GameObject go = instances[0];
            instances.Remove(go);
            Destroy(go);        
        }

        public void RemoveDeadAttackBee(FollowerBeeController bee)
        {
            instances.Remove(bee.gameObject);
            _unusedFollowerBees.Remove(bee);
        }
    }
}
