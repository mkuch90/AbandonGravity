using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class BackgroundMusic : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //DontDestroyOnLoad(this);
	}
    void Update()
    {
        Debug.Log(GetComponent<AudioSource>().volume) ;
    }
	
}
