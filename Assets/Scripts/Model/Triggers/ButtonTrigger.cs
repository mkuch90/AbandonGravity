using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// This is a class for pressing a button.
/// </summary>
public class ButtonTrigger : Trigger {
    public float forceCuttoff = 10f;
    public bool debugForce = false;
	private const string buttonAnimationName = "ButtonPress";

    private List<MassObject> colliders;

    protected void ActivateButton()
    {
        if (!triggerEnabled)
        {
            return;
        }
        if (activated)
        {
            return;
        }
		this.GetComponent<Animation>()[buttonAnimationName].speed = 1;
		this.GetComponent<Animation>().Play(buttonAnimationName);
        activated = true;
        GetComponent<AudioSource>().Play();
        Activate();
    }
    protected void DectivateButton()
    {
        if (!activated)
        {
            return;
        }
        if (!triggerEnabled)
        {
            return;
        }
        GetComponent<AudioSource>().Play();
		this.GetComponent<Animation>()[buttonAnimationName].speed = -1;
		this.GetComponent<Animation>().Play(buttonAnimationName);
        activated = false;
        Deactivate();
    }
	/// <summary>
	/// Constantly checks if the collisin force is enough to press the button.
	/// </summary>
    public void Update()
    {
        bool colForceEnough = CollisionForceEnough();
        if (!activated && colForceEnough)
        {
            ActivateButton();
        }
        if (activated && !colForceEnough)
        {
            DectivateButton();
        }
    }
   public override void OnCollisionEnter(Collision col)
    {
        MassObject collider = col.gameObject.GetComponent<LiftableObject>();
        if (collider == null)
        {
            collider = col.gameObject.GetComponent<PlatformerController>();
        }
        if (collider == null)
        {
            return;
        }
        colliders.Add(collider);
    }

   public override void OnCollisionExit(Collision col)
   {
       MassObject collider = col.gameObject.GetComponent<LiftableObject>();
       if (collider == null)
       {
           collider = col.gameObject.GetComponent<PlatformerController>();
       }
       if (collider == null)
       {
           return;
       }
       colliders.Remove(collider);
    }
   public override void OnTriggerEnter(Collider col)
   {
           MassObject collider = col.gameObject.GetComponent<LiftableObject>();
           if (collider == null)
           {
               collider = col.gameObject.GetComponent<PlatformerController>();
           }
           if (collider == null)
           {
               return;
           }
           colliders.Add(collider);
    } 
   public override void OnTriggerExit(Collider col)
   {

       MassObject collider = col.gameObject.GetComponent<LiftableObject>();
       if (collider == null)
       {
           collider = col.gameObject.GetComponent<PlatformerController>();
       }
       if (collider == null)
       {
           return;
       }
       colliders.Remove(collider);
    }

	/// <summary>
	/// Figures out if the sum of the masses on top of the button are enough.
	/// It does not take gravity direction into account.
	/// </summary>
	/// <returns></returns>
   private bool CollisionForceEnough()
   {
       float totalForce = 0;
       if(colliders.Count < 1){
           return false;
       }
       float mass = MassObjectManager.GetMass(colliders);
       totalForce = GravityManager.GetAcceleration(transform.position).magnitude * mass;
       if (debugForce)
       {
           Debug.Log(totalForce);
       }
       return totalForce >= forceCuttoff;
   }
   private new void Start()
   {
       GetComponent<AudioSource>().volume = Constants.effectsVolume;
       colliders = new List<MassObject>();
   }
}
