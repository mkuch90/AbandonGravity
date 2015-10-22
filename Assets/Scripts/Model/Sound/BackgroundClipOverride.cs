using UnityEngine;
using System.Collections;


[RequireComponent(typeof(AudioSource))]
public class BackgroundClipOverride : MonoBehaviour {

	// Use this for initialization
	void Start()
	{
		Debug.LogWarning("Changed Clip");
		SoundManager.StartNewClip(this.GetComponent<AudioSource>().clip);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
