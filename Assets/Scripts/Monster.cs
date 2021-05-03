using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Monster : MonoBehaviour
{

    public Transform eyeRayStarPos;

    public Transform groundChecker;
    public Transform standBrick;
    private float moveSpeed = 2.0f;
    private float groundHeightCheck = 0.3f;
    private float down45LengthCheck = 0.5f;
    private float forwardLengthCheck = 1.2f;

    private float brickHeightOffset = 0.25f;
    private LayerMask brickLayer;

    private bool isMoving = false;
    private bool isStartMoving = false;
    private bool isTurning = false;

    public Node currentStandOnNode;
    private List<Node> myPath;

    private float upOffset = 0.5f;


    private Animator m_animator;

    void Start()
    {
        myPath = new List<Node>();
        brickLayer = LayerMask.GetMask("Brick");
        m_animator = GetComponent<Animator>();
        isMoving = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
           
            myPath = WorldGraphManager._instance.GetPath(currentStandOnNode, Player._instance.curNode);
            
        }
        if (currentStandOnNode != null)
            Debug.Log(currentStandOnNode.index);
        //IsGrounded();
        RayCastCheckIsOnCorner();
        //CloseToWall();
        IsGrounded();

        Move(Player._instance.curNode);
        //if (IsGrounded() && !isTurning)
        //{
        //    //MoveTest();
        //    //Move(Player._instance.curNode);
        //}


    }

    private void MoveTest()
    {
        if (Input.GetKey(KeyCode.K))
        {
            isMoving = true;
            transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
        }
        if (Input.GetKeyUp(KeyCode.M))
        {
            isMoving = false;
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            StartCoroutine(QRotate(0, -90, 0, 1f));

        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            StartCoroutine(QRotate(0, 90, 0, 1f));

        }
    }
    private bool RayCastCheckIsOnCorner()
    {
        if (IsGrounded() && isMoving)
        {
            RaycastHit hit;
            Vector3 down45 = (eyeRayStarPos.forward - eyeRayStarPos.up).normalized;
            Debug.DrawRay(eyeRayStarPos.position, down45 * down45LengthCheck, Color.green);
            if (Physics.Raycast(eyeRayStarPos.position, down45, out hit, down45LengthCheck, brickLayer))
            {

                if (IsParallel(transform.forward, hit.transform.forward))
                {
                    ClimbBrickWithAnimation(hit.transform, hit.point);
                    return true;
                }
                //if (Vector3.Dot(transform.forward, hit.transform.forward) != 0)
                //    Debug.Log(hit.transform.position
            }
        }
        return false;
    }

    private bool CloseToWall()
    {
        if (IsGrounded())
        {
            RaycastHit hit;
            //Vector3 down45 = (eyeRayStarPos.forward - eyeRayStarPos.up).normalized;
            Debug.DrawRay(eyeRayStarPos.position, eyeRayStarPos.forward, Color.green);
            if (Physics.Raycast(eyeRayStarPos.position, eyeRayStarPos.forward, out hit, forwardLengthCheck, brickLayer))
            {

                if (IsParallel(transform.forward, hit.transform.forward))
                {
                    //if (Physics.Raycast(rayStarPos.position, rayStarPos.forward, out hit2, ))
                    StartCoroutine(ApproachToWall(0.5f));

                    return true;
                }
                //if (Vector3.Dot(transform.forward, hit.transform.forward) != 0)
                //    Debug.Log(hit.transform.position
            }
        }
        return false;
    }

    private IEnumerator ApproachToWall(float time)
    {
        isTurning = true;
        isMoving = false;
        Vector3 startPos = transform.position;
        Vector3 finalPos = transform.position + (transform.forward);

        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            transform.position = Vector3.Lerp(startPos, finalPos, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        isMoving = true;
        isTurning = false;
    }


    private bool IsParallel(Vector3 vec1, Vector3 vec2)
    {
        float dotNumf = Vector3.Dot(vec1, vec2);
        if (Mathf.Approximately(dotNumf, 1.0f))
        {
            return true;
        }
        else if (Mathf.Approximately(dotNumf, -1.0f))
        {
            return true;
        }
        else
            return false;
    }

    private bool IsPerpendicular(Vector3 vec1, Vector3 vec2)
    {
        float dotNum = Vector3.Dot(vec1, vec2);
        if (dotNum == 0)
            return true;
        return false;
    }

    private bool IsGrounded()
    {

        RaycastHit hit;
        if (!isTurning)
        {
            if (Physics.Raycast(groundChecker.transform.position, -groundChecker.transform.up, out hit, groundHeightCheck, brickLayer))
            {
                Debug.DrawRay(groundChecker.transform.position, -groundChecker.transform.up, Color.green);
                ChangeStandBrick(hit.transform);
                return true;
            }
            else
            {
                Debug.Log("No Ground Detected,Check height");
            }
        }
        return false;
    }

    private void ChangeStandBrick(Transform brick)
    {
        if (standBrick == null)
        {
            standBrick = brick;
            int index = standBrick.GetComponent<Brick>().index;
            currentStandOnNode = WorldGraphManager._instance.graph.Nodes[index];
            this.transform.SetParent(brick);
        }
        else if (standBrick != brick)
        {
            this.transform.SetParent(brick);
            standBrick = brick;
            int index = standBrick.GetComponent<Brick>().index;
            currentStandOnNode = WorldGraphManager._instance.graph.Nodes[index];
            Debug.Log(currentStandOnNode.index);
        }

    }

    private void ChangeStandBrickWithPosition(Transform brick)
    {
        if (standBrick == null)
        {
            standBrick = brick;
            int index = standBrick.GetComponent<Brick>().index;
            currentStandOnNode = WorldGraphManager._instance.graph.Nodes[index];

            transform.forward = transform.up;
            transform.position = brick.position + transform.up * brickHeightOffset;
            this.transform.SetParent(brick);

        }
        else if (standBrick != brick)
        {
            this.transform.SetParent(null);
            transform.forward = transform.up;
            transform.position = brick.position + transform.up * brickHeightOffset;

            this.transform.SetParent(brick);


            standBrick = brick;
            int index = standBrick.GetComponent<Brick>().index;
            currentStandOnNode = WorldGraphManager._instance.graph.Nodes[index];
            Debug.Log(currentStandOnNode.index);

        }

    }

    private Node GetNodeByIndex(int index)
    {
        Node n = WorldGraphManager._instance.graph.Nodes[index];
        return n;
    }

    private void ClimbBrickWithAnimation(Transform brick, Vector3 hitPoint)
    {
        if (standBrick != brick && isMoving)
        {
            transform.parent = null;
            StartCoroutine(ClimbAndRotate(hitPoint, 2f));
        }
    }

    private IEnumerator ClimbAndRotate(Vector3 hitpoint, float time)
    {
        isMoving = false;
        isTurning = true;
        Vector3 starPos = transform.position;
        Vector3 finalPos = hitpoint;

        Quaternion startRotation = transform.localRotation;
        Quaternion finalRotation = (transform.localRotation *= Quaternion.Euler(-90, 0, 0));

        float elapsedTime = 0;
        //transform.localRotation *= Quaternion.Euler(-90, 0, 0);
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            transform.localRotation = Quaternion.Lerp(startRotation, finalRotation, (elapsedTime / time));
            transform.position = Vector3.Lerp(starPos, finalPos, (elapsedTime / time));
            yield return null;
        }
        isTurning = false;
        isMoving = true;
    }

    private IEnumerator QRotate(float x, float y, float z, float time)
    {
        isTurning = true;
        isMoving = false;
        Quaternion startRotation = transform.localRotation;
        Quaternion finalRotation = (transform.localRotation *= Quaternion.Euler(x, y, z));
        float elapsedTime = 0;
        //transform.localRotation *= Quaternion.Euler(-90, 0, 0);
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            transform.localRotation = Quaternion.Lerp(startRotation, finalRotation, (elapsedTime / time));
            yield return null;
        }
        isTurning = false;
        isMoving = true;
    }

    //private IEnumerator LookAtRotate(Node nextNode)
    //{
    //    Vector3 nextWorldPos = nextNode.worldPosition + transform.up * brickHeightOffset;
    //    Vector3 direction = (nextWorldPos - this.transform.position);
    //    Quaternion startRotation = transform.localRotation;
    //    Quaternion finalRotation = Quaternion.LookRotation(direction, transform.up);
    //}

    private void Move(Node targetNode)
    {
        if (targetNode == null)
            return;


        if (isMoving)
        {
            if (myPath.Count < 2)
            {
                return;
            }
            Node destination = myPath[1];
            isMoving = !MoveTowardsDone(destination);
            if (!isMoving)
            {
                myPath.RemoveAt(0);
                isMoving = true;
            }
        }
        // Vector3 nextWorldPos = nextNode.worldPosition + transform.up * brickHeightOffset;

    }

    private IEnumerator MoveCrt(Node targetNode)
    {
        while (true)
        {
            myPath = WorldGraphManager._instance.GetPath(currentStandOnNode, targetNode);
            yield return new WaitForSeconds(5.0f);
        }

    }

    private bool MoveTowardsDone(Node nextNode)
    {
        Vector3 nextWorldPos = nextNode.worldPosition + transform.up * brickHeightOffset;
        //Debug.Log(nextWorldPos + ": "+transform.position);
        Vector3 direction = (nextWorldPos - this.transform.position);
        if (direction.sqrMagnitude <= 0.005f)
        {
            transform.position = nextWorldPos;
            Debug.Log(nextWorldPos + ": " + transform.position);
            return true;
        }
        this.transform.position += direction.normalized * moveSpeed * Time.deltaTime;

        //transform.LookAt(nextWorldPos, transform.up);

        return false;
    }

    private bool IsSeeTarget()
    {

        return Look(eyeRayStarPos, 25f, 180f, 10);
    }

    private bool Look(Transform startLookTrans, float lookRange, float lookAngle, int lookAccurate)
    {

        bool isSee = false;
        if (LookAround(startLookTrans, startLookTrans.forward, lookRange, Color.red))
        {
            isSee = true;
        }


        float subAngle = (lookAngle / 2) / lookAccurate;


        for (int i = 0; i < lookAccurate; i++)
        {
            Vector3 rayDirectionRight = (startLookTrans.forward + startLookTrans.right * (i / subAngle)).normalized;
            Vector3 rayDirectionLeft = (startLookTrans.forward - startLookTrans.right * (i / subAngle)).normalized;
            if ((LookAround(startLookTrans, rayDirectionRight, lookRange, Color.red)))
            {
                isSee = true;
            }
            if ((LookAround(startLookTrans, rayDirectionLeft, lookRange, Color.red)))
            {
                isSee = true;
            }

        }

        return isSee;
    }
    public bool LookAround(Transform startLookTrans, Vector3 direction, float lookRange, Color DebugColor)
    {
        Debug.DrawRay(startLookTrans.position, direction * lookRange, DebugColor);

        RaycastHit hit;
        if (Physics.Raycast(startLookTrans.position, direction, out hit, lookRange))
        {
            if (hit.transform.tag == "Player")
                return true;
        }
        return false;
    }
}
