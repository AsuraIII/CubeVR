using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    private bool isGround = false;

    private Transform groundBrick;

    private float groundHeightCheck = 0.1f;

    public Transform GroundBrick { get => groundBrick; }
    public bool IsGround { get => isGround; set => isGround = value; }
    float Timer;
    float TimeMax = 1f;

    private void Update()
    {
        Timer += Time.deltaTime;
        if (Timer > TimeMax)
        {
            IsGrounded();
            Timer = 0f; ;
        }
    }

    private bool IsGrounded()
    {
        LayerMask brickLayer = LayerMask.GetMask("Brick");
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, -transform.forward, out hit, groundHeightCheck, ~brickLayer))
        {

            Transform monster = this.transform.parent.parent;
            monster.parent = hit.transform;

            return true;
        }
        return false;
    }
    bool IsPerpOnGround(Transform ground)
    {
        if ((ground.transform.forward == this.transform.forward) || (ground.transform.forward == -this.transform.forward))
        {
            return true;
        }
        return false;
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "Brick")
    //    {
    //        if (IsPerpOnGround(other.transform))
    //        {
    //            //groundBrick = other.transform;
    //            //IsGround = true;

    //            Transform monster = this.transform.parent.parent;
    //            Vector3 worldPos = monster.localPosition;
    //            monster.parent = other.transform;
    //            //monster.localPosition =
    //            monster.transform.position = other.transform.position + Vector3.up;


    //        }
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.tag == "Brick")
    //    {
    //        if (IsPerpOnGround(other.transform))
    //        {
    //            //groundBrick = other.transform;
    //            //IsGround = true;

    //            this.transform.parent.parent.SetParent(null);
    //        }
    //    }
    //}
}
