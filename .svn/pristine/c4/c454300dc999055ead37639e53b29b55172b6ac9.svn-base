// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;

public class DeathTrigger : MonoBehaviour
{

	// Whoever enters the DeathTrigger gets an OnDeath message sent to them.
// They don't have to react to it.
	void OnTriggerEnter (Collider other)
	{
		other.gameObject.SendMessage ("OnDeath", SendMessageOptions.DontRequireReceiver);
	}

// Helper function: Draw an icon in the sceneview so this object gets easier to pick
	void OnDrawGizmos ()
	{
		Gizmos.DrawIcon (transform.position, "Skull And Crossbones Icon.tif");
	}
	
	void Start() {
        this.tag = "Player";
	}
	
}
