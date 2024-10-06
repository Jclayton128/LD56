using UnityEngine;
using DG.Tweening;

namespace BeeGame
{

    public class FollowerBeeController : MonoBehaviour
    {

        [SerializeField] private Transform spriteTransform;

        private const float MaxDistance = 6;
        private const float MinDistance = 1;
        private const float Speed = 3;

        private Transform myTransform;
        private Transform playerTransform;
        private Vector3 destination;
        private bool isCatchingUpToPlayer;

        private void Awake()
        {
            myTransform = transform;
        }

        private void Start()
        {
            var player = FindFirstObjectByType<MovementHandler>();
            if (player == null)
            {
                Debug.LogWarning("Follower bee can't find player. Deactivating.", this);
                enabled = false;
                return;
            }
            playerTransform = player.transform;
            isCatchingUpToPlayer = false;
            ChooseDestination();
            spriteTransform.DOShakePosition(1, 0.5f, 1, 45, false, false).SetLoops(-1);
        }

        private void Update()
        {
            var distanceFromDestination = Vector3.Distance(myTransform.position, destination);
            var distanceFromPlayer = Vector3.Distance(myTransform.position, playerTransform.position);
            if (distanceFromPlayer > MaxDistance && !isCatchingUpToPlayer)
            {
                // We just got too far, so catch up:
                isCatchingUpToPlayer = true;
                ChooseDestination();
            }
            else if (distanceFromPlayer < MaxDistance && isCatchingUpToPlayer)
            {
                // We're catching up and got back into close range, so stop catching up and just follow:
                isCatchingUpToPlayer = false;
                ChooseDestination();
            }
            else if (distanceFromDestination < 0.1f)
            {
                // We're at our destination, so choose a new destination:
                ChooseDestination();
            }
            else
            {
                // Move to destination:
                var direction = (destination - myTransform.position).normalized;
                myTransform.position += direction * Speed * Time.deltaTime;
            }
        }

        private void ChooseDestination()
        {
            destination = playerTransform.position +
                new Vector3(GetRandomDistance(), GetRandomDistance(), 0);
        }

        private float GetRandomDistance()
        {
            var d = Random.Range(-MaxDistance, MaxDistance) / 2;
            if (Mathf.Abs(d) < MinDistance) d = Mathf.Sign(d) * MinDistance;
            return d;
        }

        private void OnDrawGizmosSelected()
        {
            if (playerTransform == null) return;
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(myTransform.position, destination);
            Gizmos.DrawSphere(destination, 0.1f);
            Gizmos.DrawWireSphere(myTransform.position, MaxDistance);
        }

    }
}
