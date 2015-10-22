using UnityEngine;
using System.Collections;


public class Asteroid : MonoBehaviour {
	static System.Random rand;
	public bool highVelocity = false;
	// Use this for initialization
	void Start () {
		if (rand == null) { rand = new System.Random(3452345); }
		if (rand.Next(1, 100) % 2 ==0) {
			if (!highVelocity)
			{
				this.GetComponent<Rigidbody>().velocity = new Vector3(rand.Next(0, 10) / 10f - 0.5f, rand.Next(0, 10) / 10f - 0.5f);
			}
			else
			{
				this.GetComponent<Rigidbody>().velocity = new Vector3(rand.Next(0, 15) , rand.Next(0, 5));
			}
		}
		
		if (rand.Next(1, 100) % 2 ==0) {
			this.GetComponent<Rigidbody>().angularVelocity = new Vector3(rand.Next(0, 10) / 10f - 0.5f, rand.Next(0, 10) / 10f - 0.5f, rand.Next(0, 10) / 10f - 0.5f);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
