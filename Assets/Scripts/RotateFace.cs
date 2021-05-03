using UnityEngine;
using System.Collections;

public class RotateFace : MonoBehaviour
{
    public WorldConfiguration worldConfig;

    // the cube player is controlling
    private Transform smallRubik;

    // the cube player is in
    private Transform bigRubik;

    // cubes of control rubik
    private Transform[] smallRubikRooms;

    // cubes of control rubik
    private Transform[] bigRubikRooms;

    // pivots of control rubik
    private Transform[] smallRubikPivots;

    // pivots of rubik's with player in
    private Transform[] bigRubikPivots;
    public SFXManager sfx;

    private Transform[] smallRubikRoomsToRotate = new Transform[9];
    private Transform smallRubikPivot;

    private Transform[] bigRubikRoomsToRotate = new Transform[9];
    private Transform bigRubikPivot;

    // prevent multiple animations at the same time
    private bool animating = false;

    private void Update()
    {
        
        
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            smallRubikPivot = smallRubikPivots[2];
            bigRubikPivot = bigRubik.Find(smallRubikPivot.name);
            StartCoroutine(AnimateFaceRotation(transform.position));
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            smallRubikPivot = smallRubikPivots[3];
            bigRubikPivot = bigRubik.Find(smallRubikPivot.name);
            StartCoroutine(AnimateFaceRotation(transform.position));
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            smallRubikPivot = smallRubikPivots[0];
            bigRubikPivot = bigRubik.Find(smallRubikPivot.name);
            StartCoroutine(AnimateFaceRotation(transform.position));
        }
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            smallRubikPivot = smallRubikPivots[1];
            bigRubikPivot = bigRubik.Find(smallRubikPivot.name);
            StartCoroutine(AnimateFaceRotation(transform.position));
        }
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            smallRubikPivot = smallRubikPivots[4];
            bigRubikPivot = bigRubik.Find(smallRubikPivot.name);
            StartCoroutine(AnimateFaceRotation(transform.position));
        }
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            smallRubikPivot = smallRubikPivots[5];
            bigRubikPivot = bigRubik.Find(smallRubikPivot.name);
            StartCoroutine(AnimateFaceRotation(transform.position));
        }

    }

    private void FindClosestPivot(Vector3 contactPoint)
    {
        float min = float.MaxValue;
        foreach (Transform p in smallRubikPivots)
        {
            if (Vector3.Distance(contactPoint, p.transform.position) < min)
            {
                min = Vector3.Distance(contactPoint, p.transform.position);
                smallRubikPivot = p;
            }
        }
        // match pivot in big rubic by name (not the most elegant solution)
        bigRubikPivot = bigRubik.Find(smallRubikPivot.name);
    }

    private void FindFaceToRotateByPivot()
    {
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
        // the same for big rubik
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

    }

    void DEBUGHighlightPivot()
    {
        foreach (Renderer rend in smallRubikPivot.GetComponentsInChildren<Renderer>())
        {
            Material originalMaterial = rend.sharedMaterial;
            Material tempMaterial = new Material(originalMaterial);
            rend.material = tempMaterial;
            rend.material.color = Color.red;
        }
    }

    IEnumerator AnimateFaceRotation(Vector3 contactPoint)
    {
        animating = true;

        //FindClosestPivot(contactPoint);

        // DEBUGHighlightPivot ();

        FindFaceToRotateByPivot();

        Quaternion origin = smallRubikPivot.transform.localRotation;
        Quaternion target = smallRubikPivot.transform.localRotation * Quaternion.AngleAxis(90f, smallRubikPivot.transform.localPosition);

        Quaternion bigRubikOrigin = bigRubikPivot.transform.localRotation;
        Quaternion bigRubikTarget = bigRubikPivot.transform.localRotation * Quaternion.AngleAxis(90f, bigRubikPivot.transform.localPosition);

        float journey = 0f;
        float duration = 1f;

        sfx.PlayRandom("CubeFaceRotationSound", bigRubikPivot.position);

        //animating
        while (journey <= duration)
        {
            journey = journey + Time.deltaTime;
            float percent = Mathf.Clamp01(journey / duration);
            smallRubikPivot.localRotation = Quaternion.Slerp(origin, target, percent);
            bigRubikPivot.localRotation = Quaternion.Slerp(bigRubikOrigin, bigRubikTarget, percent);

            yield return null;
        }

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

        GenerateLevel.RecollorPassages();
        animating = false;
    }

    void OnTriggerStay(Collider col)
    {

        if (!animating
            && (col.gameObject.tag == "LIndex" || col.gameObject.tag == "RIndex")
            && OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger) > 0)
        {
            StartCoroutine(AnimateFaceRotation(col.transform.position));
        }
    }

    void Start()
    {
        smallRubik = worldConfig.smallRubik;
        bigRubik = worldConfig.bigRubik;
        smallRubikRooms = worldConfig.smallRubikRooms;
        bigRubikRooms = worldConfig.bigRubikRooms;
        smallRubikPivots = worldConfig.smallRubikPivots;
        bigRubikPivots = worldConfig.bigRubikPivots;
    }
}
