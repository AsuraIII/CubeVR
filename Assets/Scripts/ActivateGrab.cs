using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateGrab : MonoBehaviour {

	public HandAnimation handAnim;
	void OnTriggerEnter (Collider col){
		if (col.gameObject.tag == "LIntex" || col.gameObject.tag == "LThumb") {
			handAnim.grabbingCube = true;
		}
	}
	void OnTriggerExit(Collider col){
		if (col.gameObject.tag == "LIntex" || col.gameObject.tag == "LThumb") {
			handAnim.grabbingCube = false;
		}
	}

}
