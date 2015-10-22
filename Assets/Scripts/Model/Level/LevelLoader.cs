using UnityEngine;
using System.Collections;

public class LevelLoader : MonoBehaviour {

    public string NextLevel = "";
    void OnTriggerEnter(Collider col)
    {
        Debug.Log("Not loading next level: " + NextLevel);
        if (col.tag != "Player")
        {
            return;
        }
        if (string.IsNullOrEmpty(NextLevel))
        {
            Debug.LogError("Every LevelLoader should have level specified");
        }
        GetComponent<AudioSource>().Play();
        Debug.Log("Loading next level: " + NextLevel);
        Application.LoadLevel(NextLevel);
    }
    void Start()
    {
        GetComponent<AudioSource>().volume = Constants.effectsVolume/5;
		this.GetComponent<Rigidbody>().angularVelocity = new Vector3(100,100,100);
    }
}
