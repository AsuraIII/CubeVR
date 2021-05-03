using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {

	public Transform trackedPosition;
	public float offset = 0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (trackedPosition.position.x, trackedPosition.position.y - offset, trackedPosition.position.z);
	}
}
