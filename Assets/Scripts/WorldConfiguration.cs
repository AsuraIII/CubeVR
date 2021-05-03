using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldConfiguration : MonoBehaviour
{

    public static WorldConfiguration _instance;

    private void Awake()
    {
        _instance = this;
    }

    public Transform bigRubik;
    public Transform smallRubik;
    public GameObject sciFiWallPrefab;

    // cubes of control rubik
    public Transform[] smallRubikRooms;

    // cubes of control rubik
    public Transform[] bigRubikRooms;

    // pivots of control rubik
    public Transform[] smallRubikPivots;

    // pivots of rubik's with player in
    public Transform[] bigRubikPivots;

}
