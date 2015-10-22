using UnityEngine;
using System.Collections;
/// <summary>
/// In progeress, this won't actually work. We need to save off the state od the game such as polay an box position.
/// </summary>
public class Checkpoint : MonoBehaviour {

	void OnTriggerEnter(Collider col)
	{
		if (col.tag != "Player")
		{
			return;
		}
		SpawnPoint point = (SpawnPoint)FindObjectOfType(typeof(SpawnPoint));
		point.transform.position = transform.position;
		this.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
