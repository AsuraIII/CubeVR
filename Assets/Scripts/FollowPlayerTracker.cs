using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerTracker : MonoBehaviour {
	public Transform playerTracker;
	public Transform world;
	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		transform.localPosition = playerTracker.localPosition;
		transform.localRotation = new Quaternion(-world.rotation.x,-world.rotation.y,-world.rotation.z,world.rotation.w);
	}
}
