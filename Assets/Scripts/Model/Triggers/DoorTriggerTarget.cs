using UnityEngine;
using System.Collections;
/// <summary>
/// This is the class for controlling how a door behaves when a button is activated.
/// It extends the class triggertarget
/// </summary>
public class DoorTriggerTarget : TriggerTarget {


	private const string closeAnimationName = "Closed";
	private const string openAnimationName = "Open";
	
	public override void Start()
	{
		GetComponent<Animation>()[closeAnimationName].wrapMode = WrapMode.Clamp;
		GetComponent<Animation>()[openAnimationName].wrapMode = WrapMode.Clamp;
	}

	public override void Activate()
	{
		GetComponent<AudioSource>().Play();
		activated=true;
		GetComponent<Animation>().Play(openAnimationName, PlayMode.StopAll);
	}
	public override void Deactivate()
	{
		GetComponent<AudioSource>().Play();
		activated=false;
		GetComponent<Animation>().Play(closeAnimationName, PlayMode.StopAll);
	}
	
}
