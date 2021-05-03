using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public static Player _instance;


    public Transform groundChecker;

    private float groundHeightCheck = 0.3f;
    public Transform standBrick;
    public Node curNode;

    private void Awake()
    {
        _instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        IsGrounded();

    }


    private bool IsGrounded()
    {

        RaycastHit hit;


        if (Physics.Raycast(groundChecker.transform.position, -groundChecker.transform.up, out hit, groundHeightCheck, LayerMask.GetMask("Brick")))
        {
            Debug.DrawRay(groundChecker.transform.position, -groundChecker.transform.up, Color.green);
            ChangeStandBrick(hit.transform);
            return true;
        }

        return false;
    }

    private void ChangeStandBrick(Transform brick)
    {
        if (standBrick == null || standBrick != brick)
        {
            standBrick = brick;
            int index = standBrick.GetComponent<Brick>().index;
            curNode = WorldGraphManager._instance.graph.Nodes[index];
        }

    }
}
