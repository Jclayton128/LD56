using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Discussion")]
public class Discussion : ScriptableObject
{
    [SerializeField] string _0queenGreeting = "I am the queen.";
    public string QueenSpeech0 => _0queenGreeting;
    [SerializeField] string _1playerResponse = "but I am the worker.";
    public string PlayerResponse1 => _1playerResponse;
    [SerializeField] string _2queenResponse = "Hmm.";
    public string QueenResponse2 => _2queenResponse;
    [SerializeField] string _3playerStinger = "seeya";
    public string PlayerStinger3 => _3playerStinger;

    public int step = 0;

}
