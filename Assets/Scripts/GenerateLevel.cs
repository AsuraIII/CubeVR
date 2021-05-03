using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateLevel : MonoBehaviour
{
    private WorldConfiguration worldConfig;

    private GameObject sciFiWallPrefab;

    public GameObject monster;

    private Transform bigRubik;
    private Transform smallRubik;

    // cubes of control rubik
    private Transform[] smallRubikRooms;

    // cubes of control rubik
    private Transform[] bigRubikRooms;

    // pivots of control rubik
    private Transform[] smallRubikPivots;

    // pivots of rubik's with player in
    private Transform[] bigRubikPivots;

    private Transform[] smallRubikRoomsToRotate = new Transform[9];
    private Transform smallRubikPivot;

    private Transform[] bigRubikRoomsToRotate = new Transform[9];
    private Transform bigRubikPivot;

    public int NO_OF_PASSAGES;
    public int NO_OF_INITIAL_ROTATIONS;

    public static List<Transform> passages = new List<Transform>();

    public static List<Transform> bigRubikPassages = new List<Transform>();

    void SwitchShaderForSmallRubik()
    {
        foreach (Transform room in smallRubik)
        {
            if (room.tag == "Room")
            {
                foreach (Renderer r in room.GetComponentsInChildren<Renderer>())
                {
                    Shader wireshader = Shader.Find("SuperSystems/Wireframe-Transparent");
                    r.material.shader = wireshader;
                    //r.material.SetInt("_WireThickness", 300);
                    try
                    {
                        r.GetComponent<BoxCollider>().enabled = false;
                    }
                    catch
                    {
                    }
                }
            }
        }
    }

    void SwitchBrickPrefabInBigRubik()
    {
        foreach (Transform room in bigRubik)
        {
            if (room.tag == "Room")
            {
                foreach (Transform wall in room)
                {
                    foreach (Transform brick in wall)
                    {
                        Destroy(brick.GetComponent<MeshRenderer>());
                        Destroy(brick.GetComponent<MeshFilter>());
                        GameObject sciFiWall = Instantiate(sciFiWallPrefab, brick);
                        sciFiWall.transform.localPosition = Vector3.zero;
                        sciFiWall.transform.localRotation = Quaternion.identity;
                    }
                }
            }
        }
    }


    void SelectRandomExit(List<Transform> potentialExits)
    {
        Shader shader = Shader.Find("Unlit/Color");
        int chooseExist = Random.Range(0, potentialExits.Count);
        potentialExits[chooseExist].GetComponent<Renderer>().enabled = false;
        potentialExits[chooseExist].GetChild(0).GetComponent<Renderer>().enabled = false;
        potentialExits[chooseExist].GetComponent<BoxCollider>().isTrigger = true;
        foreach (Transform t in smallRubik)
        {
            if (t.name == potentialExits[chooseExist].parent.parent.name)
                foreach (Transform tt in t)
                    if (tt.name == potentialExits[chooseExist].parent.name)
                        foreach (Transform ttt in tt)
                            if (ttt.name == potentialExits[chooseExist].name)
                            {
                                ttt.GetComponent<Renderer>().material.shader = shader;
                                ttt.GetComponent<Renderer>().material.color = Color.blue;
                            }
        }
    }

    List<Transform> GeneratePassagesRandomly()
    {
        int i = 0;
        List<Transform> potentialExits = new List<Transform>();
        Shader shader = Shader.Find("Unlit/Color");
        while (i < NO_OF_PASSAGES)
        {
            foreach (Transform room in bigRubik)
            {
                if (room.tag == "Room")
                {
                    //find a wall and a brick that is not opened yet
                    Transform randomWall = room.GetChild(Random.Range(0, room.childCount));
                    Transform randomBrick;
                    do
                    {
                        randomBrick = randomWall.GetChild(Random.Range(0, 9));
                    }
                    while (randomBrick.GetComponent<BoxCollider>().isTrigger);
                    // check if it is inner or outher wall of the rubik
                    RaycastHit hit;
                    GameObject neighbourBrick = null;
                    if (Physics.Raycast(randomBrick.position, randomBrick.forward, out hit, 1f) || Physics.Raycast(randomBrick.position, -randomBrick.forward, out hit, 0.5f))
                    {
                        neighbourBrick = hit.collider.gameObject;
                    }
                    // if it is inner wall, create a passage
                    if (neighbourBrick)
                    {
                        neighbourBrick.GetComponent<Renderer>().enabled = false;
                        randomBrick.GetComponent<Renderer>().enabled = false;
                        neighbourBrick.transform.GetChild(0).GetComponent<Renderer>().enabled = false;
                        randomBrick.transform.GetChild(0).GetComponent<Renderer>().enabled = false;

                        neighbourBrick.GetComponent<BoxCollider>().isTrigger = true;
                        randomBrick.GetComponent<BoxCollider>().isTrigger = true;

                        //Add passage to bigRubik passage
                        bigRubikPassages.Add(randomBrick);
                        bigRubikPassages.Add(neighbourBrick.transform);

                        foreach (Transform t in smallRubik)
                        {
                            if (t.name == randomBrick.parent.parent.name)
                                foreach (Transform tt in t)
                                    if (tt.name == randomBrick.parent.name)
                                        foreach (Transform ttt in tt)
                                            if (ttt.name == randomBrick.name)
                                            {
                                                ttt.GetComponent<Renderer>().material.shader = shader;
                                                passages.Add(ttt);
                                            }
                            if (t.name == neighbourBrick.transform.parent.parent.name)
                                foreach (Transform tt in t)
                                    if (tt.name == neighbourBrick.transform.parent.name)
                                        foreach (Transform ttt in tt)
                                            if (ttt.name == neighbourBrick.transform.name)
                                            {
                                                ttt.GetComponent<Renderer>().material.shader = shader;
                                                passages.Add(ttt);
                                            }
                        }
                        i++;
                        if (i >= NO_OF_PASSAGES)
                            break;
                    }
                    else
                    {
                        potentialExits.Add(randomBrick);
                    }
                }
            }
        }
        return potentialExits;

    }

    void RotateFacesRandomly()
    {
        int j = 0;
        while (j < NO_OF_INITIAL_ROTATIONS)
        {
            j++;
            // select random pivot to rotate
            smallRubikPivot = smallRubikPivots[Random.Range(0, smallRubikPivots.Length)];
            // match pivot in big rubic by name (not the most elegant solution)
            bigRubikPivot = bigRubik.Find(smallRubikPivot.name);

            // find face based on chosen pivot
            int i = 0;
            foreach (Transform r in smallRubikRooms)
            {
                r.parent = smallRubik;
                if ((smallRubikPivot.localPosition.z != 0 && (Mathf.Abs(r.localPosition.z - smallRubikPivot.localPosition.z) < 1)) ||
                    (smallRubikPivot.localPosition.x != 0 && (Mathf.Abs(r.localPosition.x - smallRubikPivot.localPosition.x) < 1)) ||
                    (smallRubikPivot.localPosition.y != 0 && (Mathf.Abs(r.localPosition.y - smallRubikPivot.localPosition.y) < 1)))
                {
                    r.parent = smallRubikPivot.transform;
                    smallRubikRoomsToRotate[i++] = r;
                }
            }
            // do the same (find face based on pivot) for big rubik
            i = 0;
            foreach (Transform r in bigRubikRooms)
            {
                r.parent = bigRubik;
                if ((bigRubikPivot.localPosition.z != 0 && (Mathf.Abs(r.localPosition.z - bigRubikPivot.localPosition.z) < 1)) ||
                    (bigRubikPivot.localPosition.x != 0 && (Mathf.Abs(r.localPosition.x - bigRubikPivot.localPosition.x) < 1)) ||
                    (bigRubikPivot.localPosition.y != 0 && (Mathf.Abs(r.localPosition.y - bigRubikPivot.localPosition.y) < 1)))
                {
                    r.parent = bigRubikPivot.transform;
                    bigRubikRoomsToRotate[i++] = r;
                }
            }

            Quaternion origin = smallRubikPivot.transform.localRotation;
            Quaternion target = smallRubikPivot.transform.localRotation * Quaternion.AngleAxis(90f, smallRubikPivot.transform.localPosition);

            Quaternion bigRubikOrigin = bigRubikPivot.transform.localRotation;
            Quaternion bigRubikTarget = bigRubikPivot.transform.localRotation * Quaternion.AngleAxis(90f, bigRubikPivot.transform.localPosition);

            smallRubikPivot.localRotation = target;
            bigRubikPivot.localRotation = bigRubikTarget;

            // reset parent for rooms to rubik (from pivot)
            foreach (Transform r in smallRubikRooms)
            {
                r.parent = smallRubik;
            }
            foreach (Transform r in bigRubikRooms)
            {
                r.parent = bigRubik;
            }
        }
    }

    static public void RecollorPassages()
    {
        foreach (Transform p1 in passages)
        {
            p1.GetComponent<Renderer>().material.color = Color.red;
        }
        foreach (Transform p1 in passages)
        {
            foreach (Transform p2 in passages)
            {
                if ((p1.position - p2.position).magnitude < 0.003 && (p1.position - p2.position).magnitude > 0.001)
                {
                    p1.GetComponent<Renderer>().material.color = Color.green;
                }
            }
        }
    }

    void GenerateMonsterForRoom(int numMax)
    {

        foreach (Transform room in bigRubikRooms)
        {
            int randomNum = Random.Range(0, numMax + 1);

            for (int i = 0; i < randomNum; ++i)
            {
                Vector3 position = RandomPositionXZ(room.position, 4.0f, -4.0f, 4.0f);
                Instantiate(monster, position, Quaternion.identity);
            }
        }
    }

    Vector3 RandomPositionXZ(Vector3 position, float offsetRandomX, float offsetY, float offsetRandomZ)
    {
        float randomX = Random.Range(0, offsetRandomX);
        float randomZ = Random.Range(0, offsetRandomZ);
        Vector3 randomPos = position + new Vector3(randomX, offsetY, randomZ);
        return randomPos;
    }

    void Start()
    {

        worldConfig = GetComponent<WorldConfiguration>();
        smallRubik = worldConfig.smallRubik;
        bigRubik = worldConfig.bigRubik;
        smallRubikRooms = worldConfig.smallRubikRooms;
        bigRubikRooms = worldConfig.bigRubikRooms;
        smallRubikPivots = worldConfig.smallRubikPivots;
        bigRubikPivots = worldConfig.bigRubikPivots;
        sciFiWallPrefab = worldConfig.sciFiWallPrefab;

        SwitchShaderForSmallRubik();

        SwitchBrickPrefabInBigRubik();

        var potentialExits = GeneratePassagesRandomly();

        SelectRandomExit(potentialExits);

        //RotateFacesRandomly();

        RecollorPassages();

        //GenerateMonsterForRoom(0);

        WorldGraphManager._instance.InitializeGraph();
    }

}
