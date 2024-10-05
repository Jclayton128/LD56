using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllegianceHandler : MonoBehaviour
{
    public enum Allegiances { BeePlayer, Bee1, Bee2, Bee3, Wasp, Birds, Spider}

    [SerializeField] Allegiances _allegiance;
    public Allegiances Allegiance => _allegiance;
}
