using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkOnWalls : MonoBehaviour
{

    public RotateWorld world;
    public Transform pivot;


    // Use this for initialization
    void Start()
    {
        InvokeRepeating("Raycast", 0, 0.5f);
    }

    void Raycast()
    {
        RaycastHit hit;
        LayerMask bigRubikLayer = LayerMask.GetMask("Rubik");
        LayerMask characterLayer = LayerMask.GetMask("Character");
        if (Physics.Raycast(pivot.position, pivot.forward, 1, ~( bigRubikLayer |  characterLayer)))
        {
            //Debug.Log (hit.collider.gameObject.name);

            world.Rotate(pivot);
        }
        return;
    }
}
