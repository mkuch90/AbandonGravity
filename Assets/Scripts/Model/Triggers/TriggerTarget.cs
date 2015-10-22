using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Logic{
	And,
	Or
}
public class TriggerTarget : MonoBehaviour {

    public float speed = 1f;
	public List<Trigger> triggers;
	public Logic logic = Logic.And;
	protected bool activated;

    public virtual void Start()
    {
        GetComponent<AudioSource>().volume = Constants.effectsVolume/4;
    }

    protected virtual void Update()
    {
		bool shouldBeActive = false;
		foreach(Trigger trigger in triggers){
			if(trigger.IsActivated ()){
				shouldBeActive = true;
				if(this.logic == Logic.Or){
					break;
				}
			}
			else{
				if(logic==Logic.And){
				shouldBeActive = false;
					break;
				}
			}
		}
		if(shouldBeActive && !activated){
			Activate ();
		}
		if(!shouldBeActive && activated){
			Deactivate ();
		}

    }

    public virtual void Activate()
    {
		activated=true;
        GetComponent<AudioSource>().Play();
    }
    public virtual void Deactivate()
    {
		activated=false;
        GetComponent<AudioSource>().Play();
    }
    
    
}
