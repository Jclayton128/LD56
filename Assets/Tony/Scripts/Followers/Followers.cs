using System.Collections.Generic;
using UnityEngine;

namespace BeeGame
{

    public class Followers : MonoBehaviour
    {

        [SerializeField] private FollowerBeeController followerBeePrefab;

        private const float SpawnDistance = 2;

        private List<GameObject> instances = new List<GameObject>();

        private void Start()
        {
            if (GameController.Instance == null)
            {
                Debug.LogError("No GameController. Not spawning followers.", this);
                return;
            }
            RespawnFollowers();
        }

        public void RespawnFollowers()
        { 
            instances.ForEach(instance => Destroy(instance));
            instances.Clear();
            for (int i = 0; i < GameController.Instance.NumFollowers; i++)
            {
                var offset = new Vector3(
                    Random.Range(-SpawnDistance, SpawnDistance), 
                    Random.Range(-SpawnDistance, SpawnDistance), 0);
                var instance = Instantiate(followerBeePrefab, transform.position + offset, Quaternion.identity);
                instances.Add(instance.gameObject);
            }
        }
    }
}
