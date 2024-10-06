using UnityEngine;

namespace BeeGame
{

    public class Followers : MonoBehaviour
    {

        [SerializeField] private FollowerBeeController followerBeePrefab;

        private const float SpawnDistance = 2;

        private void Start()
        {
            if (GameController.Instance == null)
            {
                Debug.LogError("No GameController. Not spawning followers.", this);
                return;
            }
            for (int i = 0; i < GameController.Instance.NumFollowers; i++)
            {
                var offset = new Vector3(
                    Random.Range(-SpawnDistance, SpawnDistance), 
                    Random.Range(-SpawnDistance, SpawnDistance), 0);
                Instantiate(followerBeePrefab, transform.position + offset, Quaternion.identity);
            }
        }
    }
}
