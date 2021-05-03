using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;

public class HandAnimation : MonoBehaviour {

	public Transform CubeGrabbedLeftHandPose;
	public Transform NothingGrabbedLeftHandPose;
	public Transform RelaxedLeftHandPose;
	public Transform controllerHandAnchor;
	public Transform cubeHandAnchor;
	public float speed = 0.1f;
	public Transform hand;
	public Transform handTarget;
	public bool   grabbingCube = false;
	bool animatingHand = false;

	void Start () {
	}
		
	IEnumerator AnimateFingers(Transform targetLeftHandPose, Transform currentTarget) {
		float percentage = 0f;
		while (percentage < 1f) {			
			animatingHand = true;
			if (grabbingCube) {
				handTarget.position = Vector3.Lerp (handTarget.position, currentTarget.position, percentage);
				handTarget.rotation = Quaternion.Slerp (handTarget.rotation, currentTarget.rotation, percentage);
			}
			for (int i = 0; i < hand.childCount; i++) {
				hand.GetChild (i).localRotation = Quaternion.Slerp (hand.GetChild (i).localRotation, targetLeftHandPose.GetChild (i).localRotation, percentage);
				//hand.GetChild (i).localPosition = Vector3.Lerp     (hand.GetChild (i).localPosition, targetLeftHandPose.GetChild (i).localPosition, percentage);
			}	

			percentage += speed;

			yield return null;
		}
		animatingHand = false;
		handTarget.position = currentTarget.position;
		handTarget.rotation = currentTarget.rotation;

	}		

	void Update () {

		/*if  ((OVRInput.Get (OVRInput.RawButton.X) && OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger)) || (OVRInput.GetDown (OVRInput.RawButton.X)   && OVRInput.Get(OVRInput.RawButton.LIndexTrigger))) 
		{
			Debug.Log ("Grip started!");
			StopCoroutine ("AnimateFingers");
			StartCoroutine (AnimateFingers(CubeGrabbedLeftHandPose, cubeHandAnchor));
		}

		if  (OVRInput.GetUp(OVRInput.RawButton.LHandTrigger) || OVRInput.GetUp (OVRInput.RawButton.X)) 
		{
			Debug.Log ("Grip released!");
			StopCoroutine ("AnimateFingers");
			StartCoroutine (AnimateFingers(RelaxedLeftHandPose, controllerHandAnchor));
		}*/
	}
}

