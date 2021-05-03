using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWorld : MonoBehaviour {

	Transform pivot;

	// prevent multiple animations at the same time
	private bool animating = false;

	IEnumerator AnimateWorldRotation(Vector3 v, Vector3 axis)
	{

		animating = true;

		float journey = 0f;
		float duration = 1f;

		//sfx.PlayRandom ("CubeFaceRotationSound",bigRubikPivot.position);

		Quaternion originalRotation = transform.rotation;
		Vector3 originalPosition = transform.position;
		//animating
		while (journey <= duration)
		{
			transform.RotateAround (v, axis, -90f*Time.deltaTime);

		    journey = journey + Time.deltaTime;
			float percent = Mathf.Clamp01(journey / duration); 
/*			transform.rotation = Quaternion.Slerp(origin, target, percent);
			transform.position += transform.rotation * pivot.position;*/
			yield return null;
		}

		transform.rotation = originalRotation;
		transform.position = originalPosition;
		transform.RotateAround (v, axis, -90f);
		animating = false;
	}

	// if you get a message requesting rotation, rotate the entire would around the pivot
	public void Rotate(Transform piv){
		if (!animating) {

			Vector3 v = piv.position-new Vector3(0,-1f,0);
			Quaternion r = piv.rotation;
			Vector3 axis = Vector3.zero;
			float dotZ = (Vector3.Dot (piv.forward, Vector3.forward));
			float dotX = (Vector3.Dot (piv.forward, Vector3.left));

			if (dotZ > 0.9f)
				axis = Vector3.left;
			if (dotZ < -0.9f)
				axis = Vector3.right;
			if (dotX > 0.9f)
				axis = Vector3.back;
			if (dotX < -0.9f)
				axis = Vector3.forward;

			if (Vector3.Distance(axis,Vector3.zero) > 0.1)
				StartCoroutine (AnimateWorldRotation(v,axis));
		}
	}
}
