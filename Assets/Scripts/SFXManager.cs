using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour {

	public void PlayRandom(string soundName, Vector3 position){
		List<AudioSource> clips = new List<AudioSource>();
		foreach (AudioSource ac in GetComponentsInChildren<AudioSource>()) {
			if (ac.name.Contains (soundName))
				clips.Add (ac);
		}
		AudioSource.PlayClipAtPoint(clips [Random.Range (0, clips.Count)].clip, position);
	}
}
