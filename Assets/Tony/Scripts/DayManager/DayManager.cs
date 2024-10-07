using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeeGame
{

    /// <summary>
    /// Call StartDay() to start day at sunrise.
    /// 
    /// Hook into the BecameNight event to know when it's night.
    /// </summary>
    public class DayManager : MonoBehaviour
    {
        [SerializeField] private float dayLengthInSeconds = 60;
        [SerializeField] private int sunriseHour = 5;
        [SerializeField] private int sunsetHour = 18;
        [SerializeField] private float currentTime;
        [SerializeField] private List<CanvasGroup> morningImages;
        [SerializeField] private List<CanvasGroup> eveningImages;

        public event System.Action BecameNight = null;

        private bool isActive;
        private float hourLengthInSeconds;
        private CanvasGroup previousImage;
        private int currentHour, previousHour;

        private void Awake()
        {
            hourLengthInSeconds = dayLengthInSeconds / 24;
        }

        private void Start()
        {
            UIController.Instance.FadeToBlackCompleted += HandleBlackoutCompleted;
        }
        public void StartDay()
        {
            StartMorning();
            currentTime = sunriseHour;
            previousHour = sunriseHour - 1;
            isActive = true;
        }

        public void StopDay()
        {
            isActive = false;
        }

        private void StartMorning()
        {
            morningImages.ForEach(x => x.alpha = 1);
            eveningImages.ForEach(x => x.alpha = 0);
        }

        private void StopMorning()
        {
            morningImages.ForEach(x => x.alpha = 0);
        }

        private void StartNight()
        {
            eveningImages.ForEach(x => x.alpha = 0);
            eveningImages[eveningImages.Count - 1].alpha = 1;
        }

        private void Update()
        {
            if (!isActive) return;
            currentTime += Time.deltaTime * (24 / dayLengthInSeconds);
            if (currentTime >= 24) currentTime = 0;
            currentHour = Mathf.FloorToInt(currentTime);
            if (currentHour > previousHour)
            {
                if (currentHour == sunriseHour)
                {
                    // At start of day, turn on all morning layers.
                    // We'll gradually set their alphas to 0.
                    StartMorning();
                }
                else if (currentHour == sunriseHour + morningImages.Count + 1)
                {
                    StopMorning();
                }
                else if (currentHour == sunsetHour + eveningImages.Count + 1)
                {
                    // After layering all evening images, turn them all
                    // off except the final night image and invoke event.
                    StartNight();
                    BecameNight?.Invoke();
                    UpgradeController.Instance.ReasonToEnteringMode = UpgradeController.ReasonsForEnteringMode.NightTime;
                    PlayerController.Instance.Player.GetComponentInChildren<HarvestHandler>().DumpAllPollen();
                    PlayerController.Instance.Player.transform.position = new Vector2(0.5f, 0.5f);
                    UIController.Instance.FadeToBlack();
                    
                }
            }
            if (sunriseHour <= currentHour && currentHour < (sunriseHour + morningImages.Count))
            {
                morningImages[currentHour - sunriseHour].alpha = (1 - (currentTime - currentHour));
            }
            else if (sunsetHour <= currentHour && currentHour < (sunsetHour + eveningImages.Count))
            {
                // In evening, progressively increase alphas and keep on:
                var alpha = currentTime - Mathf.Floor(currentTime);
                eveningImages[currentHour - sunsetHour].alpha = currentTime - currentHour;
            }
        }

        private void HandleBlackoutCompleted()
        {
            GameController.Instance.SetGameMode(GameController.GameModes.Upgrading);
            UIController.Instance.FadeOutFromBlack();
        }
    }
}
