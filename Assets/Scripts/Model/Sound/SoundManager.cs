using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour {

	public static AudioClip defaultClip;
	public static AudioClip currentClip;

	// Use this for initialization
	void Start () {
		if (defaultClip == null)
		{
			defaultClip = this.GetComponent<AudioSource>().clip;
		}

		if (currentClip == null)
		{
			currentClip = defaultClip;
		}
		Debug.LogWarning("We can't stop the music");

	}
	public static void StartNewClip(AudioClip clip)
	{
		currentClip = clip;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (!this.GetComponent<AudioSource>().clip.name.Equals(currentClip.name))
		{
			Debug.LogWarning("Actually Changed Clip");
			this.GetComponent<AudioSource>().Stop();
			this.GetComponent<AudioSource>().clip = currentClip;
			this.GetComponent<AudioSource>().Play();
		}
	}
}
