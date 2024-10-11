using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HarvestHandler : MonoBehaviour
{
    public Action<int> PollenLoadChanged;

    //refs
    ContextHandler _contextHandler;
    [SerializeField] ParticleSystem _ps = null;

    //settings
	[SerializeField] private AudioClip pollenCollectClip;


    //state
    bool _hasHitSpaceThisRun = false;
    [SerializeField] List<FlowerHandler> _harvestableFlowersInRange = new List<FlowerHandler>();

    [SerializeField] int _totalQuarters = 0;
    public int TotalQuarters => _totalQuarters;

    [SerializeField] HiveHandler _hiveHandler;

    private void Awake()
    {
        _contextHandler = GetComponentInParent<ContextHandler>();
        _ps.Stop();
    }

    private void Start()
    {
        GameController.Instance.GameModeChanged += HandleGameModeChanged;
    }

    private void HandleGameModeChanged(GameController.GameModes newGameMode)
    {
        if (newGameMode == GameController.GameModes.Flying)
        {
            _hasHitSpaceThisRun = false;
            enabled = true;
            ArenaController.Instance.UnfreezeAllEnemies();
        }
        else enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        FlowerHandler fh;
        if (collision.TryGetComponent<FlowerHandler>(out fh))
        {
            fh.PollenAvailabilityChanged += HandlePollenAvailabilityChanged;
            if (fh.Pollen > 0)
            {
                if (!_harvestableFlowersInRange.Contains(fh))
                {
                    _harvestableFlowersInRange.Add(fh);

                }
                _contextHandler.AddAvailableContext(ContextHandler.BeeContexts.Harvest);
            }
        }

        HiveHandler hh;
        if (collision.TryGetComponent<HiveHandler>(out hh))
        {
            //TODO check allegiance to make sure that the player is near a friendly hive.
            _hiveHandler = hh;

            if (_totalQuarters / 4 > 0)
            {
                _contextHandler.AddAvailableContext(ContextHandler.BeeContexts.DepositPollenAtHive);

            }           
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        FlowerHandler fh;
        if (collision.TryGetComponent<FlowerHandler>(out fh))
        {
            _harvestableFlowersInRange.Remove(fh);
            fh.PollenAvailabilityChanged -= HandlePollenAvailabilityChanged;
            if (_harvestableFlowersInRange.Count == 0)
            {
                _contextHandler.RemoveAvailableContext(ContextHandler.BeeContexts.Harvest);

            }
        }

        HiveHandler hh;
        if (collision.TryGetComponent<HiveHandler>(out hh))
        {
            //TODO check allegiance to make sure that the player is near a friendly hive.
            _hiveHandler = null;
            _contextHandler.RemoveAvailableContext(ContextHandler.BeeContexts.DepositPollenAtHive);
        }

    }

    private void HandlePollenAvailabilityChanged(FlowerHandler flower, bool hasPollenAvailable)
    {
        if (hasPollenAvailable)
        {
            if (!_harvestableFlowersInRange.Contains(flower))
            {
                _harvestableFlowersInRange.Add(flower);
                _contextHandler.AddAvailableContext(ContextHandler.BeeContexts.Harvest);
            }
        }
        else
        {
            _harvestableFlowersInRange.Remove(flower);
            if (_harvestableFlowersInRange.Count == 0)
            {
                _contextHandler.RemoveAvailableContext(ContextHandler.BeeContexts.Harvest);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!_hasHitSpaceThisRun && _contextHandler.BeeContext == ContextHandler.BeeContexts.DepositPollenAtHive && _hiveHandler && (_totalQuarters/4 > 0))
            {
                _hasHitSpaceThisRun = true;
                GameObject.FindAnyObjectByType<BeeGame.DayManager>().enabled = false;
                DepositPollen();
            }

            if (_contextHandler.BeeContext == ContextHandler.BeeContexts.Harvest &&
                _harvestableFlowersInRange.Count > 0)
            {
                HarvestPollen();
            }


        }

    }

    private void HarvestPollen()
    {
        int gainedQuarters = 0;
        for (int i = _harvestableFlowersInRange.Count -1; i >= 0; i--)
        {
            gainedQuarters += _harvestableFlowersInRange[i].HarvestPollen();
        }
        _totalQuarters += gainedQuarters;
        _totalQuarters = Mathf.Clamp(_totalQuarters, 0,
            4 * UpgradeController.Instance.PollenCap_Current);

        PollenLoadChanged?.Invoke(_totalQuarters);
		
		//play sound fx
		SoundFXManager.instance.PlaySoundFXClip(pollenCollectClip, transform, 1f);

    }

    private void DepositPollen()
    {
        UpgradeController.Instance.BankPollenHexesToSpend(_totalQuarters / 4);


        Vector2 dir = (_hiveHandler.transform.position - transform.position).normalized;
        _ps.transform.up = dir;
        _ps.Play();
        ArenaController.Instance.FreezeAllEnemies();
        //AUDIO This is called a single time when the player initiates a Deposit at Hive action. Could put a single deposit sound here, or multiple in the "DecrementPollenOnDeposit" method (see below).

        DecrementPollenOnDeposit();
        
    }

    private void DecrementPollenOnDeposit()
    {
        _totalQuarters--;
        //AUDIO This is called during the Deposit at Hive moment. It will get called 
        //multiple times, once for every quarter hex that is deposited.
        if (_totalQuarters == 0)
        {
            PollenLoadChanged?.Invoke(_totalQuarters);
            HandleDecrementEffectComplete();

        }
        else
        {
            PollenLoadChanged?.Invoke(_totalQuarters);
            Invoke(nameof(DecrementPollenOnDeposit), 0.75f);
        }
    }

    private void HandleDecrementEffectComplete()
    {
        _ps.Stop();
        _contextHandler.RemoveAvailableContext(ContextHandler.BeeContexts.DepositPollenAtHive);
        _totalQuarters = 0;
        UpgradeController.Instance.ReasonToEnteringMode = UpgradeController.ReasonsForEnteringMode.NormalDeposit;
        GameController.Instance.SetGameMode(GameController.GameModes.Upgrading);
        GameObject.FindAnyObjectByType<BeeGame.DayManager>().enabled = true;
    }
    
    public void DumpAllPollen()
    {
        _totalQuarters = 0;
    }

    private void OnDestroy()
    {
        GameController.Instance.GameModeChanged -= HandleGameModeChanged;
    }

}
