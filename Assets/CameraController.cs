using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }


    //state
    CinemachineVirtualCamera _cvc; 

    private void Awake()
    {
        Instance = this;        
    }

    private void Start()
    {
        _cvc = Camera.main.GetComponentInChildren<CinemachineVirtualCamera>();
        PlayerController.Instance.NewPlayerSpawned += HandleNewPlayerSpawned;
    }

    private void HandleNewPlayerSpawned()
    {
        _cvc.Follow = PlayerController.Instance.Player.transform;
    }
}
