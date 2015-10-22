using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// Used to debug where the system thinks the ground is visually.
/// </summary>
public class DebugRaycast : MonoBehaviour {


    private PlatformerController controller;
	private List<Vector3> positions;
	public int index = 0;
    public bool source = false;
	public bool forward = false;
	void Start () {
		positions = new List<Vector3>();
		controller = (PlatformerController)FindObjectOfType(typeof(PlatformerController)); //SLOW, only called once
	}
	
	void Update () {
		positions.Clear();
		if (forward)
		{
			if (!source)
			{
				transform.position = controller.HitForward();
			}
			else
			{
				transform.position = controller.SourceForward();
			}
			return;
		}
		if (source)
		{
			positions .AddRange( controller.SourceLocations());

		}
		else
		{
			positions.AddRange(  controller.HitLocations());

		}
		if (index >= positions.Count)
		{
			transform.position = Vector3.zero;
			return;
		}
		transform.position = positions[index];
	}
}
