using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseRune : MonoBehaviour {
	Renderer rend;
	Vector2 off;
	// Use this for initialization
	void Start () {
		rend = GetComponent<Renderer> ();
		off = new Vector2 (Random.Range (0, 8) * 1 / 7f, Random.Range (0, 8) * 1 / 7f);
		rend.material.SetTextureOffset ("_MainTex", off);
		//Debug.Log (off);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
