using UnityEngine;
using System.Collections;

public class DeathZone : Trigger {

	new void Activate()
	{

		ResponseManager manager = (ResponseManager)FindObjectOfType(typeof(ResponseManager));
		manager.Death();
	}
	public override void OnCollisionEnter(Collision col)
	{
		if (!triggerEnabled)
		{
			return;
		}
		Activate();
	}

	public override void OnCollisionExit(Collision col)
	{

		
	}
	public override void OnTriggerEnter(Collider col)
	{
		if (!triggerEnabled)
		{
			return;
		}
		Activate();
	}
	public override void OnTriggerExit(Collider col)
	{
		
	}
}
