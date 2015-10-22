using UnityEngine;
using System.Collections;

public class Trigger : MonoBehaviour {

    public bool triggerEnabled = true;
	protected bool activated;

    protected void Activate()
    {
		activated = true;
    }
    protected void Deactivate()
	{
		
		activated = false;
    }
    public virtual void OnCollisionEnter(Collision col)
	{
		
		Activate();
    }
    
    public virtual void OnCollisionExit(Collision col)
    {
		
		Deactivate();
    }
    public virtual void OnTriggerEnter(Collider col)
    {
        Activate();
    }
    public virtual void OnTriggerExit(Collider col)
    {
        Deactivate();
    }
    public void Start()
    {
		activated = false;
    }
	public bool IsActivated(){
		return activated;
	}

    
}
