using UnityEngine;
using System.Collections;
public class GravityResponseTarget : MonoBehaviour, ResponseTarget
{
	bool activated;
	bool buttonPressed;
	// Use this for initialization
	void Start()
	{
		buttonPressed = false;
		ResponseManager.AddActivateObject(this);
	}

	public virtual void Activate()
	{
		buttonPressed = true;
	}
	public void Deactivate()
	{
	}
	public Vector3 GetPosition()
	{
		return transform.position;
	}
	public void OnDestroy()
	{
		ResponseManager.RemoveActivateObject(this);
	}
	public void Update()
	{
		
			this.GetComponent<Light>().enabled = activated;
	}
	public void ActivatedCue()
	{
	}
	public void VisualCue()
	{

	}
	public bool WasButtonPressed()
	{
		return buttonPressed;
	}
	public void ResetButtonPressed()
	{
		buttonPressed = false;
	}
	public void SetActive(bool active)
	{
		Debug.Log ("SetActive: "+active);
		this.GetComponent<Light>().enabled=active;
		activated = active;
	}

}

